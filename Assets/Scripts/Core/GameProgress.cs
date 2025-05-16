using System;
using UnityEngine;
using Zenject;

namespace DoodleLegend.Core
{
    public class GameProgress : IGameProgress, IInitializable
    {
        private const string BestScoreKey = "BestScore";

        public int CurrentScore { get; private set; }
        public int CurrentHeight { get; private set; }
        public int BestScore { get; private set; }

        public event Action<int> OnScoreUpdated = delegate { };
        public event Action<int> OnHeightUpdated = delegate { };
        public event Action<int> OnNewBestScore = delegate { };

        public void Initialize()
        {
            LoadProgress();
        }

        public void AddScore(int amount)
        {
            CurrentScore += amount;
            OnScoreUpdated?.Invoke(CurrentScore);

            if (CurrentScore > BestScore)
            {
                BestScore = CurrentScore;
                OnNewBestScore?.Invoke(BestScore);
            }
        }

        public void UpdateHeight(int newHeight)
        {
            if (newHeight > CurrentHeight)
            {
                CurrentHeight = newHeight;
                OnHeightUpdated?.Invoke(CurrentHeight);
            }
        }

        public void SaveProgress()
        {
            PlayerPrefs.SetInt(BestScoreKey, BestScore);
            PlayerPrefs.Save();
        }

        public void LoadProgress()
        {
            BestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        }

        public void ResetProgress()
        {
            CurrentScore = 0;
            CurrentHeight = 0;
            PlayerPrefs.DeleteKey(BestScoreKey);
        }
    }
}