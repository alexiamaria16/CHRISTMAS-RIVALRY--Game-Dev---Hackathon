using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameStarter : MonoBehaviour
{   
    public TMP_InputField player1Input;
    public TMP_InputField player2Input;
    public void Play()
    {
        if (player1Input.text != null && player2Input.text != null)
        {
            int index = 0;
            if (PlayerPrefs.GetInt("TeamNumber") > 0)
                index = PlayerPrefs.GetInt("TeamNumber");

            Debug.Log("NUMAR ECHIPE LB" + index);

            PlayerPrefs.SetString("P1_Name"+index.ToString(), player1Input.text);
            PlayerPrefs.SetString("P2_Name"+ index.ToString(), player2Input.text);

            PlayerPrefs.SetString("P1_Name", player1Input.text);
            PlayerPrefs.SetString("P2_Name", player2Input.text);

            PlayerPrefs.SetInt("TeamNumber",index + 1);
            SceneManager.LoadScene("MainScene");
        }
    }
}
