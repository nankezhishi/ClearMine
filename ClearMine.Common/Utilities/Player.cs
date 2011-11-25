namespace ClearMine.Common.Utilities
{
    using System;
    using System.Threading;
    using System.Windows.Media;
    using ClearMine.Common.Properties;

    /// <summary>
    /// 
    /// </summary>
    public static class Player
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="volume"></param>
        public static void Play(string location, double volume = 0.5)
        {
            if (Settings.Default.PlaySound)
            {
                // Without a Thread, The MediaPlayer will not play it in more cases.
                ThreadPool.QueueUserWorkItem(args =>
                {
                    var player = new MediaPlayer();
                    player.Volume = volume;
                    player.Open(new Uri(location, UriKind.RelativeOrAbsolute));
                    player.Play();
                });
            }
        }
    }
}
