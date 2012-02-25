namespace ClearMine.VM.Records
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using System.Xml.Serialization;

    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Utilities;
    using ClearMine.GameDefinition;

    public interface IGamePlayer
    {
        void Play(IClearMine game, FrameworkElement panel);
    }

    public interface IGameRecorder
    {
        void Record(IClearMine game, FrameworkElement panel);

        void SaveTo(string path);

        string SupportedFileExtension { get; }
    }

    [XmlInclude(typeof(MouseMoveRecord))]
    [XmlInclude(typeof(CellStateChanged))]
    public abstract class RecordItem : IGamePlayer
    {
        /// <summary>
        /// Gets or sets the time the current record happens since the game starts. in ms.
        /// </summary>
        [XmlAttribute("time")]
        public int Timestamp { get; set; }

        public abstract void Play(IClearMine game, FrameworkElement panel);
    }

    [Serializable]
    public class MouseMoveRecord : RecordItem
    {
        [XmlAttribute("position")]
        public Point Position { get; set; }

        public override void Play(IClearMine game, FrameworkElement panel)
        {
            if (panel == null)
                throw new ArgumentNullException("panel");

            var absolutePosition = panel.PointToScreen(Position);

            if (!NativeMethods.MoveMouseTo((int)absolutePosition.X, (int)absolutePosition.Y))
            {
                Trace.TraceError("Cannot move mouse to {0}.", absolutePosition);
            }
        }
    }

    [Serializable]
    public class CellStateChanged : RecordItem
    {
        [XmlAttribute("column")]
        public int Column { get; set; }

        [XmlAttribute("row")]
        public int Row { get; set; }

        [XmlAttribute("state")]
        public CellState NewState { get; set; }

        public override void Play(IClearMine game, FrameworkElement panel)
        {
            if (game == null)
                throw new ArgumentNullException("game");

            game.GetCell(Column, Row).State = NewState;
        }
    }

    [Serializable]
    [XmlRoot("gameSteps")]
    public class RecordCollection : List<RecordItem>, IGamePlayer
    {
        /// <summary>
        /// You need a seperated thread to call this Play. Because it will takes a long time.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="panel"></param>
        public void Play(IClearMine game, FrameworkElement panel)
        {
            if (game == null)
                throw new ArgumentNullException("game");

            foreach (var record in this.OrderBy(item => item.Timestamp))
            {
                if (game.UsedTime < record.Timestamp)
                {
                    Thread.Sleep(record.Timestamp - game.UsedTime);
                }

                record.Play(game, panel);
            }
        }
    }

    public class XmlGameRecorder : IDisposable, IGameRecorder
    {
        private bool isEnabled;
        private IClearMine game;
        private FrameworkElement panel;
        private RecordCollection records = new RecordCollection();

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (value == isEnabled)
                {
                    return;
                }

                if (value)
                {
                    panel.MouseMove -= new MouseEventHandler(OnMouseMove);
                }
                else
                {
                    panel.MouseMove += new MouseEventHandler(OnMouseMove);
                }

                isEnabled = value;
            }
        }

        public void Record(IClearMine gameToRecord, FrameworkElement panelToWatch)
        {
            this.game = gameToRecord;
            this.panel = panelToWatch;
            this.IsEnabled = true;
        }

        public void SaveTo(string path)
        {
            bool shouldStart = IsEnabled;
            IsEnabled = false;
            var gameSaver = new XmlSerializer(records.GetType());
            using (var file = File.Open(path, FileMode.Create, FileAccess.Write))
            {
                gameSaver.Serialize(file, game);
            }

            records.Clear();
            IsEnabled = shouldStart;
        }

        public string SupportedFileExtension
        {
            get { return ".cmr"; }
        }

        public void Dispose()
        {
            if (IsEnabled)
            {
                IsEnabled = false;
            }
        }

        private void OnCellStateChanged(object sender, CellStateChangedEventArgs e)
        {
            if (e.Cell.CachingState == CachingState.InUse)
            {
                records.Add(new CellStateChanged()
                {
                    Row = e.Cell.Row,
                    Column = e.Cell.Column,
                    NewState = e.Cell.State,
                    Timestamp = this.game.UsedTime,
                });
            }
            else
            {
                Trace.TraceWarning("Useless state changed event triggered from the game.");
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            records.Add(new MouseMoveRecord()
            {
                Timestamp = this.game.UsedTime,
                Position = e.MouseDevice.GetPosition(this.panel)
            });
        }
    }
}
