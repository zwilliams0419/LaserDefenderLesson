using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

    private Text scoreText;

    public static int score = 0;

	// Use this for initialization
	void Start () {
        ResetScore();
        scoreText = GetComponent<Text>();
        scoreText.text = "Score: " + score;
	}

    public void ChangeScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void ResetScore()
    {
        score = 0;
    }
}
