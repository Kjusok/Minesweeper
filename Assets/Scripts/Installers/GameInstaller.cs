using Configs;
using Core.ResourceSystem;
using Core.UI;
using Game.Grid;
using Game.Input;
using Game.Timer;
using UnityEngine;
using Zenject;
using ResourceProvider = Core.ResourceSystem.ResourceProvider;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private UiWindowsHandler _uiWindowsHandler;
        [SerializeField] private CellView _cellPrefab;

        public override void InstallBindings()
        {
            Container.BindInstance(_gridConfig).AsSingle();

            Container.Bind<IResourceProvider>().To<ResourceProvider>().AsSingle();

            Container.Bind<UiWindowsHandler>()
                .FromComponentInNewPrefab(_uiWindowsHandler)
                .AsSingle()
                .NonLazy();

            Container.Bind<GridService>().AsSingle();
            Container.Bind<MineGenerator>().AsSingle();
            Container.Bind<GameTimerService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameInputHandler>().AsSingle();

            Container.BindFactory<CellView, CellView.Factory>()
                .FromComponentInNewPrefab(_cellPrefab);
        }
    }
}
