using System;
using DoodleLegend.Core;
using UnityEngine;

namespace DoodleLegend.Platform
{
    public abstract class PlatformBase : MonoBehaviour 
    {
        [SerializeField] protected bool shouldJumpOnLand = true;
        [SerializeField] protected PlatformType type;

        public bool ShouldJumpOnLand => shouldJumpOnLand;
        public PlatformType Type => type;


        public event Action<PlayerController> OnPlatformInteracted;

        public enum PlatformType
        {
            Regular,
            Breakable,
            Moving,
            Trampoline,
            Spikes
        }

        public virtual void OnPlayerLanded(PlayerController player)
        {
            OnPlatformInteracted?.Invoke(player);
        }


        public virtual void Initialize(Vector2 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
        }


        public virtual void DisablePlatform()
        {
            gameObject.SetActive(false);
        }

        public event Action OnDisposed;
    }
}