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
    using ClearMine.Framework.Messages;
    using ClearMine.GameDefinition;
    using Microsoft.Win32;

    /// <summary>
    /// 
    /// </summary>
    [Export(typeof(IGameSerializer))]
    public class GameSerializer : IGameSerializer
    {
        public string SaveGame(IClearMine game, string path = null)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                var savePathDialog = new SaveFileDialog();
                savePathDialog.DefaultExt = Settings.Default.SavedGameExt;
                savePathDialog.Filter = LocalizationHelper.FindText("SavedGameFilter", Settings.Default.SavedGameExt);
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
                throw new FileNotFoundException(LocalizationHelper.FindText("SavedGamePathNotFound"), path);
            }

            if (gameType.GetInterface(typeof(IClearMine).FullName) == null)
            {
                throw new InvalidOperationException(LocalizationHelper.FindText("InvalidClearMineGameType", gameType.FullName));
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
                MessageBox.Show(LocalizationHelper.FindText("CorruptedSavedGameMessage"), LocalizationHelper.FindText("CorruptedSavedGameTitle"));
            }

            return newgame;
        }
    }
}
