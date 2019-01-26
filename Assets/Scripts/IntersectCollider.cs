using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectCollider : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            gameObject.SetActive(false);
        }
    }
}
