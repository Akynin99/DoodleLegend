using UnityEngine;

namespace DoodleLegend.Platform
{
    [System.Serializable]
    public class PlatformTypeSettings
    {
        public PlatformBase.PlatformType Type;
        public PlatformBase Prefab;
        [Range(0, 1)] public float SpawnWeight;
        public int MinHeightToSpawn;
    }
}