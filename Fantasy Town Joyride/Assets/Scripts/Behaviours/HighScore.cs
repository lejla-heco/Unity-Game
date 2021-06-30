using System.Collections;
using System.Collections.Generic;
using Spacecraft.Core.Entities;
using TMPro;
using UnityEngine;

namespace Spacecraft.Behaviours
{
    public class HighScore : TrackedEntity
    {
        private int Score = 0;
        private int Highscore;
        [SerializeField] private TextMeshProUGUI ScoreText;
        void Start()
        {
            Highscore = PlayerPrefs.GetInt("Highscore", 0);
            DisplayHighScore(Highscore);
        }

        private void DisplayHighScore(int score)
        {
            string ScoreString = score.ToString();
            ScoreText.text = "Highscore: " + ScoreString;
        }

        void Update()
        {
            if (!IsGameOver) StartCoroutine(IncreaseScore());
            if (IsGameOver && Score > Highscore)
            {
                PlayerPrefs.SetInt("Highscore", Score);
            }
        }

        private IEnumerator IncreaseScore()
        {
            Score++;
            yield return new WaitForSeconds(1f);
            if (Score > Highscore)
            {
                DisplayHighScore(Score);
            }
        }
        
    }
}
