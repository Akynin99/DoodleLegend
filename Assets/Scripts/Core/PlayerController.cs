using System;
using DoodleLegend.Platform;
using DoodleLegend.PlayerInput;
using DoodleLegend.PowerUp;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DoodleLegend.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        // Movement parameters
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private float maxHorizontalSpeed = 10f;
        [SerializeField] private float airControlMultiplier = 0.5f;
        [SerializeField] private float maxHorizontalOffset = 3;
        [SerializeField] private GameObject defaultSpriteRenderer;
        [SerializeField] private GameObject swappedSpriteRenderer;
        
        // Dependency injections
        private IInputHandler _inputHandler;
        private IPowerUpSystem _powerUpSystem;
        private IEventBus _eventBus;
        private IGameProgress _gameProgress;

        // Components
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;

        // State flags
        private bool _isGrounded = false;
        private bool _isDead = false;
        private UnityEngine.Camera _camera;
        private float _initialHeight;
        private bool _swappedScale;

        [Inject]
        public void Construct(IInputHandler inputHandler, IPowerUpSystem powerUpSystem, IEventBus eventBus, IGameProgress gameProgress)
        {
            _inputHandler = inputHandler;
            _powerUpSystem = powerUpSystem;
            _eventBus = eventBus;
            _gameProgress = gameProgress;
            
            _camera = Camera.main;
            defaultSpriteRenderer.SetActive(true);
            swappedSpriteRenderer.SetActive(false);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            _initialHeight = transform.position.y;
        }

        private void Update()
        {
            if (_isDead) return;

            HandleMovement();
            HandlePowerUpActivation();
            CheckDeathCondition();
        }

        private void FixedUpdate()
        {
            _collider.enabled = _rigidbody.linearVelocity.y < 0;
        }

        private void HandleMovement()
        {
            float horizontalInput = _inputHandler.GetHorizontal();
            bool isTryingToJump = _inputHandler.IsJumpPressed();
            
            UpdateSpriteRotation(horizontalInput);

            // Air control adjustment
            float speedMultiplier = _isGrounded ? 1f : airControlMultiplier;

            // Horizontal movement
            float targetVelocity = horizontalInput * moveSpeed * speedMultiplier;
            _rigidbody.linearVelocity = new Vector2(
                Mathf.Clamp(targetVelocity, -maxHorizontalSpeed, maxHorizontalSpeed),
                _rigidbody.linearVelocity.y
            );

            // Auto-jump when landing on platform
            if (_isGrounded && isTryingToJump)
            {
                PerformJump();
            }

            if (transform.position.x > maxHorizontalOffset)
            {
                float diff = transform.position.x - maxHorizontalOffset;
                float newX = -maxHorizontalOffset + diff;
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            }
            else if(transform.position.x < -maxHorizontalOffset)
            {
                float diff = transform.position.x + maxHorizontalOffset;
                float newX = maxHorizontalOffset - diff;
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            }

            float height = transform.position.y - _initialHeight;
            _gameProgress.UpdateHeight((int)height);
        }

        private void PerformJump()
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, jumpForce);
            _isGrounded = false;
        }

        private void HandlePowerUpActivation()
        {
            if (_inputHandler.IsPowerUpActivated())
            {
                _powerUpSystem.TryActivatePowerUp(this);
            }
        }

        private void CheckDeathCondition()
        {
            if (transform.position.y < _camera.ViewportToWorldPoint(Vector2.zero).y - 2f)
            {
                Die();
            }
        }

        private void Die()
        {
            if (_isDead) return;

            _isDead = true;
            _rigidbody.simulated = false;
            _eventBus.PublishPlayerDeath();
        }

        private void UpdateSpriteRotation(float horizontalMovement)
        {
            if (horizontalMovement < 0 && !_swappedScale)
            {
                _swappedScale = true;
                defaultSpriteRenderer.SetActive(false);
                swappedSpriteRenderer.SetActive(true);
            }
            else if (horizontalMovement > 0 && _swappedScale)
            {
                _swappedScale = false;
                defaultSpriteRenderer.SetActive(true);
                swappedSpriteRenderer.SetActive(false);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                // Check if landing on top of platform
                if (collision.contacts[0].normal.y > 0.5f)
                {
                    _isGrounded = true;
                    HandlePlatformInteraction(collision.gameObject.GetComponent<PlatformBase>());
                }
            }
        }

        private void HandlePlatformInteraction(PlatformBase platform)
        {
            platform.OnPlayerLanded(this);

            if (platform.ShouldJumpOnLand)
            {
                PerformJump();
            }
        }
        
        public void ApplyJumpModifier(float multiplier, float duration)
        {
            StartCoroutine(JumpModifierRoutine(multiplier, duration));
        }

        private System.Collections.IEnumerator JumpModifierRoutine(float multiplier, float duration)
        {
            float originalJumpForce = jumpForce;
            jumpForce *= multiplier;

            yield return new WaitForSeconds(duration);

            jumpForce = originalJumpForce;
        }
    }
}