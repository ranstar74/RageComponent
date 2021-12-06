using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace RageComponent.Core.Tests
{
    [TestClass()]
    public class ComponentObjectCollectionTests
    {
        public class FooComponents : ComponentCollection
        {
            public class BarComponent : Component
            {
                public int FooCounter { get; set; }

                public BarComponent(ComponentCollection components) : base(components)
                {

                }

                public override void Update()
                {
                    FooCounter++;
                }
            }

            public BarComponent Bar { get; }

            public FooComponents(Foo foo) : base(foo)
            {
                Bar = Create<BarComponent>();

                OnStart();
            }
        }

        public class Foo : IComponentObject
        {
            public int Handle => _handle;

            public FooComponents Components => (FooComponents) GetComponents();

            private readonly FooComponents _components;
            private readonly int _handle;

            public Foo(int handle = 1)
            {
                _components = new FooComponents(this);
                _handle = handle;
            }

            public ComponentCollection GetComponents()
            {
                return _components;
            }

            public void Dispose()
            {

            }

            public static implicit operator int (Foo componentObject) => componentObject.Handle;
        }

        [TestMethod()]
        public void Add_TwoComponentsWithTheSameHandle_ThrowsArgumentException()
        {
            // Arrange
            Foo foo = new Foo();
            ComponentObjectCollection fooCollection = new ComponentObjectCollection();

            // Act
            fooCollection.Add(foo);

            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                fooCollection.Add(foo);
            }, "An object with the same handle is already in the collection.");
        }

        [TestMethod()]
        public void Remove_ComponentObject_RemovesObject()
        {
            // Arrange
            Foo foo = new Foo();
            ComponentObjectCollection fooCollection = new ComponentObjectCollection();

            // Act
            fooCollection.Add(foo);
            fooCollection.Remove(foo);

            // Assert
            Assert.IsFalse(fooCollection.Contains(foo));
        }

        [TestMethod()]
        public void GetByHandle_ReturnsRightObject()
        {
            // Arrange
            Foo foo = new Foo();
            Foo foo2;
            ComponentObjectCollection fooCollection = new ComponentObjectCollection();

            // Act
            fooCollection.Add(foo);
            foo2 = fooCollection.GetByHandle<Foo>(foo);

            // Assert
            Assert.AreEqual(foo, foo2);
        }

        [TestMethod]
        public void GetByHandle_NotExistingObject_ThrowsArgumentException()
        {
            // Arrange
            int handle = -1;
            ComponentObjectCollection fooCollection = new ComponentObjectCollection();

            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                _ = fooCollection.GetByHandle<Foo>(handle);
            }, $"Component Object with given handle: {handle} was not found.");
        }

        [TestMethod]
        public void Enumerator_EnumeratingThroughCollection()
        {
            // Arrange
            List<Foo> enumeratedFoo = new List<Foo>();
            List<Foo> expectedFoo = new List<Foo>();
            ComponentObjectCollection fooCollection = new ComponentObjectCollection();

            for (int i = 0; i < 10; i++)
            {
                expectedFoo.Add(fooCollection.Add(new Foo(i)));
            }

            // Act
            foreach (Foo foo in fooCollection)
            {
                enumeratedFoo.Add(foo);
            }

            // Assert
            CollectionAssert.AreEqual(expectedFoo, enumeratedFoo);
        }
    }
}
