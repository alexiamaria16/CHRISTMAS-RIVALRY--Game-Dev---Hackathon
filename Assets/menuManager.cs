using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void playButton()
    {
        SceneManager.LoadScene("StartGame");
    }
    public void exitButton()
    {
        Application.Quit();
    }
}
