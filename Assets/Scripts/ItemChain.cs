using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChain : MonoBehaviour
{
    private struct Timestamp {
        public const int WALK = 0;
        public const int JUMP = 1;
        public const int FALL = 2;
        public int jumpstate;
        public List<List<bool>> isValid;
        public List<List<Vector3>> position;

        public Timestamp(int jumpstate) {
            this.jumpstate = jumpstate;
            isValid = new List<List<bool>>();
            position = new List<List<Vector3>>();
        }
    }

    private List<FurnitureObjects> objects;
    private List<LinkedListNode<Timestamp>> currentTimestampPosition;
    public float itemSpace;
    public float distanceToPlayer;
    private PlayerController player;
    private Vector3 lastPosition;
    private LinkedList<Timestamp> timestamps;
    public int maxItems;
    public float itemDistance;
    public int numberOfMaximumCorrectedPositions;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        lastPosition = transform.position;
        objects = new List<FurnitureObjects>();
        timestamps = new LinkedList<Timestamp>();
    }

    // Update is called once per frame
    void Update() {
        if (!transform.position.Equals(lastPosition)) {
            Timestamp stamp = new Timestamp(player.IsOnSurface ? Timestamp.WALK : (player.jumpStarted ? Timestamp.JUMP : Timestamp.FALL));

            timestamps.AddFirst(stamp);

        }
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

    public FurnitureObjects GetObject(int index) {
        return objects != null && index < objects.Count ? objects[index] : null;
    }
}
