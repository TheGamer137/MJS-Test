using System;
using Models;
using Presenters;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Views.Interfaces;
using Zenject;
using VideoPlayer = UnityEngine.Video.VideoPlayer;

namespace Views
{
    public class VideoPlayerView : MonoBehaviour, IVideoPlayerView
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private AudioSource _audioSource;
        [Inject] private VideoPlayerPresenter _presenter;

        private void Update()
        {
            _startButton.onClick.AddListener(PrepareVideo);
        }

        private void PrepareVideo()
        {
            _videoPlayer.source = VideoSource.Url;
            GetVideo?.Invoke(this, EventArgs.Empty);
            _videoPlayer.prepareCompleted+= VideoPlayerOnPrepareCompleted;
            _videoPlayer.loopPointReached+= VideoPlayerOnloopPointReached;
            _videoPlayer.Prepare();
        }

        private void VideoPlayerOnloopPointReached(VideoPlayer source)
        {
            _audioSource.UnPause();
        }

        private void VideoPlayerOnPrepareCompleted(VideoPlayer source)
        {
            _videoPlayer.Play();
            _audioSource.Pause();
        }

        public string Url
        {
            get=>_videoPlayer.url; 
            set=>_videoPlayer.url = value;
        }
        
        public event EventHandler GetVideo;
    }
}