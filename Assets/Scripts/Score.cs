using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public int lives = 3; // Initial number of lives
    public TMP_Text livesText; // Reference to the LivesText UI element
    public TMP_Text scoreText; // Reference to the ScoreText UI element

    private float startTime;
    private float score;

    private void Start()
    {
        startTime = Time.time; // Record the start time
        UpdateUI();
    }

    private void Update()
    {
        // Calculate the score based on the time elapsed
        float elapsedTime = Time.time - startTime;
        score = elapsedTime * 100;

        UpdateUI();
    }

    private void UpdateUI()
    {
        // Update the UI Text elements with lives and score
        livesText.text = "Lives: " + lives;
        scoreText.text = "Score: " + (int)score;
    }

    public void LoseLife()
    {
        // Decrease the number of lives and update the UI
        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            // Game over logic (e.g., load a game over scene or restart the level)
        }
    }
}
