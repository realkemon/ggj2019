using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public void DropItem(FurnitureObjects obj) {
        obj.SetColour(true);
        obj.IsUncovered = false;
    }

    public void RaiseItem(FurnitureObjects obj) {
        obj.SetColour(true);
        obj.IsUncovered = false;
    }
}
