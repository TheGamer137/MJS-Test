using Interfaces;
using Models;
using Presenters;
using Services;
using Services.Interfaces;
using Views;
using Views.Interfaces;
using Zenject;

namespace Installers
{
    public class VideoPlayerInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            
            Container.Bind<IVideoPlayer>().To<VideoPlayer>().AsSingle();
            Container.Bind<IVideoPlayerView>().To<VideoPlayerView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<VideoPlayerPresenter>().AsSingle();
        }
    }
}