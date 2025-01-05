using TOM.Common.Utils;

using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SingletonTests : MonoBehaviour
{
    //Singleton should not be initialized
    [UnityTest]
    public IEnumerator Singleton_IsAbsent()
    {
        // Arrange
        
        // Act
        yield return null;

        // Assert
        Assert.IsNull(DummySingleton.Instance);
        LogAssert.Expect(LogType.Error, "Singleton of DummySingleton is absent!");
    }

    // Singleton should be initialized
    [UnityTest]
    public IEnumerator Singleton_Exists()
    {
        // Arrange
        var singleton = new GameObject().AddComponent<DummySingleton>();
        
        // Act
        yield return null;

        // Assert
        Assert.IsTrue(DummySingleton.Instance);
    }

    // Singleton should be the same instance between
    // .Instance and the direct handle
    [UnityTest]
    public IEnumerator Singleton_IsSameInstance()
    {
        // Arrange
        var singleton = new GameObject().AddComponent<DummySingleton>();
        
        // Act
        yield return null;

        // Assert
        Assert.AreEqual(singleton.GetInstanceID(), DummySingleton.Instance.GetInstanceID());
    }

    // 1st instance should be kept, 2nd instance should be destroyed
    [UnityTest]
    public IEnumerator Singleton_MultipleInstanceManagement()
    {
        // Arrange
        var singleton1 = new GameObject().AddComponent<DummySingleton>();
        var singleton2 = new GameObject().AddComponent<DummySingleton>();

        // Act
        yield return null;

        // Assert
        LogAssert.Expect(LogType.Warning, "Detected >1 instance of DummySingleton Singletons");
        Assert.IsTrue(singleton2 == null);
    }
}
