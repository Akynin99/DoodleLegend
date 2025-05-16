using DoodleLegend.Core;
using UnityEngine;
using Zenject;

namespace DoodleLegend
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController playerController;
        public override void InstallBindings() 
        {
            Container.Bind<IInputHandler>().FromMethod(_ => InputHandlerFactory.Create()).AsSingle().NonLazy();
        
            Container.Bind<PlayerController>().FromInstance(playerController).AsSingle();
        }
    }
}