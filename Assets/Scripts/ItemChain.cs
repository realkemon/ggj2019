using System.Collections.Generic;
using UnityEngine;

public class ItemChain : MonoBehaviour {
    private class Timestamp {
        public const int WALK = 0;
        public const int JUMP = 1;
        public const int FALL = 2;
        public int jumpstate;
        //public List<List<int>> correctionStreak;
        public List<List<Vector3>> position;
        public float distanceToPrevious;
        public int id;
        public Vector3 playerPos;

        public Timestamp(int jumpstate, int id, Vector3 playerPos) {
            this.playerPos = playerPos;
            this.jumpstate = jumpstate;
            this.id = id;
            //correctionStreak = new List<List<int>>();
            position = new List<List<Vector3>>();
            distanceToPrevious = -1f;
        }
    }

    private List<FurnitureObjects> objects;
    private List<LinkedListNode<Timestamp>> currentTimestampPosition;
    private PlayerController player;
    private Vector3 lastPosition;
    private LinkedList<Timestamp> timestamps;

    // Start is called before the first frame update
    void Start() {
        player = GetComponent<PlayerController>();
        lastPosition = transform.position;
        objects = new List<FurnitureObjects>();
        timestamps = new LinkedList<Timestamp>();
        currentTimestampPosition = new List<LinkedListNode<Timestamp>>();
    }

    // Update is called once per frame
    void Update() {
        if (!(transform.position == lastPosition) || timestamps.Count == 0) {
            Timestamp stamp = new Timestamp(player.IsOnSurface ? Timestamp.WALK : (player.jumpStarted ? Timestamp.JUMP : Timestamp.FALL), timestamps.Count > 0 ? timestamps.First.Value.id + 1 : 0, transform.position);
            Timestamp previousStamp = (timestamps.Count > 1 ? timestamps.First.Next.Value : null);
            int mask = LayerMask.GetMask(new string[] { "Walls" });
            List<List<Vector3>> positions = new List<List<Vector3>>();
            for (int width = 1; width < 7; ++width) {
                List<Vector3> widthPos = new List<Vector3>();
                for (int height = 1; height < 7; ++height) {
                    RaycastHit2D hitLowLeft = Physics2D.Raycast(transform.position + 0.1f * Vector3.up, Vector3.left, width * 0.5f, mask);
                    RaycastHit2D hitLowRight = Physics2D.Raycast(transform.position + 0.1f * Vector3.up, Vector3.right, width * 0.5f, mask);
                    RaycastHit2D hitHighLeft = Physics2D.Raycast(transform.position + Vector3.up * (height - 0.1f), Vector3.left, width * 0.5f, mask);
                    RaycastHit2D hitHighRight = Physics2D.Raycast(transform.position + Vector3.up * (height - 0.1f), Vector3.right, width * 0.5f, mask);

                    RaycastHit2D hitLeftUp = Physics2D.Raycast(transform.position + Vector3.up * height * 0.5f + (0.5f * width - 0.1f) * Vector3.left, Vector3.up, height * 0.5f, mask);
                    RaycastHit2D hitRightUp = Physics2D.Raycast(transform.position + Vector3.up * height * 0.5f + (0.5f * width - 0.1f) * Vector3.right, Vector3.up, height * 0.5f, mask);
                    RaycastHit2D hitLeftDown = Physics2D.Raycast(transform.position + Vector3.up * height * 0.5f + (0.5f * width - 0.1f) * Vector3.left, Vector3.down, height * 0.5f, mask);
                    RaycastHit2D hitRightDown = Physics2D.Raycast(transform.position + Vector3.up * height * 0.5f + (0.5f * width - 0.1f) * Vector3.right, Vector3.down, height * 0.5f, mask);
                    if (hitLowLeft.collider == null && hitLowRight.collider == null &&
                        hitHighLeft.collider == null && hitHighRight.collider == null &&
                        (previousStamp != null ? previousStamp.position[width - 1][height - 1] == previousStamp.playerPos : true)) {
                        widthPos.Add(transform.position);
                    }
                    else {
                        Vector3 newPos = transform.position;
                        if (hitLowLeft.collider != null) {
                            if (hitHighLeft.collider != null) {
                                widthPos.Add(transform.position + Vector3.right * (width * 0.5f - Mathf.Min(hitLowLeft.distance, hitHighLeft.distance)));
                            }
                            else if (hitLowRight.collider != null) {
                                widthPos.Add(previousStamp != null ? new Vector3(transform.position.x, previousStamp.position[width - 1][height - 1].y) : transform.position);
                            }
                            else {
                                widthPos.Add(transform.position + Vector3.right * (width * 0.5f - hitLowLeft.distance));
                            }
                        }
                        else if (hitLowRight.collider != null) {
                            if (hitHighRight.collider != null) {
                                widthPos.Add(transform.position + Vector3.left * (width * 0.5f - Mathf.Min(hitLowRight.distance, hitHighRight.distance)));
                            }
                            else {
                                widthPos.Add(transform.position + Vector3.left * (width * 0.5f - hitLowRight.distance));
                            }
                        }
                        else if (hitHighLeft.collider != null) {
                            if (hitHighRight.collider != null) {
                                widthPos.Add(transform.position);
                            }
                            else {
                                widthPos.Add(transform.position);
                            }
                        }
                        else if (hitHighRight.collider != null) {
                            widthPos.Add(transform.position + Vector3.left * (width * 0.5f - hitHighRight.distance));
                        }
                        else {
                            //Debug.Log("no collider is null");
                            widthPos.Add(transform.position);
                        }
                    }
                }
                positions.Add(widthPos);
            }
            stamp.position = positions;
            timestamps.AddFirst(stamp);
            if (timestamps.First.Next != null) {
                Timestamp nextStamp = timestamps.First.Next.Value;
                nextStamp.distanceToPrevious = Vector3.Distance(transform.position, nextStamp.position[0][0]);
            }
        }
        LinkedListNode<Timestamp> currentStamp = timestamps.First;
        float dist = 0;
        LinkedList<int> objectsToRemove = new LinkedList<int>();
        for (int i = 0; i < objects.Count; ++i) {
            while (currentStamp.Next != null && (dist < (i + 1) * GlobalGameParameters.ItemDistance || currentStamp.Value.jumpstate != Timestamp.WALK)) {
                currentStamp = currentStamp.Next;
                dist += currentStamp.Value.distanceToPrevious;
            }
            if (currentStamp.Value.id > currentTimestampPosition[i].Value.id) {
                currentTimestampPosition[i] = currentTimestampPosition[i].Previous;
                if (Vector3.Distance(currentTimestampPosition[i].Value.position[Mathf.CeilToInt(objects[i].ColliderWidth) - 1][Mathf.CeilToInt(objects[i].ColliderHeight) - 1], currentTimestampPosition[i].Value.playerPos) > GlobalGameParameters.MaximumItemDistance) {
                    objectsToRemove.AddFirst(i);
                }
                else {
                    objects[i].transform.position = currentTimestampPosition[i].Value.position[Mathf.CeilToInt(objects[i].ColliderWidth) - 1][Mathf.CeilToInt(objects[i].ColliderHeight) - 1];
                }
            }
        }
        while (currentStamp.Next != null && dist < GlobalGameParameters.MaxItems * GlobalGameParameters.ItemDistance) {
            currentStamp = currentStamp.Next;
            dist += currentStamp.Value.distanceToPrevious;
        }
        while (currentStamp != timestamps.Last) {
            timestamps.Remove(currentStamp.Next);
        }
        foreach (int idx in objectsToRemove) {
            RemoveObject(idx);
        }
        lastPosition = transform.position;
    }

