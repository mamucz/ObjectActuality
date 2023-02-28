using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectActuality
{
    public sealed class Actuality<T> : IActuality<T> where T : notnull
    {
        private long _time;
        public long Time => _time;

        class MyTime
        { 
            public long Time;
        }

        ConcurrentDictionary<T, MyTime> datas = new();

        public bool PurgeItemsActualEnough(long time)
        {
            bool inNotEpty = !datas.IsEmpty;
            foreach (var dat in datas)
            {
                var res = GreaterOrEqual(ref dat.Value.Time, time);
                if (res <= time)
                    datas.TryRemove(dat.Key, out _);
            }
            if (datas.IsEmpty && inNotEpty)
            {
                _time = 0;                
                return true;
            }
            return false;
        }

        public bool TryAddOrUpdate(T key, long time)
        {
            if (datas.TryGetValue(key, out var result))
            {
                GreaterOrOld(ref result.Time, time);
            }
            else
            {
                result= new MyTime();
                if (!datas.TryAdd(key, result))
                {
                    datas.TryGetValue(key, out result);                    
                }
                GreaterOrOld(ref result!.Time, time);
            }
            var restime = GreaterOrOld(ref _time, time);
            return (restime < time);
            
        }
        private static long GreaterOrOld(ref long oldVal, long newVal)
        {
            long currentValue = oldVal;
            while (true)
            { 
                long newValuSafe = newVal>currentValue? newVal : currentValue;
                long old = Interlocked.CompareExchange(ref oldVal, newValuSafe, currentValue);
                if (old == currentValue)
                    return old;
                else
                    currentValue = old;
            }

        }

        private static long GreaterOrEqual(ref long oldVal, long newVal)
        {
            long currentValue = oldVal;
            while (true)
            {
                long newValuSafe = newVal >= currentValue ? newVal : currentValue;
                long old = Interlocked.CompareExchange(ref oldVal, newValuSafe, currentValue);
                if (old == currentValue)
                    return old;
                else
                    currentValue = old;
            }

        }
    }
}
