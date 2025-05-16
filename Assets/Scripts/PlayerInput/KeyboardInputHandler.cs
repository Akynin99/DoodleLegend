using UnityEngine;
using UnityEngine.InputSystem;

namespace DoodleLegend.PlayerInput
{
    public class KeyboardInputHandler : IInputHandler 
    {
        private const string HorizontalAxis = "Horizontal";
    
        public float GetHorizontal()
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                return -1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                return 1;
            
            return 0;
        }

        public bool IsJumpPressed() => Keyboard.current.spaceKey.isPressed;
    
        public bool IsPowerUpActivated() => Keyboard.current.eKey.isPressed;
    }
}