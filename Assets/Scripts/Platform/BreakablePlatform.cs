using System.Collections;
using DoodleLegend.Core;
using UnityEngine;
using Zenject;

namespace DoodleLegend.Platform
{
    public class BreakablePlatform : PlatformBase 
    {
        [SerializeField] private float destroyDelay = 0.2f;
    
        public override void OnPlayerLanded(PlayerController player) 
        {
            base.OnPlayerLanded(player);
            StartCoroutine(DestroyPlatform());
        }

        private IEnumerator DestroyPlatform() 
        {
            yield return new WaitForSeconds(destroyDelay);
            
            gameObject.SetActive(false);
        }
    }
}