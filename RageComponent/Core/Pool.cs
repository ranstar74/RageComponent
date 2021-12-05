using System;
using System.Collections.Generic;
using System.Linq;

namespace RageComponent.Core
{
    /// <summary>
    /// The exception that is thrown when object is requested from a pool with no free objects.
    /// </summary>
    public class NoFreeObjectsInPoolException : Exception { }

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
        /// Gets a value indicating how much of pool objects are free / unused.
        /// </summary>
        public int FreeObjects => _pool.Count(x => x.Value == false);

        /// <summary>
        /// Invokes on <see cref="Dispose"/> for every pool object.
        /// </summary>
        public Action<T> OnDispose { get; set; }

        /// <summary>
        /// Gets a value indicating whether this pool is disposed or not.
        /// </summary>
        public bool IsDisposed { get; set; } = false;

        /// <summary>
        /// Dict that contains an object with bool, indicating whether object is used or not.
        /// </summary>
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
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            T key = _pool.FirstOrDefault(x => x.Value == false).Key;
            
            if (key == null)
                throw new NoFreeObjectsInPoolException();

            // Mark as used object
            _pool[key] = true;

            return key;
        }

        /// <summary>
        /// Releases a object back to pool.
        /// </summary>
        /// <param name="obj">Objec to release.</param>
        public void Free(T obj)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            // Mark as unused object
            _pool[obj] = false;
        }

        /// <summary>
        /// Disposes this <see cref="Pool{T}"/>.
        /// </summary>
        public void Dispose()
        {
            if(IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            _pool.Keys
                .ToList()
                .ForEach(x => OnDispose?.Invoke(x));

            IsDisposed = true;
        }
    }
}
