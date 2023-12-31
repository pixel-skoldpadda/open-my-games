using App.Scripts.Infrastructure.LevelSelection;
using App.Scripts.Libs.Installer;
using App.Scripts.Libs.ServiceLocator;
using App.Scripts.Scenes.SceneFillwords.Config;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel;
using UnityEngine;

namespace App.Scripts.Scenes.SceneFillwords.Installers
{
    public class InstallerFillwordServices : MonoInstaller
    {
        [SerializeField] private ConfigLevelSelection configLevelSelection;
        [SerializeField] private ConfigFillwordLevelData fillwordLevelData;

        public override void InstallBindings(ServiceContainer container)
        {
            container.SetService<IServiceLevelSelection, ServiceLevelSelection>(
                new ServiceLevelSelection(configLevelSelection));
            container.SetService<IProviderFillwordLevel, ProviderFillwordLevel>(
                new ProviderFillwordLevel(configLevelSelection, fillwordLevelData, container.Get<IServiceLevelSelection>()));

            container.SetServiceSelf(new ContainerGrid());
        }
    }
}