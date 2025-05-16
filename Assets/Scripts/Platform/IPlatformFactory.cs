using UnityEngine;

namespace DoodleLegend.Platform
{
    public interface IPlatformFactory
    {
        PlatformBase CreatePlatform(Vector2 position);
        void ReturnPlatform(PlatformBase platform);
    }
}