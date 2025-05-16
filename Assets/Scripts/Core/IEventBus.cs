using System;
using DoodleLegend.PowerUp;

namespace DoodleLegend.Core
{
    public interface IEventBus {
        // Player Events
        void SubscribeToPlayerDeath(Action callback);
        void UnsubscribeFromPlayerDeath(Action callback);
        void PublishPlayerDeath();
    
        // Score Events
        void SubscribeToScoreChanged(Action<int> callback);
        void UnsubscribeFromScoreChanged(Action<int> callback);
        void PublishScoreChanged(int newScore);
    
        // PowerUp Events
        void SubscribeToPowerUpCollected(Action<IPowerUpStrategy> callback);
        void UnsubscribeFromPowerUpCollected(Action<IPowerUpStrategy> callback);
        void PublishPowerUpCollected(IPowerUpStrategy powerUp);
    
        // Универсальный метод для очистки всех подписок
        void ClearAllSubscriptions();
    }
}