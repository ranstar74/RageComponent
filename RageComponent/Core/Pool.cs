using System;
using System.Collections.Generic;
using System.Linq;

namespace RageComponent.Core
{
    /// <summary>
    /// A pool implementation.
    /// </summary>
    /// <typeparam name="T">Type of the pool object.</typeparam>
    public class Pool<T> : IDisposable
    {
        /// <summary>
        /// Size of the pool.
        /// </summary>
        public int Size => _size;

        /// <summary>
        /// Invokes on <see cref="Dispose"/> for every pool object.
        /// </summary>
        public Action<T> OnDispose { get; set; }

        private readonly Dictionary<T, bool> _pool = new Dictionary<T, bool>();
        private readonly int _size;

        /// <summary>
        /// Creates a new <see cref="Pool{T}"/> instance.
        /// </summary>
        /// <param name="size">Size of the pull. Must be one or greater.</param>
        /// <param name="fill">A delegative that will be called to fill pool with objects.</param>
        /// <exception cref="ArgumentException"></exception>
        public Pool(int size, Func<T> fill)
        {
            if (size < 1)
                throw new ArgumentException("Size can't be less than 1.");

            _size = size;

            for(int i = 0; i < size; i++)
            {
                _pool.Add(fill(), false);
            }
        }

        /// <summary>
        /// Tries to get an object from pool.
        /// </summary>
        /// <returns>An pool object if there's any free.</returns>
        public T Get()
        {
            return _pool.FirstOrDefault(x => x.Value == false).Key;
        }

        /// <summary>
        /// Releases a object back to pool.
        /// </summary>
        /// <param name="obj">Objec to release.</param>
        public void Free(T obj)
        {
            _pool[obj] = false;
        }

        /// <summary>
        /// Disposes this <see cref="Pool{T}"/>.
        /// </summary>
        public void Dispose()
        {
            _pool.Keys.ToList().ForEach(OnDispose);
        }
    }
}
