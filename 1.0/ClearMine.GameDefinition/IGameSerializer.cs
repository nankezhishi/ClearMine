namespace ClearMine.GameDefinition
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface IGameSerializer
    {
        string SaveGame(IClearMine game, string path = null);

        IClearMine LoadGame(string path, Type gameType);
    }
}
