using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //public List<FurnitureObject> blackObjectsNeeded;
    //public List<FurnitureObject> whiteObjectsNeeded;
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
}
