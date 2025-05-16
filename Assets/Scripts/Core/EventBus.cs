using System;
using DoodleLegend.PowerUp;
using Zenject;

namespace DoodleLegend.Core
{
    public class EventBus : IEventBus, IInitializable {
        // Player Events
        private Action _onPlayerDeath;
    
        // Score Events
        private Action<int> _onScoreChanged;
    
        // PowerUp Events
        private Action<IPowerUpStrategy> _onPowerUpCollected;

        public void Initialize() {
            // Инициализация при старте
        }

        // Player Death
        public void SubscribeToPlayerDeath(Action callback) => _onPlayerDeath += callback;
        public void UnsubscribeFromPlayerDeath(Action callback) => _onPlayerDeath -= callback;
        public void PublishPlayerDeath() => _onPlayerDeath?.Invoke();

        // Score Changed
        public void SubscribeToScoreChanged(Action<int> callback) => _onScoreChanged += callback;
        public void UnsubscribeFromScoreChanged(Action<int> callback) => _onScoreChanged -= callback;
        public void PublishScoreChanged(int newScore) => _onScoreChanged?.Invoke(newScore);

        // PowerUp Collected
        public void SubscribeToPowerUpCollected(Action<IPowerUpStrategy> callback) => _onPowerUpCollected += callback;
        public void UnsubscribeFromPowerUpCollected(Action<IPowerUpStrategy> callback) => _onPowerUpCollected -= callback;
        public void PublishPowerUpCollected(IPowerUpStrategy powerUp) => _onPowerUpCollected?.Invoke(powerUp);

        public void ClearAllSubscriptions() {
            _onPlayerDeath = null;
            _onScoreChanged = null;
            _onPowerUpCollected = null;
        }
    }
}