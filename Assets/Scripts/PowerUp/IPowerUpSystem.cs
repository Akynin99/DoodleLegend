using DoodleLegend.Core;

namespace DoodleLegend.PowerUp
{
    public interface IPowerUpSystem
    {
        void TryActivatePowerUp(PlayerController player);
        void RegisterPowerUp(IPowerUpStrategy powerUp);
    }
}