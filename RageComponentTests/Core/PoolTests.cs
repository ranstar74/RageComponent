using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace RageComponent.Core.Tests
{
    [TestClass()]
    public class PoolTests
    {
        private class TestPoolObject
        {
            public string Name { get; set; }
        }

        [TestMethod]
        public void Check_Pool_Size_Returns_Value_That_Was_Passed_In_Constructor()
        {
            // Arrange
            var pool = new Pool<TestPoolObject>(25, () =>
            {
                return new TestPoolObject();
            });

            // Act
            // ...

            // Asset
            Assert.AreEqual(25, pool.Size);
        }

        [TestMethod()]
        public void Check_Object_Get_In_A_Pool_With_No_Free_Objects_Throws_NoFreeObjectsInPoolException()
        {
            // Arrange
            var pool = new Pool<TestPoolObject>(1, () =>
            {
                return new TestPoolObject();
            });

            // Act
            _ = pool.Get();

            // Asset
            Assert.ThrowsException<NoFreeObjectsInPoolException>(pool.Get);
        }

        [TestMethod]
        public void Check_Object_Get_In_A_Pool_Retuns_Object()
        {
            // Arrange
            var pool = new Pool<TestPoolObject>(1, () =>
            {
                return new TestPoolObject();
            });
            object result;

            // Act
            result = pool.Get();

            // Asset
            Assert.IsTrue(result is TestPoolObject);
        }

        [TestMethod]
        public void Check_Pool_UsedObjects_Without_Release()
        {
            // Arrange
            var pool = new Pool<TestPoolObject>(10, () =>
            {
                return new TestPoolObject();
            });
            int usedObjects;

            // Act
            List<TestPoolObject> objects = new List<TestPoolObject>();

            for(int i = 0; i < 10; i++)
            {
                objects.Add(pool.Get());
            }
            usedObjects = pool.FreeObjects;

            // Asset
            Assert.AreEqual(0, usedObjects);
        }

        [TestMethod]
        public void Check_Pool_UsedObjects_With_Release()
        {
            // Arrange
            var pool = new Pool<TestPoolObject>(10, () =>
            {
                return new TestPoolObject();
            });
            int freeObjects;

            // Act
            List<TestPoolObject> objects = new List<TestPoolObject>();

            for (int i = 0; i < 10; i++)
            {
                objects.Add(pool.Get());
            }
            objects.ForEach(x => pool.Free(x));

            freeObjects = pool.FreeObjects;

            // Asset
            Assert.AreEqual(10, freeObjects);
        }

        [TestMethod]
        public void Check_Accesing_Disposed_Pool_Throws_ObjectDisposedException()
        {
            // Arrange
            var pool = new Pool<TestPoolObject>(1, () =>
            {
                return new TestPoolObject();
            });

            // Act
            pool.Dispose();

            // Asset
            Assert.ThrowsException<ObjectDisposedException>(pool.Get);
        }

        [TestMethod]
        public void Check_Pool_OnDisposed_Being_Called_For_Every_Object()
        {
            // Arrange
            var pool = new Pool<TestPoolObject>(10, () =>
            {
                return new TestPoolObject();
            });
            int onDisposeCount = 0;

            pool.OnDispose += _ => onDisposeCount++;

            // Act
            pool.Dispose();

            // Asset
            Assert.AreEqual(10, onDisposeCount);
        }

        [TestMethod]
        public void Check_Pool_IsDisposed_Returns_True_After_Dispose()
        {
            // Arrange
            var pool = new Pool<TestPoolObject>(10, () =>
            {
                return new TestPoolObject();
            });

            // Act
            pool.Dispose();

            // Asset
            Assert.IsTrue(pool.IsDisposed);
        }

        [TestMethod]
        public void Check_Not_Disposed_Pool_IsDisposed_Returns_False()
        {
            // Arrange
            var pool = new Pool<TestPoolObject>(10, () =>
            {
                return new TestPoolObject();
            });

            // Act
            // ...

            // Asset
            Assert.IsFalse(pool.IsDisposed);
        }
    }
}
