using System;
using System.Collections.Generic;
using DoodleLegend.Core;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace DoodleLegend.Platform
{
    public class PlatformFactory : MonoBehaviour, IPlatformFactory
    {
        [SerializeField] private Transform platformContainer;
        [SerializeField] private PlatformConfig platformConfig;
        [SerializeField] private int poolSize;

        [Inject] private DiContainer _diContainer;
        [Inject] private IGameProgress _gameProgress;

        private Dictionary<PlatformBase.PlatformType, ObjectPool<PlatformBase>> _pools = new();
        private WeightedRandomSelector<PlatformBase.PlatformType> _typeSelector = new();
        private List<PlatformTypeSettings> _platformSettings;

        private void Awake()
        {
            _platformSettings = platformConfig.Settings;
            
            InitializePools();
            BuildTypeSelector();
        }

        private void Start()
        {
            PrewarmPools();
        }

        private void InitializePools()
        {
            foreach (var setting in _platformSettings)
            {
                _pools[setting.Type] = new ObjectPool<PlatformBase>(
                    createFunc: () => CreateNewPlatform(setting.Prefab),
                    actionOnGet: platform => platform.gameObject.SetActive(true),
                    actionOnRelease: platform => platform.gameObject.SetActive(false),
                    actionOnDestroy: Destroy,
                    defaultCapacity: poolSize
                );
            }
        }
        
        private void PrewarmPools()
        {
            // foreach (var pool in _pools.Values)
            // {
            //     pool.Prewarm(_prewarmCount);
            // }
        }

        private void BuildTypeSelector()
        {
            foreach (var setting in _platformSettings)
            {
                if (_gameProgress.CurrentHeight >= setting.MinHeightToSpawn)
                {
                    _typeSelector.Add(setting.Type, setting.SpawnWeight);
                }
            }
        }

        public PlatformBase CreatePlatform(Vector2 position)
        {
            var selectedType = _typeSelector.GetRandom();
            var platform = _pools[selectedType].Get();

            platform.Initialize(position);
            platform.OnDisposed += () => ReturnPlatform(platform);

            UpdateAvailableTypes((int)position.y);
            return platform;
        }

        private PlatformBase CreateNewPlatform(PlatformBase prefab)
        {
            var instance = _diContainer.InstantiatePrefabForComponent<PlatformBase>(
                prefab,
                Vector3.zero,
                Quaternion.identity,
                platformContainer
            );

            _diContainer.Inject(instance);
            return instance;
        }

        public void ReturnPlatform(PlatformBase platform)
        {
            var type = platform.Type;
            if (_pools.ContainsKey(type))
            {
                _pools[type].Release(platform);
            }
            else
            {
                Destroy(platform.gameObject);
            }
        }
        
        public void UpdateAvailableTypes(int currentHeight)
        {
            _typeSelector.Clear();
            foreach (var setting in _platformSettings)
            {
                if (currentHeight >= setting.MinHeightToSpawn)
                {
                    _typeSelector.Add(setting.Type, setting.SpawnWeight);
                }
            }
        }
    }
}