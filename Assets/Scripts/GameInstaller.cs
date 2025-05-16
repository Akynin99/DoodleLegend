using DoodleLegend.Core;
using DoodleLegend.Platform;
using DoodleLegend.PlayerInput;
using DoodleLegend.PowerUp;
using UnityEngine;
using Zenject;

namespace DoodleLegend
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PowerUpSystem powerUpSystem;
        [SerializeField] private PlatformFactory factory;
        [SerializeField] private CameraController cameraController;

        private void Awake()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Application.targetFrameRate = 60;
#endif
        }

        public override void InstallBindings()
        {
            Container.Bind<IEventBus>().To<EventBus>().AsSingle();
            
            Container.Bind<IPowerUpStrategy>().To<JetpackStrategy>().AsSingle();
            
            Container.Bind<IInputHandler>().FromMethod(_ => InputHandlerFactory.Create()).AsSingle().NonLazy();
        
            Container.Bind<PlayerController>().FromInstance(playerController).AsSingle();
            
            Container.Bind<IPowerUpSystem>().FromInstance(powerUpSystem).AsSingle();
            
            Container.Bind<IPlatformFactory>().To<PlatformFactory>().FromInstance(factory).AsSingle();
            
            Container.Bind<IGameProgress>().To<GameProgress>().AsSingle().NonLazy();
            
            Container.Bind<ICameraController>().To<CameraController>().FromInstance(cameraController).AsSingle();
        }
    }
}