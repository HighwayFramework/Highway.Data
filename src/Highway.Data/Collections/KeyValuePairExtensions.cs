using System;
using System.Collections.Generic;

namespace Highway.Data.Collections
{
    //https://github.com/Athari/Alba.Framework/blob/master/Alba.Framework/Collections/Collections/BiDictionary(TFirst%2CTSecond).cs
    public static class KeyValuePairExtensions
    {
        public static KeyValuePair<TValue, TKey> Reverse<TKey, TValue>(this KeyValuePair<TKey, TValue> @this)
        {
            return new KeyValuePair<TValue, TKey>(@this.Value, @this.Key);
        }
    }
}