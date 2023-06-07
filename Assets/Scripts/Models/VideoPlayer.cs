using Interfaces;
using Services.Interfaces;

namespace Models
{
    public class VideoPlayer : IVideoPlayer
    {
        private const string Url = "192.168.0.105";
        public string GetVideo() => Url;
    }
}