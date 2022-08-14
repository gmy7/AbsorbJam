using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreField;
    [SerializeField] private GameObject scoreDisplayGO;

    static ScoreDisplayer scoreDisplay;
    private static int score = 0;
    public static int Score
    {
        get 
        {
            return score; 
        }
        set
        {
            score = value;
            UpdateScore();
        }
    }
    private void Awake()
    {
        GameObject displayer = Instantiate(scoreDisplayGO);
        scoreDisplay = displayer.GetComponent<ScoreDisplayer>();
        scoreDisplay.SetScoreField(scoreField);
    }
    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }
    public static void SaveScore()
    {
        PlayerPrefs.SetInt("HighScore", Score);
    }
    private static void UpdateScore()
    {
        scoreDisplay.DisplayScore(Score);
    }
    public static void DeleteScore()
    {
        PlayerPrefs.SetInt("Score", 0);
    }
}
