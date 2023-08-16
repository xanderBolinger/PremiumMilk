using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;


public class StabPlaymodeTest
{
    private GameObject player = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Character/Player.prefab");

    [UnityTest]
    public IEnumerator StabTest()
    {
        var playerInstance = Object.Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        var idleController = playerInstance.GetComponent<IdleAnimator>();
        var swingController = playerInstance.GetComponent<StabAnimator>();

        idleController.Idle(1);

        Assert.AreEqual(false, idleController.idle_0.activeSelf);
        Assert.AreEqual(true, idleController.idle_1.activeSelf);

        swingController.Stab(0);

        Assert.AreEqual(false, idleController.idle_0.activeSelf);
        Assert.AreEqual(false, idleController.idle_1.activeSelf);
        Assert.AreEqual(true, swingController.stab_0.activeSelf);

    }
}

