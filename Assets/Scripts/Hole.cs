using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public Vector3 lowerTarget;
    public Vector3 upperTarget;
    public float moveTime;

    public void DropItem(FurnitureObjects obj) {
        obj.SetColour(true);
        obj.ResetColliders();
        obj.IsUncovered = false;
        StartCoroutine(MovementCoroutine(transform.position + upperTarget, transform.position + lowerTarget, obj));
    }

    public void RaiseItem(FurnitureObjects obj) {
        obj.SetColour(false);
        obj.ResetColliders();
        obj.IsUncovered = false;
        StartCoroutine(MovementCoroutine(transform.position + lowerTarget, transform.position + upperTarget, obj));
    }

    IEnumerator MovementCoroutine(Vector3 from, Vector3 to, FurnitureObjects obj) {
        float lerp = 0f;
        while (lerp < 1f) {
            lerp += Time.deltaTime / moveTime;
            lerp = Mathf.Min(1f, lerp);
            obj.transform.position = Vector3.Lerp(from, to, lerp);
            yield return null;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position + lowerTarget, Vector3.one * 0.3f);
        Gizmos.DrawCube(transform.position + upperTarget, Vector3.one * 0.3f);
    }
}
