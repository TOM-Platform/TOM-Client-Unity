using TOM.Apps;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HomeControllerTests
{

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator LoadRunningScene_Loads_RunningScene()
    {
        // Arrange
        var homeController = new GameObject().AddComponent<HomeController>();

        // Act
        homeController.LoadRunningScene();
        yield return null;

        // Assert
        Assert.AreEqual("Running", SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator LoadLearningScene_Loads_LearningScene()
    {
        // Arrange
        var homeController = new GameObject().AddComponent<HomeController>();

        // Act
        homeController.LoadLearningScene();
        yield return null;

        // Assert
        Assert.AreEqual("Learning", SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator LoadHomeScene_Loads_HomeScene()
    {
        // Arrange
        var homeController = new GameObject().AddComponent<HomeController>();

        // Act
        homeController.LoadHomeScene();
        yield return null;

        // Assert
        Assert.AreEqual("Home", SceneManager.GetActiveScene().name);
    }

    //[UnityTest]
    //public IEnumerator LoadNewScene_Unloads_PreviousScene_And_Loads_NewScene()
    //{
    //    // Arrange
    //    var homeController = new GameObject().AddComponent<HomeController>();
    //    SceneManager.LoadScene("Learning", LoadSceneMode.Single);
    //    yield return null;

    //    // Act
    //    homeController.LoadHomeScene();
    //    yield return new WaitForSeconds(1f); // Waiting for scene to load

    //    // Assert
    //    Assert.IsFalse(SceneManager.GetSceneByName("Learning").isLoaded); // Previous scene is unloaded
    //    Assert.IsTrue(SceneManager.GetSceneByName("Home").isLoaded); // New scene is loaded
    //}
}
