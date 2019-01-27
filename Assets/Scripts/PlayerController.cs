using UnityEngine;

public class PlayerController : MonoBehaviour {
    public bool isPlayerOne;

    private float HorizontalInput => Input.GetAxis(isPlayerOne ? "Horizontal1" : "Horizontal2");

    private float lastVertical;
    private float VerticalInputVal => Input.GetAxis(isPlayerOne ? "Vertical1" : "Vertical2");
    private bool VerticalInput => VerticalInputVal > 0 && lastVertical == 0f;

    private float lastInteract;
    private float InteractInputVal => Input.GetAxis(isPlayerOne ? "Interact1" : "Interact2");
    private bool InteractInput => InteractInputVal > 0f && lastInteract == 0f;

    private bool DropInput => Input.GetAxis(isPlayerOne ? "Drop1" : "Drop2") > 0f;

    private float lastCrouch;
    private float CrouchInputVal => Input.GetAxis(isPlayerOne ? "Crouch1" : "Crouch2");
    private bool CrouchInput => CrouchInputVal > 0f;

    private float lastSelect;
    private float SelectInputVal => Input.GetAxis(isPlayerOne ? "Select1" : "Select2");
    private bool SelectInput => SelectInputVal > 0f && lastSelect == 0f;

    private float JumpSpeed => Mathf.Sqrt(Mathf.Abs(GlobalGameParameters.JumpHeight * Physics2D.gravity.y * 2f));

    new private Rigidbody2D rigidbody;
    public Vector3 Velocity => rigidbody.velocity;
    public bool IsOnSurface { get; private set; }
    public bool jumpStarted;
    public float airMoveFactor = 0.3f;
    
    public BoxCollider2D box;
    public ItemChain chain;
    public Animator anim;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        chain = GetComponent<ItemChain>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (!(isPlayerOne ? LevelManager.BlackIsDone : LevelManager.WhiteIsDone)) {
            Vector2 vel = rigidbody.velocity;
            if (HorizontalInput != 0f) {
                float temp = vel.x + airMoveFactor * Time.deltaTime * HorizontalInput;
                anim.SetBool("IsWalking", HorizontalInput != 0f);
                anim.transform.localScale = new Vector3(-Mathf.Sign(HorizontalInput) * Mathf.Abs(anim.transform.localScale.x), 1f, 1f);
                vel.x = IsOnSurface
                    ? GlobalGameParameters.MaxWalkSpeed * HorizontalInput
                    : Mathf.Sign(temp) * Mathf.Min(GlobalGameParameters.MaxWalkSpeed, Mathf.Abs(temp));
            }

            if (!jumpStarted && IsOnSurface && VerticalInput) {
                vel.y = JumpSpeed;
                jumpStarted = true;
            }
            rigidbody.velocity = vel;

            if (InteractInput) {

            }
            if (SelectInput) {
                if (chain.Count == 0) {
                    chain.SelectObject(-1);
                }
                else if (chain.SelectedItem == -1 && chain.Count > 0) {
                    chain.SelectObject(0);
                }
                else if (chain.SelectedItem == chain.Count - 1) {
                    chain.SelectObject(-1);
                }
                else {
                    chain.SelectObject(chain.SelectedItem + 1);
                }
            }
            if (CrouchInput) {
                box.offset = new Vector2(0f, 1f);
                box.size = new Vector2(2f, 2f);
                anim.SetBool("IsCrouching", true);
            }
            else {
                box.offset = new Vector2(0f, 2f);
                box.size = new Vector2(2f, 4f);
                anim.SetBool("IsCrouching", false);
            }
        }
        lastSelect = SelectInputVal;
        lastCrouch = CrouchInputVal;
        lastInteract = InteractInputVal;
        lastVertical = VerticalInputVal;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (jumpStarted && !IsOnSurface && collision.gameObject.layer == LayerMask.NameToLayer("Walls")) {
            Vector3 contactPoint = collision.contacts[0].point;
            if (contactPoint.y <= transform.position.y) {
                IsOnSurface = true;
                jumpStarted = false;
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Exit")) {
            if (isPlayerOne) {
                LevelManager.BlackIsDone = true;
            }
            else {
                LevelManager.WhiteIsDone = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (IsOnSurface && collision.gameObject.layer == LayerMask.NameToLayer("Walls")) {
            IsOnSurface = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        Vector3 contactPoint = collision.contacts[0].point;
        if (!IsOnSurface && contactPoint.y <= transform.position.y && collision.gameObject.layer == LayerMask.NameToLayer("Walls")) {
            IsOnSurface = true;
        }
    }
}
