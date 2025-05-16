
using UnityEngine;

namespace DoodleLegend.Core
{
    public interface ICameraController
    {
        void ResetCamera();
        void SetCameraEnabled(bool enabled);
        Vector2 CurrentPosition();
    }
}