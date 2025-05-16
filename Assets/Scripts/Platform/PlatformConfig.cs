using System.Collections.Generic;
using UnityEngine;

namespace DoodleLegend.Platform
{
    [CreateAssetMenu(fileName = "PlatformConfig", menuName = "DoodleLegend/PlatformConfig")]
    public class PlatformConfig : ScriptableObject
    {
        [SerializeField] private List<PlatformTypeSettings> settings;

        public List<PlatformTypeSettings> Settings => settings;
    }
}