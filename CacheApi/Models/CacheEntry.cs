namespace CacheApi.Models
{
    public class CacheEntry<TValue>
    {
        public int Key { get; }
        public TValue Value { get; }
        public CacheEntry<TValue>? Prev { get; set; }
        public CacheEntry<TValue>? Next { get; set; }
        public bool IsDirty { get; set; }

        public CacheEntry(int key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
