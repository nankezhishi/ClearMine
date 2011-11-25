namespace ClearMine.Plugin.AudioPlayer
{
    using System;
    using System.IO;
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
    public class AudioPlayerPlugin : AbstractPlugin
    {
        public AudioPlayerPlugin()
        {
            options.Add(new PluginOption()
            {
                ID = "Volumn",
                Name = "Volumn",
                Description = "Set the volumn of the sound",
                ValueType = typeof(double),
                Value = 0.5,
                ValueValidator = value =>
                {
                    var v = (double)value;
                    return v >= 0.0 && v <= 1.0;
                },
            });
            options.Add(new PluginOption()
            {
                ID = "Won",
                Name = "Won Music",
                Description = "The music need to play when won.",
                ValueType = typeof(string),
                Value = null,
                ValueValidator = value => File.Exists(Convert.ToString(value)),
            });
            options.Add(new PluginOption()
            {
                ID = "Lost",
                Name = "Lost Music",
                Description = "The music need to play when lost.",
                ValueType = typeof(string),
                Value = null,
                ValueValidator = value => File.Exists(Convert.ToString(value)),
            });
            options.Add(new PluginOption()
            {
                ID = "New",
                Name = "New Game Music",
                Description = "The music need to play when start a new game.",
                ValueType = typeof(string),
                Value = null,
                ValueValidator = value => File.Exists(Convert.ToString(value)),
            });
        }

        public override string Name
        {
            get { return "Music Player"; }
        }

        public override string Description
        {
            get { return "Provide sound effect to some key game event."; }
        }

        public override void Initialize()
        {
            MessageManager.SubscribeMessage<GameStateMessage>(OnGameStateChanged);
            MessageManager.SubscribeMessage<UserOperationMessage>(OnUserOperation);
        }

        private void OnGameStateChanged(GameStateMessage message)
        {
            var game = message.Source as IClearMine;
            if (IsEnabled && game != null)
            {
                if (game.GameState == GameState.Failed)
                {
                    Player.Play(this["Lost"].Value as string ?? Settings.Default.SoundLose);
                }
                else if (game.GameState == GameState.Success)
                {
                    Player.Play(this["Won"].Value as string ?? Settings.Default.SoundWin);
                }
                else if (game.GameState == GameState.Initialized)
                {
                    Player.Play(this["New"].Value as string ?? Settings.Default.SoundStart);
                }
                else
                {
                    // Play nothing.
                }
            }
        }

        private void OnUserOperation(UserOperationMessage message)
        {
            if (IsEnabled && (message.UserOperation == GameOperation.Expand ||
                message.UserOperation == GameOperation.Dig))
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
