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
    public void DisplayScore(int score)
    {
        scoreField.text = score.ToString();
    }
}
