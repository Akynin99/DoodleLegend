using UnityEngine;
using Zenject;

namespace DoodleLegend.Core
{
    public class CameraController : MonoBehaviour, ICameraController
    {
        [Header("Settings")]
        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private Vector2 verticalOffset = new Vector2(0f, 2f);
        [SerializeField] private Vector2 horizontalLimits = new Vector2(-5f, 5f);

        private Transform _playerTransform;
        private float _maxHeight;
        private Vector3 _initialPosition;
        private bool _isEnabled = true;

        [Inject]
        public void Construct(PlayerController player)
        {
            _playerTransform = player.transform;
            _initialPosition = transform.position;
            _maxHeight = _initialPosition.y;
        }

        private void LateUpdate()
        {
            if (!_isEnabled || _playerTransform == null) return;

            UpdateCameraPosition();
        }

        private void UpdateCameraPosition()
        {
            float targetY = GetTargetYPosition();
            float clampedX = transform.position.x;
            // clampedX = Mathf.Clamp(_playerTransform.position.x, horizontalLimits.x, horizontalLimits.y);

            Vector3 targetPosition = new Vector3(
                clampedX,
                targetY,
                _initialPosition.z
            );

            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                smoothSpeed * Time.deltaTime
            );
        }

        private float GetTargetYPosition()
        {
            float playerY = _playerTransform.position.y;
            if (playerY > _maxHeight - verticalOffset.y)
            {
                _maxHeight = Mathf.Max(_maxHeight, playerY + verticalOffset.y);
            }
            return _maxHeight - verticalOffset.y;
        }

        public void ResetCamera()
        {
            _maxHeight = _initialPosition.y;
            transform.position = _initialPosition;
        }

        public void SetCameraEnabled(bool enabled)
        {
            _isEnabled = enabled;
        }

        public Vector2 CurrentPosition()
        {
            return (Vector2)transform.position;
        }
    }
}