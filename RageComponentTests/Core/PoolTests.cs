using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace RageComponent.Core.Tests
{
    [TestClass()]
    public class PoolTests
    {
        private class Foo
        {
            public string Name { get; set; }
        }

        [TestMethod]
        public void Size_ReturnsValuePassedInConstructor()
        {
            // Arrange
            int size = 25;
            Pool<Foo> pool = new Pool<Foo>(size, () =>
            {
                return new Foo();
            });

            // Asset
            Assert.AreEqual(size, pool.Size);
        }

        [TestMethod()]
        public void Get_FromPoolWithNoFreeObjects_ThrowsNoFreeObjectsInPoolException()
        {
            // Arrange
            int size = 1;
            Pool<Foo> pool = new Pool<Foo>(size, () =>
            {
                return new Foo();
            });

            // Act
            _ = pool.Get();

            // Asset
            Assert.ThrowsException<NoFreeObjectsInPoolException>(pool.Get);
        }

        [TestMethod]
        public void Get_ReturnsPoolObject()
        {
            // Arrange
            int size = 1;
            Foo result;
            Pool<Foo> pool = new Pool<Foo>(size, () =>
            {
                return new Foo();
            });

            // Act
            result = pool.Get();

            // Asset
            Assert.IsTrue(result is Foo);
        }

        [TestMethod]
        public void UsedObjects_GivesCorrectValueWithOnlyGet()
        {
            // Arrange
            int size = 10;
            int usedObjects;
            int expectedUsedObjects = 0;
            Pool<Foo> pool = new Pool<Foo>(size, () =>
            {
                return new Foo();
            });

            // Act
            for (int i = 0; i < size; i++)
            {
                _ = pool.Get();
            }
            usedObjects = pool.FreeObjects;

            // Asset
            Assert.AreEqual(expectedUsedObjects, usedObjects);
        }

        [TestMethod]
        public void UsedObjects_GivesCorrectValueWithGetAndFree()
        {
            // Arrange
            int size = 10;
            int usedObjects;
            int expectedUserObjects = 10;
            Pool<Foo> pool = new Pool<Foo>(size, () =>
            {
                return new Foo();
            });

            // Act
            List<Foo> objects = new List<Foo>();
            for (int i = 0; i < size; i++)
            {
                objects.Add(pool.Get());
            }
            objects.ForEach(x => pool.Free(x));

            usedObjects = pool.FreeObjects;

            // Asset
            Assert.AreEqual(expectedUserObjects, usedObjects);
        }

        [TestMethod]
        public void AccessingPoolAfterDispose_ThrowsObjectDisposedException()
        {
            // Arrange
            int size = 1;
            var pool = new Pool<Foo>(size, () =>
            {
                return new Foo();
            });

            // Act
            pool.Dispose();

            // Asset
            Assert.ThrowsException<ObjectDisposedException>(pool.Get);
        }

        [TestMethod]
        public void OnDisposed_CalledForEveryObject()
        {
            // Arrange
            int size = 10;
            int onDisposeCount = 0;
            int onDisposedCountExpected = size;
            var pool = new Pool<Foo>(size, () =>
            {
                return new Foo();
            });

            pool.OnDispose += _ => onDisposeCount++;

            // Act
            pool.Dispose();

            // Asset
            Assert.AreEqual(onDisposedCountExpected, onDisposeCount);
        }

        [TestMethod]
        public void IsDisposed_ReturnsTrueAfterDisposed()
        {
            // Arrange
            int size = 1;
            var pool = new Pool<Foo>(size, () =>
            {
                return new Foo();
            });

            // Act
            pool.Dispose();

            // Asset
            Assert.IsTrue(pool.IsDisposed);
        }

        [TestMethod]
        public void IsDisposed_ReturnsFalse()
        {
            // Arrange
            int size = 1;
            var pool = new Pool<Foo>(size, () =>
            {
                return new Foo();
            });

            // Asset
            Assert.IsFalse(pool.IsDisposed);
        }

        [TestMethod]
        public void Check_Not_Disposed_Pool_IsDisposed_Returns_False()
        {
            // Arrange
            var pool = new Pool<Foo>(10, () =>
            {
                return new Foo();
            });

            // Asset
            Assert.IsFalse(pool.IsDisposed);
        }

        [TestMethod]
        public void CreatePoolWithWrongSize_ThrowsArgumentException()
        {
            // Arrange
            int size = 0;

            // Act & Asset
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var pool = new Pool<Foo>(size, () =>
                {
                    return new Foo();
                });
            }, "Size can't be less than 1.");
        }
    }
}
