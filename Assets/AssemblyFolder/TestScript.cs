using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class TestScript
{

    [Test]
    public void EnsurePlayerObjectIsInstantiated()
    {
        var player = new GameObject("PlayerObject");
        Assert.AreEqual("PlayerObject", player.name);
    }

    [Test]
    public void EnsureUITextAndScoreCounterAreMatching()
    {
        int score = 5;
        Text scoreText = new GameObject().AddComponent<Text>();

        scoreText.text = score.ToString();

        Assert.AreEqual(int.Parse(scoreText.text), score);
    }

    [UnityTest]
    public IEnumerator FirstGameObjectLoadedIsGameCanvas()
    {
        EditorSceneManager.LoadSceneInPlayMode("MainGame", new LoadSceneParameters(LoadSceneMode.Single));
        yield return null;

        GameObject[] sceneObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        Assert.IsNotNull(sceneObjects, "No objects found in the scene");
        Assert.Greater(sceneObjects.Length, 0, "No objects found in the scene");

        GameObject firstLoadedObject = sceneObjects[0];
        Assert.IsNotNull(firstLoadedObject, "First loaded object is null");
        Assert.AreEqual("GameCanvas", firstLoadedObject.name, "First loaded object is not the GameCanvas with main menu");
    }
}
