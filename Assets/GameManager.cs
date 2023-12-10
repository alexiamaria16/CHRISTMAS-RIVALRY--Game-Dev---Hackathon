using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float player1_score = 0f;
    public float player2_score = 0f;

    

    public GameObject endPanel, player1, player2;
    public Transform winTransform;

    public string player1_name = "Player Left";
    public string player2_name = "Player Right";

    public TMP_Text player1_nameText,player1_scoreText, player1_endScore, player1_endName;
    public TMP_Text player2_nameText,player2_scoreText, player2_endScore, player2_endName;
    public TMP_Text timerText, winnerName;

    public float bonus = 1f;

    private float timerDuration = 150f; // 3 minutes in seconds
    private float currentTime;

    public List<string> players = new List<string>();
    public List<float> players_score = new List<float>();
    private void Start()
    {
        

        if (PlayerPrefs.GetString("P1_Name").Length > 0)
            player1_name = PlayerPrefs.GetString("P1_Name");
        if (PlayerPrefs.GetString("P2_Name").Length > 0)
            player2_name = PlayerPrefs.GetString("P2_Name");

        player1_scoreText.text = "Score: " + player1_score.ToString();
        player2_scoreText.text = "Score: " + player2_score.ToString();

        player1_nameText.text = player1_name;
        player2_nameText.text = player2_name;
        // Initialize timer
        currentTime = timerDuration;
        UpdateTimerText();

        // Start the timer
        InvokeRepeating("UpdateTimer", 1f, 1f); // Call UpdateTimer every 1 second, starting after 1 second
    }

    private void UpdateTimer()
    {
        // Update the timer and check if it reaches zero
        if (currentTime > 0)
        {
            currentTime -= 1;
            UpdateTimerText();
        }
        else
        {
            Time.timeScale = 0;
            if (player1_score > player2_score)
            {
                winnerName.text = player1_name + " Won!";
                player1.GetComponent<player>().enabled = false;
                player1.GetComponent<CharacterController>().enabled = false;
                player1.transform.position = new Vector3(player1.transform.position.x, 18, player1.transform.position.z);
                player1.transform.localScale = new Vector3(15, 15, 15);
                
                player1.GetComponent<Animator>().SetBool("Win", true);

            }
            else if (player2_score > player1_score)
            {
                winnerName.text = player2_name + " Won!";
                player2.GetComponent<player>().enabled = false;
                player2.GetComponent<CharacterController>().enabled = false;
                player2.transform.position = new Vector3(player1.transform.position.x, 18, player1.transform.position.z);
                player2.transform.localScale = new Vector3(15, 15, 15);
                player2.GetComponent<Animator>().SetBool("Win", true);
            }
            else
                winnerName.text = "Draw!";

            int index = PlayerPrefs.GetInt("TeamNumber");
            for ( int i = 0; i < index; i++ )
            {
                players.Add(PlayerPrefs.GetString("P1_Name" + i.ToString()));
                players.Add(PlayerPrefs.GetString("P2_Name" + i.ToString()));
            }
            for ( int i = 0;  i < players.Count; i++ )
            {
                players_score.Add(PlayerPrefs.GetFloat(players[i]));
            }
            endPanel.SetActive(true);
            endPanel.GetComponent<Animator>().enabled=true;

            if ( player1_score > PlayerPrefs.GetFloat(player1_name) )
            {
                PlayerPrefs.SetFloat(player1_name, player1_score);
            }
            if (player2_score > PlayerPrefs.GetFloat(player2_name))
            {
                PlayerPrefs.SetFloat(player2_name, player2_score);
            }
            CancelInvoke("UpdateTimer"); // Stop the timer updates
        }
    }

    private void UpdateTimerText()
    {
        // Format the time as minutes:seconds
        string minutes = Mathf.Floor(currentTime / 60).ToString("00");
        string seconds = (currentTime % 60).ToString("00");
        timerText.text = "Time Left: " + minutes + ":" + seconds;
    }

    public void receiveOrder(string tag, float value, bool player1)
    {
        float total = value * bonus;

        Debug.Log("ADD: " + value.ToString() + " FOR PLAYER");
        if (player1)
        {
            player1_score += total;
            player1_scoreText.text = "Score: " + player1_score.ToString();
        }
        else
        {
            player2_score += total;
            player2_scoreText.text = "Score: " + player2_score.ToString();
        }
    }
}
