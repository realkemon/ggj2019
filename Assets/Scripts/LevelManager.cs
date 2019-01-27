using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private bool CancelInput => Input.GetAxis("Cancel") > 0f;

    public List<FurnitureObjects> blackObjectsNeeded;
    public List<FurnitureObjects> whiteObjectsNeeded;
    public static LevelManager instance;

    public string nextLevel;

    public Exit whiteExit;
    public static Exit WhiteExit => instance.whiteExit;

    public Exit blackExit;
    public static Exit BlackExit => instance.blackExit;

    public PlayerController whitePlayer;
    public static PlayerController WhitePlayer => instance.whitePlayer;

    public PlayerController blackPlayer;
    public static PlayerController BlackPlayer => instance.blackPlayer;

    public bool blackIsDone;
    public static bool BlackIsDone { get => instance != null ? instance.blackIsDone : false; set { if (instance != null) instance.blackIsDone = value; } }

    public bool whiteIsDone;
    public static bool WhiteIsDone { get => instance != null ? instance.whiteIsDone : false; set { if (instance != null) instance.whiteIsDone = value; } }

    private void Awake() {
        instance = this;
        blackIsDone = false;
        whiteIsDone = false;
    }

    private void Update() {
        if (CancelInput)
        {
            SceneManager.LoadScene("MainMenu");
        }

        bool missingPart = false;
        if (!blackIsDone) {
            foreach (FurnitureObjects furn in blackObjectsNeeded) {
                if (furn.isBlack || !furn.IsUncovered) {
                    missingPart = true;
                    break;
                }
            }
        }
        if (!missingPart) {
            foreach (FurnitureObjects furn in whiteObjectsNeeded) {
                if (!furn.isBlack || !furn.IsUncovered) {
                    missingPart = true;
                    break;
                }
            }
        }
        if (!missingPart) {
            whiteExit.SetOpen(true);
            blackExit.SetOpen(true);
        }
        else {
            whiteExit.SetOpen(false);
            blackExit.SetOpen(false);
        }
        if (blackIsDone && whiteIsDone) {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
