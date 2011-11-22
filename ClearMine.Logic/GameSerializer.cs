namespace ClearMine.Logic
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Windows;
    using System.Xml.Serialization;

    using ClearMine.Common.Messaging;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.GameDefinition;
    using ClearMine.GameDefinition.Messages;
    using Microsoft.Win32;

    /// <summary>
    /// 
    /// </summary>
    [Export(typeof(IGameSerializer))]
    public class GameSerializer : IGameSerializer
    {
        public string SaveGame(IClearMine game, string path = null)
        {
            if (game == null)
                throw new ArgumentNullException("game");

            if (String.IsNullOrWhiteSpace(path))
            {
                var savePathDialog = new SaveFileDialog();
                savePathDialog.DefaultExt = Settings.Default.SavedGameExt;
                savePathDialog.Filter = ResourceHelper.FindText("SavedGameFilter", Settings.Default.SavedGameExt);
                if (savePathDialog.ShowDialog() == true)
                {
                    path = savePathDialog.FileName;
                }
                else
                {
                    return null;
                }
            }

            // Pause game to make sure the timestamp correct.
            game.PauseGame();
            var gameSaver = new XmlSerializer(game.GetType());
            using (var file = File.Open(path, FileMode.Create, FileAccess.Write))
            {
                gameSaver.Serialize(file, game);
            }
            game.ResumeGame();

            return path;
        }

        public IClearMine LoadGame(string path, Type gameType)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(ResourceHelper.FindText("SavedGamePathNotFound"), path);
            }

            if (gameType == null || gameType.GetInterface(typeof(IClearMine).FullName) == null)
            {
                throw new InvalidOperationException(ResourceHelper.FindText("InvalidClearMineGameType", gameType.FullName));
            }

            IClearMine newgame = null;
            var gameLoader = new XmlSerializer(gameType);
            using (var file = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                newgame = gameLoader.Deserialize(file) as IClearMine;
            }

            if (newgame.CheckHash())
            {
                MessageManager.SendMessage<GameLoadMessage>(newgame);
                newgame.ResumeGame();
            }
            else
            {
                MessageBox.Show(ResourceHelper.FindText("CorruptedSavedGameMessage"), ResourceHelper.FindText("CorruptedSavedGameTitle"));
            }

            return newgame;
        }
    }
}
