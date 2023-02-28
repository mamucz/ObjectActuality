namespace ObjectActuality;

public interface IActuality<in T> where T : notnull
{
    /// <summary>
    /// Maximal time of all items
    /// </summary>
    long Time { get; }
    
    /// <summary>
    /// Adds a key with specified time.
    /// If time is greater than previous, time is updated.
    /// </summary>
    /// <returns>True if key was added or Time updated.</returns>
    bool TryAddOrUpdate(T key, long time);
    
    /// <summary>
    /// Removes all keys with time lower than or equal to time.
    /// </summary>
    /// <returns>True if no key left.</returns>
    bool PurgeItemsActualEnough(long time);
}