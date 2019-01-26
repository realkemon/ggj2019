using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGameClick()
    {
        SceneManager.LoadScene("level_01");
    }

    public void LevelsClick()
    {
        SceneManager.LoadScene("LevelsMenu");
    }

    public void OptionsClick()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void QuitClick()
    {

    }
}
