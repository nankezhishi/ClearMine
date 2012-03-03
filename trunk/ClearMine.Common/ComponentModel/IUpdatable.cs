namespace ClearMine.Common.ComponentModel
{
    public interface IUpdatable<T>
    {
        void Update(T newValue);
    }
}
