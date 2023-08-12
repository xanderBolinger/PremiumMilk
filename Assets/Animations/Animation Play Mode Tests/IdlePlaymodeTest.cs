using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class IdlePlaymodeTest
{

    private GameObject player = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Character/Player.prefab");


    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator IdleAnimation()
    {
        var playerInstance = Object.Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
        
        yield return null;

        var controller = playerInstance.GetComponent<CharacterAnimator>();

        yield return new WaitForSeconds(0.5f);

        Assert.AreEqual(false, controller.idle_0.activeSelf);
        Assert.AreEqual(true, controller.idle_1.activeSelf);

        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(0.5f);

        Assert.AreEqual(true, controller.idle_0.activeSelf);
        Assert.AreEqual(false, controller.idle_1.activeSelf);


    }
}
