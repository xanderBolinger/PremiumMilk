using System.Collections;
using System.Collections.Generic;
using Mirror;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class ExamplePlayModeTest
{

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator ExamplePlayModeTestWithEnumeratorPasses()
    {
        Debug.Log("Play mode test pass!");

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        Assert.AreEqual(true, true);
        yield return null;
    }

    


    [UnityTest]
    public IEnumerator GridMoverTestScene()
    {

        /* SceneManager.LoadScene("GridMoverTestScene");

        Debug.Log("Play mode test pass!");

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return 10f;

        NetworkManager.singleton.StartClient();

        yield return 10f;

        Assert.AreEqual(true, true);

        var player = GameObject.Find("Player [connId=0]");

        Assert.AreEqual(true, player != null); */

        yield return null;
    }
}
