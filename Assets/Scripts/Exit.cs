using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D coll;

    private void Awake() {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void SetOpen(bool open) {
        anim.SetBool("IsOpen", open);
        gameObject.layer = LayerMask.NameToLayer(open ? "Exit" : "Walls");
    }
}
