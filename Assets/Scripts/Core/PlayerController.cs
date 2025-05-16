using UnityEngine;

namespace DoodleLegend.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour 
    {
        private IInputHandler _input;
        private Rigidbody2D _rb;
    
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpForce = 10f;

        public void Initialize(IInputHandler inputHandler) 
        {
            _input = inputHandler;
            _rb = GetComponent<Rigidbody2D>();
        }

        void Update() 
        {
            float horizontal = _input.GetHorizontal();
            _rb.linearVelocity = new Vector2(horizontal * _moveSpeed, _rb.linearVelocity.y);
            
            if (_input.IsPowerUpActivated()) 
            {
                // PowerUpSystem.Instance.ActivatePowerUp();
            }
        }

        public void Jump() 
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
        }
    }
}