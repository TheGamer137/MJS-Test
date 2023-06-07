using System;
using Interfaces;
using Services.Interfaces;
using Views.Interfaces;

namespace Presenters
{
    public class MenuPresenter
    {
        private readonly IAudioView _audioView;
        private readonly IMenuView _menuView;
        private readonly IVideoPlayer _videoPlayer;
        private readonly IWebSocketService _webSocketService;

        public MenuPresenter(IMenuView menuView, IVideoPlayer videoPlayer, 
            IWebSocketService webSocketService,IAudioView audioView)
        {
            _menuView = menuView;
            _audioView = audioView;
            _videoPlayer = videoPlayer;
            _webSocketService = webSocketService;
            _menuView.PortText = _webSocketService.Port;
            _menuView.ServerText = _webSocketService.Url;
            _menuView.VideoText = _videoPlayer.GetVideo();
            _menuView.UpdateVolume += MenuViewOnUpdateVolume;
            _menuView.UpdateMusicVolume += MenuViewOnUpdateMusicVolume;
            _menuView.UpdateEffectsVolume += MenuViewOnUpdateEffectsVolume;
        }


        private void MenuViewOnUpdateEffectsVolume(object sender, EventArgs e)
        {
            if (_menuView.ClickSoundIsOn)
            {
                _audioView.clickSoundMuted = false;
            }
            else
            {
                _audioView.clickSoundMuted = true;
            }
        }

        private void MenuViewOnUpdateMusicVolume(object sender, EventArgs e)
        {
            if (_menuView.MusicIsOn)
            {
                _audioView.musicMuted = false;
            }
            else
            {
                _audioView.musicMuted = true;
            }
        }

        private void MenuViewOnUpdateVolume(object sender, EventArgs e)
        {
            _audioView.SetVolume(_menuView.AudioVolume);
            SetToggles(_menuView.AudioVolume);
        }

        private void SetToggles(float volume)
        {
            if (volume == 0)
            {
                _menuView.MusicIsOn = false;
                _menuView.ClickSoundIsOn = false;
            }

            if (volume > 0)
            {
                _menuView.MusicIsOn = true;
                _menuView.ClickSoundIsOn = true;
            }
        }
    }
}