using UnityEngine;

public class PlayerController : MonoBehaviour {
    public bool isPlayerOne;
    public float jumpSpeed;
    public float maxWalkSpeed;

    private float HorizontalInput => Input.GetAxis(isPlayerOne ? "Horizontal1" : "Horizontal2");
    private float VerticalInput => Input.GetAxis(isPlayerOne ? "Vertical1" : "Vertical2");
    private float InteractInput => Input.GetAxis(isPlayerOne ? "Interact1" : "Interact2");
    private float DropInput => Input.GetAxis(isPlayerOne ? "Drop1" : "Drop2");

    new private Rigidbody2D rigidbody;
    private bool isOnSurface;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        Vector2 vel = rigidbody.velocity;
        vel.x = maxWalkSpeed * HorizontalInput;

        if (VerticalInput > 0f && isOnSurface) {
            vel.y = jumpSpeed;
        }
        rigidbody.velocity = vel;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walkable")) {
            isOnSurface = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walkable")) {
            isOnSurface = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walkable")) {
            isOnSurface = true;
        }
    }
}
