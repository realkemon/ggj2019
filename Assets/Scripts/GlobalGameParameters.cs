using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameParameters : MonoBehaviour
{
    private static GlobalGameParameters instance;

    public float jumpHeight;
    public static float JumpHeight => instance != null ? instance.jumpHeight : 0f;

    public float jumpWidth;
    public static float JumpWidth => instance != null ? instance.jumpWidth : 0f;

    public float maxWalkSpeed;
    public static float MaxWalkSpeed => instance != null ? instance.maxWalkSpeed : 0f;

    public int maxItems;
    public static int MaxItems => instance != null ? instance.maxItems : 0;

    public float itemDistance;
    public static float ItemDistance => instance != null ? instance.itemDistance : 0f;

    public float maximumItemDistance;
    public static float MaximumItemDistance => instance != null ? instance.maximumItemDistance : 0f;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (jumpWidth != 0 && maxWalkSpeed != 0) {
                float temp = jumpWidth / maxWalkSpeed;
                float gravity = -Mathf.Abs(jumpHeight / (0.5f * temp * temp - temp));
                Debug.Log(gravity);
                Physics2D.gravity = new Vector2(0f, gravity);
            }
        }
        else {
            Destroy(gameObject);
        }
    }
}
