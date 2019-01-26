using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChain : MonoBehaviour
{
    private List<FurnitureObjects> objects;
    private List<Vector3> positions;
    private int needJump = -1;
    private List<Vector3> jumpsFrom;
    private List<Vector3> jumpsTo;
    public float itemSpace;
    public float distanceToPlayer;
    public int maxItems;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        objects = new List<FurnitureObjects>();
        positions = new List<Vector3>();
        for (int i = 0; i < maxItems; ++i) {
            positions.Add(player.transform.position);
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void AddObject(FurnitureObjects obj) {
        objects.Add(obj);
    }

    public void RemoveObject(FurnitureObjects obj) {
        int idx = objects.IndexOf(obj);
        if (idx > -1)
            RemoveObject(idx);
    }

    public void RemoveObject(int idx) {
        objects.RemoveAt(idx);
    }

    public void AddJump(Vector3 from, Vector3 to) {

    }

    public FurnitureObjects GetObject(int index) {
        return objects != null && index < objects.Count ? objects[index] : null;
    }
}
