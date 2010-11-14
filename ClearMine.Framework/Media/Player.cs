namespace ClearMine.Framework.Media
{
    using System;
    using System.Threading;
    using System.Windows.Media;
    using ClearMine.Common.Properties;

    /// <summary>
    /// 
    /// </summary>
    internal class Player
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        public static void Play(string location)
        {
            if (Settings.Default.PlaySound)
            {
                // Without a Thread, The MediaPlayer will not play it in more cases.
                ThreadPool.QueueUserWorkItem(args =>
                {
                    var player = new MediaPlayer();
                    player.Open(new Uri(location, UriKind.RelativeOrAbsolute));
                    player.Play();
                });
            }
        }
    }
}
