using System;
using Interfaces;
using Services.Interfaces;
using Views.Interfaces;

namespace Presenters
{
    public class VideoPlayerPresenter
    {
        private readonly IVideoPlayer _model;
        private readonly IVideoPlayerView _view;
        private readonly IAudioView _audioView;
        
        public VideoPlayerPresenter(IVideoPlayer model, IVideoPlayerView view)
        {
            _model = model;
            _view = view;
            _view.GetVideo+= OnClickGetVideo;
        }

        private void OnClickGetVideo(object sender, EventArgs e)
        {
            _view.Url = _model.GetVideo();
        }
    }
}