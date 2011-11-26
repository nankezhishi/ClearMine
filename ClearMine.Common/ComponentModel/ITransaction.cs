namespace ClearMine.Common.ComponentModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITransaction
    {
        void Commit();

        void Rollback();
    }
}