    public void AddObject(FurnitureObjects obj) {
        objects.Add(obj);
        int idx = objects.Count - 1;
        if (objects.Count > 1) {
            LinkedListNode<Timestamp> lastStamp = currentTimestampPosition[idx - 1];
            float dist = 0f;
            LinkedListNode<Timestamp> temp = lastStamp;
            while (temp != null && temp.Value.distanceToPrevious > -1) {
                dist += temp.Value.distanceToPrevious;
                temp = temp.Previous;
            }
            while (lastStamp.Next != null && dist < (idx + 1) * GlobalGameParameters.ItemDistance) {
                lastStamp = lastStamp.Next;
                dist += lastStamp.Value.distanceToPrevious;
            }
            currentTimestampPosition.Add(lastStamp);
        }
        else {
            LinkedListNode<Timestamp> lastStamp = timestamps.First;
            float dist = 0f;
            while (lastStamp.Next != null && dist < GlobalGameParameters.ItemDistance) {
                lastStamp = lastStamp.Next;
                dist += lastStamp.Value.distanceToPrevious;
            }
            currentTimestampPosition.Add(lastStamp);
        }
        Debug.Log(idx + " " + Mathf.CeilToInt(objects[idx].ColliderWidth * 0.5f) + " " + Mathf.CeilToInt(objects[idx].ColliderHeight));
        objects[idx].transform.position = currentTimestampPosition[idx].Value.position[Mathf.CeilToInt(objects[idx].ColliderWidth * 0.5f) - 1][Mathf.CeilToInt(objects[idx].ColliderHeight) - 1];
    }

    public void RemoveObject(FurnitureObjects obj) {
        int idx = objects.IndexOf(obj);
        if (idx > -1)
            RemoveObject(idx);
    }

    public void RemoveObject(int idx) {
        objects.RemoveAt(idx);
        currentTimestampPosition.RemoveAt(idx);
    }

    public FurnitureObjects GetObject(int index) {
        return objects != null && index < objects.Count ? objects[index] : null;
    }
}
