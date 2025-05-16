using DoodleLegend.Core;
using UnityEngine;

namespace DoodleLegend.PowerUp
{
    public interface IPowerUpStrategy {
        
        void ApplyEffect(PlayerController player);
        
        void CancelEffect();
        
        Sprite Icon { get; }
        
        float Duration { get; }
        
        bool IsStackable { get; }
    }
}