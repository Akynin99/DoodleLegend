using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

namespace DoodleLegend.PlayerInput
{
    public class MobileInputHandler : IInputHandler 
    {
        private float _horizontalSensitivity = 2f;
        private bool _isTouching;
    
        public float GetHorizontal() 
        {
            if (Accelerometer.current != null)
            {
                InputSystem.EnableDevice(Accelerometer.current);
            }
            
            if (Accelerometer.current == null) return 0f;
        
            Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();
            return acceleration.x;
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