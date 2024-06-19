//Time taken: 4 hours
namespace CacheComponent
{
    public class Cache<TKey, TValue>
    {
        private readonly int _cacheSize;
        private readonly Dictionary<TKey, CacheItem<TValue>> _cache;
        private readonly LinkedList<TKey> _cacheList;
        //Threadsafe _lock

        public event Action<TKey, TValue> RemoveItem;

        public Cache(int cachesize = 10) //set default to 10
        {
            if (cachesize <= 0)
                throw new ArgumentException("Cache size must be greater than 0", nameof(cachesize));
            _cacheSize = cachesize;
            _cache = new Dictionary<TKey, CacheItem<TValue>>(cachesize);
            _cacheList = new LinkedList<TKey>();
        }

        public TValue GetValue(TKey key) 
        {
            //check if key already exists in cache 
            if(_cache.TryGetValue(key, out var cacheObject)) 
            {
                //upddate last used datetime
                cacheObject.LastUsedDateTime = DateTime.UtcNow;
                //remove item then add to top if list
                _cacheList.Remove(key);
                _cacheList.AddFirst(key);
                return cacheObject.Value;
            }

            return default;
        }

        public void SetValue(TKey key, TValue value) 
        {
            //check if key already exists in cache 
            if (_cache.ContainsKey(key))
            {
                //update, remove then add
                _cache[key].Value = value;
                _cache[key].LastUsedDateTime = DateTime.UtcNow;
                _cacheList.Remove(key);
                _cacheList.AddFirst(key);
            }
            else
            {
                //add new. Check if cachesize is full
                if(_cache.Count >= _cacheSize)
                {
                    //full. remove last used before adding new
                    var lastUsedKey = _cacheList.Last.Value;
                    var valueToRemove = _cache[lastUsedKey].Value;
                    _cache.Remove(lastUsedKey);
                    _cacheList.RemoveLast();
                    //call event
                    RemoveItem.Invoke(lastUsedKey, valueToRemove);
                }

                //not full, not already in cache, add as normal
                var cacheItem = new CacheItem<TValue>(value);
                //_cache.Add(key, cacheItem); 
                _cache[key] = cacheItem;
                _cacheList.AddFirst(key);
            }
        }
    }

}
