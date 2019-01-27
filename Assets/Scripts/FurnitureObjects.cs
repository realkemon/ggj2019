﻿using System.Collections.Generic;
using UnityEngine;

public class FurnitureObjects : MonoBehaviour {
    public bool _isBlack;
    public bool IsBlack { get => _isBlack; set { _isBlack = value; outline.color = value ? Color.white : Color.black; fullSprite.color = value ? Color.black : Color.white; } }
    private bool isUncovered;
    public bool IsUncovered { get => isUncovered; set { isUncovered = value; outline.gameObject.SetActive(value); fullSprite.gameObject.SetActive(!value); } }
    public SpriteRenderer fullSprite;
    public SpriteRenderer outline;
    private List<IntersectCollider> intersects;
    new private BoxCollider2D collider;

    public float ColliderHeight => collider.bounds.size.y;
    public float ColliderWidth => collider.bounds.size.x;

    public float angle1 = 30; // swing angle = 2 * angle
    public float speed1 = 3.0f; // speed (6.28 means about 1 second)

    private void OnValidate() {
        IsBlack = IsBlack;
    }

    // Start is called before the first frame update
    void Start() {
        collider = GetComponent<BoxCollider2D>();
        outline.gameObject.SetActive(false);
        intersects = new List<IntersectCollider>();
        IsUncovered = false;
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
                (!IsBlack ? LevelManager.BlackPlayer : LevelManager.WhitePlayer).chain.AddObject(this);
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

    public void SetColour(bool black) {
        IsBlack = black;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);
        if (collider != null) {
            Gizmos.DrawSphere(transform.position + Vector3.right * ColliderWidth * 0.5f, 0.1f);
            Gizmos.DrawSphere(transform.position + Vector3.left * ColliderWidth * 0.5f, 0.1f);
        }
    }
}
