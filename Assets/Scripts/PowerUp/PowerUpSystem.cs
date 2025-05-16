using System.Collections.Generic;
using DoodleLegend.Core;
using UnityEngine;
using Zenject;

namespace DoodleLegend.PowerUp
{
    public class PowerUpSystem : MonoBehaviour, IPowerUpSystem
{
    [SerializeField] private bool allowMultiplePowerUps = false;
    
    [Inject] private IEventBus _eventBus;
    [Inject] private DiContainer _diContainer;

    private List<IPowerUpStrategy> _availablePowerUps = new();
    private IPowerUpStrategy _activePowerUp;
    private Coroutine _activePowerUpRoutine;

    public IPowerUpStrategy ActivePowerUp => _activePowerUp;
    public IReadOnlyList<IPowerUpStrategy> AvailablePowerUps => _availablePowerUps.AsReadOnly();

    private void OnDestroy()
    {
        CancelActivePowerUp();
    }

    public void RegisterPowerUp(IPowerUpStrategy strategy)
    {
        if (!_availablePowerUps.Contains(strategy))
        {
            // Инжектим зависимости в стратегию через Zenject
            _diContainer.Inject(strategy);
            _availablePowerUps.Add(strategy);
        }
    }

    public void TryActivatePowerUp(PlayerController player)
    {
        if (player == null || _availablePowerUps.Count == 0) return;
        
        // Выбор случайного бонуса с учетом весов
        var selectedStrategy = SelectWeightedPowerUp();
        
        if (!allowMultiplePowerUps && _activePowerUp != null)
        {
            if (!_activePowerUp.IsStackable)
            {
                CancelActivePowerUp();
            }
            else if (_activePowerUp.GetType() == selectedStrategy.GetType())
            {
                return;
            }
        }

        ApplyNewPowerUp(player, selectedStrategy);
    }

    private IPowerUpStrategy SelectWeightedPowerUp()
    {
        // Простой выбор случайного бонуса
        // Можно расширить систему весов через ScriptableObject
        return _availablePowerUps[Random.Range(0, _availablePowerUps.Count)];
    }

    private void ApplyNewPowerUp(PlayerController player, IPowerUpStrategy strategy)
    {
        _activePowerUp = strategy;
        _activePowerUp.ApplyEffect(player);
        
        _eventBus.PublishPowerUpCollected(strategy);

        if (strategy.Duration > 0)
        {
            _activePowerUpRoutine = StartCoroutine(
                AutoCancelPowerUpRoutine(strategy.Duration)
            );
        }
    }

    private System.Collections.IEnumerator AutoCancelPowerUpRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        CancelActivePowerUp();
    }

    public void CancelActivePowerUp()
    {
        if (_activePowerUp == null) return;

        _activePowerUp.CancelEffect();
        
        if (_activePowerUpRoutine != null)
        {
            StopCoroutine(_activePowerUpRoutine);
        }
        
        _activePowerUp = null;
    }
}
}