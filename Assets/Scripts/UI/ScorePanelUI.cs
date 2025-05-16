using System;
using DoodleLegend.Core;
using TMPro;
using UnityEngine;
using Zenject;

namespace DoodleLegend.UI
{
    public class ScorePanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpScore;

        private IGameProgress _gameProgress;
        
        [Inject]
        public void Construct(IGameProgress gameProgress)
        {
            _gameProgress = gameProgress;
            
            _gameProgress.OnHeightUpdated += OnHeightChanged;
        }

        private void OnHeightChanged(int value)
        {
            tmpScore.text = value.ToString();
        }

        private void OnDisable()
        {
            _gameProgress.OnHeightUpdated -= OnHeightChanged;
        }
    }
}