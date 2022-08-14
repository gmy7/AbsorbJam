using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameSystem
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject endGameScreen;
        [SerializeField] private TMP_Text currentScoreText;
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private GameObject leaderboardGO;

        public delegate void GameEnded();
        public GameEnded gameEnded;
        public void GameOver()
        {
            gameEnded();
            endGameScreen.SetActive(true);
            currentScoreText.text = ScoreDisplayer.ConvertScoreToString(ScoreHandler.Score);
            int highScore = ScoreHandler.GetHighScore();
            if (highScore < ScoreHandler.Score)
            {
                ScoreHandler.SaveScore();
                StartCoroutine(leaderboardGO.GetComponent<Leaderboard>().SubmitScoreRoutine(ScoreHandler.GetHighScore()));
            }
            highScoreText.text = ScoreDisplayer.ConvertScoreToString(ScoreHandler.GetHighScore());
            StartCoroutine(leaderboardGO.GetComponent<Leaderboard>().FetchTopHighScoresRoutine());

        }
    }
}

