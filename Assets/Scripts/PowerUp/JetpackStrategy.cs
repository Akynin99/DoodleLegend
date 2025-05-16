using DoodleLegend.Core;
using UnityEngine;

namespace DoodleLegend.PowerUp
{
    [CreateAssetMenu(fileName = "JetpackPowerUp", menuName = "DoodleLegend/Jetpack")]
    public class JetpackStrategy : ScriptableObject, IPowerUpStrategy
    {
        [Header("Settings")] [SerializeField] private float flightSpeed = 8f;
        [SerializeField] private float defaultDuration = 5f;
        [SerializeField] private Sprite icon;

        [Header("Leveling")] [SerializeField] private int maxLevel = 3;

        private PlayerController _player;
        private Coroutine _activeRoutine;

        public Sprite Icon => icon;
        public float Duration => defaultDuration;
        public bool IsStackable => false;

        public void ApplyEffect(PlayerController player)
        {
            _player = player;
            // _player.EnableFlight(flightSpeed);
            _activeRoutine = _player.StartCoroutine(EffectRoutine());
            
            PlayJetpackVFX(true);
        }

        public void CancelEffect()
        {
            if (_player != null)
            {
                // _player.DisableFlight();
                PlayJetpackVFX(false);
            }

            if (_activeRoutine != null)
            {
                _player.StopCoroutine(_activeRoutine);
            }
        }

        private System.Collections.IEnumerator EffectRoutine()
        {
            yield return new WaitForSeconds(Duration);
            CancelEffect();
        }

        private void PlayJetpackVFX(bool isActive)
        {
            ParticleSystem vfx = _player.GetComponentInChildren<ParticleSystem>();
            if (vfx != null)
            {
                if (isActive) vfx.Play();
                else vfx.Stop();
            }
        }
    }
}