namespace ClearMine.Common.ComponentModel
{
    internal interface IUpdatable<T>
        where T : class
    {
        void Update(T newValue);
    }
}
