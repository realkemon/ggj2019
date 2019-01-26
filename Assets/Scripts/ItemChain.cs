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
        public float distanceToNext;

        public Timestamp(int jumpstate) {
            this.jumpstate = jumpstate;
            isValid = new List<List<bool>>();
            position = new List<List<Vector3>>();
            distanceToNext = -1f;
        }
    }

    private List<FurnitureObjects> objects;
    private List<LinkedListNode<Timestamp>> currentTimestampPosition;
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
        currentTimestampPosition = new List<LinkedListNode<Timestamp>>();
    }

    // Update is called once per frame
    void Update() {
        if (!transform.position.Equals(lastPosition) || timestamps.Count == 0) {
            Timestamp stamp = new Timestamp(player.IsOnSurface ? Timestamp.WALK : (player.jumpStarted ? Timestamp.JUMP : Timestamp.FALL));
            int mask = LayerMask.GetMask(new string[] { "Walls" });
            List<List<Vector3>> positions = new List<List<Vector3>>();
            List<List<bool>> isValid = new List<List<bool>>();
            for (int width = 1; width < 4; ++width) {
                List<Vector3> widthPos = new List<Vector3>();
                List<bool> widthValid = new List<bool>();
                for (int height = 1; height < 7; ++height) {
                    RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, height, mask);
                    RaycastHit2D hitLowLeft = Physics2D.Raycast(transform.position, Vector3.left, width, mask);
                    RaycastHit2D hitLowRight = Physics2D.Raycast(transform.position, Vector3.right, width, mask);
                    RaycastHit2D hitHighLeft = Physics2D.Raycast(transform.position + Vector3.up * height, Vector3.left, width, mask);
                    RaycastHit2D hitHighRight = Physics2D.Raycast(transform.position + Vector3.up * height, Vector3.right, width, mask);
                    if (hitUp.collider == null && hitLowLeft.collider == null && hitLowRight.collider == null &&
                        hitHighLeft.collider == null && hitHighRight.collider == null) {
                        widthPos.Add(transform.position);
                        widthValid.Add(true);
                    }
                    else {
                        widthPos.Add(transform.position);
                        widthValid.Add(false);
                    }
                }
                positions.Add(widthPos);
                isValid.Add(widthValid);
            }
            stamp.position = positions;
            stamp.isValid = isValid;
            timestamps.AddFirst(stamp);
            if (timestamps.First.Next != null) {
                Timestamp nextStamp = timestamps.First.Next.Value;
                nextStamp.distanceToNext = Vector3.Distance(transform.position, nextStamp.position[0][0]);

                LinkedListNode<Timestamp> currentStamp = timestamps.First;
                float dist = 0;
                for (int i = 0; i < objects.Count; ++i) {
                    while (currentStamp.Next != null && dist < (i + 1) * itemDistance) {
                        currentStamp = currentStamp.Next;
                        dist += currentStamp.Value.distanceToNext;
                    }
                    currentTimestampPosition[i] = currentStamp;
                    objects[i].transform.position = currentStamp.Value.position[Mathf.CeilToInt(objects[i].ColliderWidth * 0.5f)][Mathf.CeilToInt(objects[i].ColliderHeight)];
                }
            }
        }
    }

    public void AddObject(FurnitureObjects obj) {
        objects.Add(obj);
        int idx = objects.Count - 1;
        if (objects.Count > 1) {
            LinkedListNode<Timestamp> lastStamp = currentTimestampPosition[idx - 1];
            float dist = 0f;
            LinkedListNode<Timestamp> temp = lastStamp;
            while (temp != null && temp.Value.distanceToNext > -1) {
                dist += temp.Value.distanceToNext;
                temp = temp.Previous;
            }
            while (lastStamp.Next != null && dist < (idx + 1) * itemDistance) {
                lastStamp = lastStamp.Next;
                dist += lastStamp.Value.distanceToNext;
            }
            currentTimestampPosition.Add(lastStamp);
        }
        else {
            LinkedListNode<Timestamp> lastStamp = timestamps.First;
            float dist = 0f;
            while (lastStamp.Next != null && dist < itemDistance) {
                lastStamp = lastStamp.Next;
                dist += lastStamp.Value.distanceToNext;
            }
            currentTimestampPosition.Add(lastStamp);
        }
        objects[idx].transform.position = currentTimestampPosition[idx].Value.position[Mathf.CeilToInt(objects[idx].ColliderWidth * 0.5f)][Mathf.CeilToInt(objects[idx].ColliderHeight)];
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
