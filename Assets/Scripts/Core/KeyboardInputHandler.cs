using UnityEngine;

namespace DoodleLegend.Core
{
    public class KeyboardInputHandler : IInputHandler 
    {
        private const string HorizontalAxis = "Horizontal";
    
        public float GetHorizontal() => Input.GetAxis(HorizontalAxis);
    
        public bool IsJumpPressed() => Input.GetKeyDown(KeyCode.Space);
    
        public bool IsPowerUpActivated() => Input.GetKeyDown(KeyCode.E);
    }
}