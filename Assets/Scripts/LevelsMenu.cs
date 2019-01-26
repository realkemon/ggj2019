using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void lvl1Click()
    {
        SceneManager.LoadScene("level_01");
    }

    public void lvl2Click()
    {
        SceneManager.LoadScene("level_02");
    }

    public void lvl3Click()
    {
        SceneManager.LoadScene("level_03");
    }

    public void lvl4Click()
    {
        SceneManager.LoadScene("level_04");
    }

    public void lvl5Click()
    {
        SceneManager.LoadScene("level_05");
    }

    public void lvl6Click()
    {
        SceneManager.LoadScene("level_06");
    }

    public void lvl7Click()
    {
        SceneManager.LoadScene("level_07");
    }

    public void BackClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
