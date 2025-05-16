using System.Collections.Generic;
using DoodleLegend.Platform;
using UnityEngine;
using Zenject;

namespace DoodleLegend.Core
{
    public class LevelGenerator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float spawnThreshold = 5f;
    [SerializeField] private float despawnOffset = 3f; 
    [SerializeField] private int initialPlatforms = 20;
    [SerializeField] private float minVerticalDistance;
    [SerializeField] private float maxVerticalDistance;
    [SerializeField] private float initialSpawnHeight;
    [SerializeField] private float minHorizontalDistance = 1.5f;
    [SerializeField] private float maxHorizontalDistance = 2f;
    [SerializeField] private float maxHorizontalX = 3f;

    [Inject] private IPlatformFactory _platformFactory;
    [Inject] private IGameProgress _gameProgress;
    [Inject] private ICameraController _cameraController;

    private float _lastSpawnHeight;
    private List<PlatformBase> _activePlatforms = new();

    private void Start()
    {
        GenerateInitialPlatforms();
    }

    private void Update()
    {
        UpdateGeneration();
        UpdateDespawn();
    }

    private void GenerateInitialPlatforms()
    {
        float currentY = initialSpawnHeight;
        
        for (int i = 0; i < initialPlatforms; i++)
        {
            currentY += Random.Range(minVerticalDistance, maxVerticalDistance);
            SpawnPlatform(currentY);
        }
        _lastSpawnHeight = currentY;
    }

    private void UpdateGeneration()
    {
        float cameraTopY = _cameraController.CurrentPosition().y + spawnThreshold;
        
        if (cameraTopY > _lastSpawnHeight)
        {
            float targetHeight = _lastSpawnHeight + Random.Range(minVerticalDistance, maxVerticalDistance);
            SpawnPlatform(targetHeight);
            _lastSpawnHeight = targetHeight;
        }
    }

    private void SpawnPlatform(float yPosition)
    {
        float xPos = GetNewPlatformX();
        var platform = _platformFactory.CreatePlatform(new Vector2(xPos, yPosition));
        platform.gameObject.SetActive(true);
        _activePlatforms.Add(platform);
    }

    private float GetNewPlatformX()
    {
        float lastX = _activePlatforms.Count > 0 
            ? _activePlatforms[^1].transform.position.x 
            : 0f;
        
        float random = Random.Range(minHorizontalDistance, maxHorizontalDistance) * (Random.value > 0.5f ? 1 : -1);

        if (lastX + random > maxHorizontalX || lastX + random < -maxHorizontalX) 
            return lastX - random;

        return lastX + random;
    }

    private void UpdateDespawn()
    {
        float despawnY = _cameraController.CurrentPosition().y - despawnOffset;
        
        for (int i = _activePlatforms.Count - 1; i >= 0; i--)
        {
            if (_activePlatforms[i].transform.position.y < despawnY)
            {
                _platformFactory.ReturnPlatform(_activePlatforms[i]);
                _activePlatforms.RemoveAt(i);
            }
        }
    }
}
}