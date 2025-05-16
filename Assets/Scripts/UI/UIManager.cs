using System;
using DoodleLegend.Core;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace DoodleLegend.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject gameplayScreen;
        [SerializeField] private GameObject gameOverScreen;
        [SerializeField] private TMP_Text finalScoreText;
        [SerializeField] private string prefix;
        [SerializeField] private Button againButton;
        
        private IEventBus _eventBus;
        private IGameProgress _gameProgress;

        private void Awake()
        {
            gameplayScreen.SetActive(true);
            gameOverScreen.SetActive(false);
            againButton.onClick.AddListener(OnAgainButtonPressed);
        }
        
        [Inject]
        public void Construct(IGameProgress gameProgress, IEventBus eventBus)
        {
            _gameProgress = gameProgress;
            _eventBus = eventBus;
            
            _eventBus.SubscribeToPlayerDeath(OnPlayerDeath);
        }

        private void OnPlayerDeath()
        {
            gameplayScreen.SetActive(false);
            gameOverScreen.SetActive(true);

            finalScoreText.text = prefix + _gameProgress.CurrentHeight.ToString();
        }

        private void OnAgainButtonPressed()
        {
            SceneManager.LoadScene(0);
        }

        private void OnDestroy()
        {
            _eventBus.UnsubscribeFromPlayerDeath(OnPlayerDeath);
            againButton.onClick.RemoveListener(OnAgainButtonPressed);
        }
    }
}