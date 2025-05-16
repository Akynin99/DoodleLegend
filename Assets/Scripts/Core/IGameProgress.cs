using System;

namespace DoodleLegend.Core
{
    public interface IGameProgress
    {
       
        int CurrentScore { get; }
        int CurrentHeight { get; }
        int BestScore { get; }
        event Action<int> OnScoreUpdated;
        event Action<int> OnHeightUpdated;
        event Action<int> OnNewBestScore;
        void AddScore(int amount);
        void UpdateHeight(int newHeight);
        void SaveProgress();
        void LoadProgress();
        void ResetProgress();
    }

}