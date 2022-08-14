using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{
    private TMP_Text scoreField;
    public void SetScoreField(TMP_Text _scoreField)
    {
        scoreField = _scoreField;
    }
    public static string ConvertScoreToString(int score)
    {
        string scoreString;
        if (score < 100)
        {
            scoreString = "000" + score.ToString();
            return scoreString;
        }
        if (score < 1000)
        {
            scoreString = "00" + score.ToString();
            return scoreString;
        }
        if (score < 10000)
        {
            scoreString = "0" + score.ToString();
            return scoreString;
        }
        if (score < 10000)
        {
            scoreString = score.ToString();
            return scoreString; 
        }
        return "00000";
    }
    public void DisplayScore(int score)
    {
        if (score < 100)
        {
            scoreField.text = "000" + score.ToString();
            return;
        }
        if (score < 1000)
        {
            scoreField.text = "00" + score.ToString();
            return;
        }
        if (score < 10000)
        {
            scoreField.text = "0" + score.ToString();
            return;
        }
        if (score < 10000)
        {
            scoreField.text = score.ToString();
            return;
        }
    }
}
