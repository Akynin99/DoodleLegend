using UnityEngine;

namespace DoodleLegend.PlayerInput
{
    public class MobileInputHandler : IInputHandler 
    {
        private float _horizontalSensitivity = 2f;
        private bool _isTouching;
    
        public float GetHorizontal() 
        {
            return Mathf.Clamp(Input.acceleration.x * _horizontalSensitivity, -1f, 1f);
        }

        public bool IsJumpPressed() 
        {
            return false; 
        }

        public bool IsPowerUpActivated() 
        {
            if (Input.touchCount > 0) 
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began) {
                    return true;
                }
            }
            return Input.GetMouseButtonDown(0); 
        }
    }
}