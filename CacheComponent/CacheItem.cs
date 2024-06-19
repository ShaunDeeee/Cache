namespace CacheComponent
{
    public class CacheItem<T>
    {
        public DateTime LastUsedDateTime { get; set; }

        public T Value { get; set; }

        public CacheItem(T value)
        {
            Value = value;
            LastUsedDateTime = DateTime.UtcNow;
        }
    }

}
