using System.Collections.Generic;
using UnityEngine;

public class FurnitureObjects : MonoBehaviour {
    public bool isBlack;
    public bool IsUncovered { get; private set; }
    public SpriteRenderer fullSprite;
    public SpriteRenderer outline;
    private List<IntersectCollider> intersects;
    new private BoxCollider2D collider;

    public float ColliderHeight => collider.bounds.size.y;
    public float ColliderWidth => collider.bounds.size.x;

    public float angle1 = 30; // swing angle = 2 * angle
    public float speed1 = 3.0f; // speed (6.28 means about 1 second)

    private void OnValidate() {
        if (fullSprite != null) {
            fullSprite.color = isBlack ? Color.black : Color.white;
        }
        if (outline != null) {
            outline.color = isBlack ? Color.white : Color.black;
        }
    }

    // Start is called before the first frame update
    void Start() {
        collider = GetComponent<BoxCollider2D>();
        outline.gameObject.SetActive(false);
        intersects = new List<IntersectCollider>();
        foreach (IntersectCollider coll in GetComponentsInChildren<IntersectCollider>()) {
            intersects.Add(coll);
        }
    }

    // Update is called once per frame
    void Update() {
        if (!IsUncovered) {
            bool hasActiveColliders = false;
            foreach (IntersectCollider coll in intersects) {
                if (coll.gameObject.activeSelf) {
                    hasActiveColliders = true;
                    break;
                }
            }
            if (!hasActiveColliders) {
                IsUncovered = true;
                outline.gameObject.SetActive(true);
            }
        }
        else {
            outline.transform.localEulerAngles = new Vector3(0f, 0f, angle1 * Mathf.Sin(speed1 * Time.time));
            fullSprite.transform.localEulerAngles = new Vector3(0f, 0f, angle1 * Mathf.Sin(speed1 * Time.time));
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!IsUncovered) {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
                foreach (IntersectCollider coll in intersects) {
                    coll.gameObject.SetActive(true);
                }
            }
        }
    }

    public void SetColour() {
        //isPlayerOne ? "black" : "white";
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
