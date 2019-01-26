using UnityEngine;

public class PlayerController : MonoBehaviour {
    public bool isPlayerOne;

    private float HorizontalInput => Input.GetAxis(isPlayerOne ? "Horizontal1" : "Horizontal2");
    private float VerticalInput => Input.GetAxis(isPlayerOne ? "Vertical1" : "Vertical2");
    private bool InteractInput => Input.GetAxis(isPlayerOne ? "Interact1" : "Interact2") > 0f;
    private bool DropInput => Input.GetAxis(isPlayerOne ? "Drop1" : "Drop2") > 0f;
    private bool CrouchInput => Input.GetAxis(isPlayerOne ? "Crouch1" : "Crouch2") > 0f;
    private float JumpSpeed => Mathf.Sqrt(Mathf.Abs(GlobalGameParameters.JumpHeight * Physics2D.gravity.y * 2f));

    new private Rigidbody2D rigidbody;
    public Vector3 Velocity => rigidbody.velocity;
    public bool IsOnSurface { get; private set; }
    private bool jumpStarted;
    private Vector3 jumpFrom;

    public SpriteRenderer sr;
    public BoxCollider2D box;
    private ItemChain chain;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        chain = GetComponent<ItemChain>();
    }

    // Update is called once per frame
    void Update() {
        Vector2 vel = rigidbody.velocity;
        vel.x = GlobalGameParameters.MaxWalkSpeed * HorizontalInput;

        if (!jumpStarted && IsOnSurface && VerticalInput > 0f) {
            vel.y = JumpSpeed;
            jumpFrom = transform.position;
            Debug.Log("JumpFrom: " + jumpFrom);
            jumpStarted = true;
        }
        rigidbody.velocity = vel;

        if (InteractInput) {

        }
        if (CrouchInput) {
            box.offset = new Vector2(0f, 1f);
            box.size = new Vector2(2f, 2f);
            sr.transform.localScale = new Vector3(24f, 24f, 1f);
            sr.transform.localPosition = new Vector3(0f, 1f, 0f);
        }
        else {
            box.offset = new Vector2(0f, 2f);
            box.size = new Vector2(2f, 4f);
            sr.transform.localScale = new Vector3(24f, 48f, 1f);
            sr.transform.localPosition = new Vector3(0f, 2f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (jumpStarted && !IsOnSurface && collision.gameObject.layer == LayerMask.NameToLayer("Walls")) {
            Vector3 contactPoint = collision.contacts[0].point;
            if (contactPoint.y <= transform.position.y) {
                chain.AddJump(jumpFrom, transform.position);
                Debug.Log("JumpTo: " + transform.position);
                IsOnSurface = true;
                jumpStarted = false;
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
