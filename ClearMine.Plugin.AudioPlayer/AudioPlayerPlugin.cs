namespace ClearMine.Plugin.AudioPlayer
{
    using System.Linq;

    using ClearMine.Common.Messaging;
    using ClearMine.Common.Modularity;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.GameDefinition;
    using ClearMine.GameDefinition.Messages;

    /// <summary>
    /// 
    /// </summary>
    public class AudioPlayerPlugin : IPlugin
    {
        public string Name
        {
            get { return "Music Player"; }
        }

        public string Description
        {
            get { return "Provide sound effect to some key game event."; }
        }

        public bool IsEnabled { get; set; }

        public void Initialize()
        {
            MessageManager.SubscribeMessage<UserOperationMessage>(OnUserOperation);
            MessageManager.SubscribeMessage<GameStatusMessage>(OnGameStateChanged);
        }

        private static void OnGameStateChanged(GameStatusMessage message)
        {
            var game = message.Source as IClearMine;
            if (game != null)
            {
                if (game.GameState == GameState.Failed)
                {
                    Player.Play(Settings.Default.SoundLose);
                }
                else if (game.GameState == GameState.Success)
                {
                    Player.Play(Settings.Default.SoundWin);
                }
                else if (game.GameState == GameState.Initialized)
                {
                    Player.Play(Settings.Default.SoundStart);
                }
                else
                {
                    // Play nothing.
                }
            }
        }

        private static void OnUserOperation(UserOperationMessage message)
        {
            if (message.UserOperation == GameOperation.Expand ||
                message.UserOperation == GameOperation.Dig)
            {
                int emptyCellExpanded = message.EffectedCells.Count(c => c.MinesNearby == 0);
                if (emptyCellExpanded == 0)
                {
                    // Do nothing.
                }
                else if (emptyCellExpanded > 1)
                {
                    Player.Play(Settings.Default.SoundTileMultiple);
                }
                else
                {
                    Player.Play(Settings.Default.SoundTileSingle);
                }
            }
        }
    }
}
