﻿using System.Collections.Generic;

namespace Kli.Infrastructure
{
    public class Cache: ICache
    {
        /// <summary>
        /// Dados do cache.
        /// </summary>
        private readonly IDictionary<string, object> _cache = new Dictionary<string, object>();

        /// <summary>
        /// Grava um valor no cache.
        /// </summary>
        /// <param name="key">Identificador.</param>
        /// <param name="value">Valor.</param>
        /// <typeparam name="T">Tipo de valor.</typeparam>
        /// <returns>O mesmo valor é retornado.</returns>
        public T Save<T>(string key, T value)
        {
            if (value == null)
            {
                if (_cache.ContainsKey(key))
                {
                    _cache.Remove(key);
                }
            }
            else
            {
                _cache[key] = value;
            }
            return value;
        }

        /// <summary>
        /// LÊ um valor do cache.
        /// </summary>
        /// <param name="key">Identificador.</param>
        /// <typeparam name="T">Tipo de valor.</typeparam>
        /// <returns>Valor.</returns>
        public T Read<T>(string key)
        {
            if (_cache.ContainsKey(key)) return (T) _cache[key];
            return default !;
        }
    }
}