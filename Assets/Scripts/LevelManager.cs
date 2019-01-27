using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<FurnitureObjects> blackObjectsNeeded;
    public List<FurnitureObjects> whiteObjectsNeeded;
    public static LevelManager instance;

    public Exit whiteExit;
    public static Exit WhiteExit => instance.whiteExit;

    public Exit blackExit;
    public static Exit BlackExit => instance.blackExit;

    public PlayerController whitePlayer;
    public static PlayerController WhitePlayer => instance.whitePlayer;

    public PlayerController blackPlayer;
    public static PlayerController BlackPlayer => instance.blackPlayer;

    private void Awake() {
        instance = this;
    }

    private void Update() {
        bool missingPart = false;
        foreach (FurnitureObjects furn in blackObjectsNeeded) {
            if (!furn.isBlack) {
                missingPart = true;
                break;
            }
        }
        if (!missingPart) {
            foreach (FurnitureObjects furn in whiteObjectsNeeded) {
                if (furn.isBlack) {
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
    }
}
