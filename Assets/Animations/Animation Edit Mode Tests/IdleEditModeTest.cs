using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class IdleEditModeTest
{
    
    [Test]
    public void IdleTestChangeState()
    {
        GameObject animator = new GameObject();
        GameObject idleOne = new GameObject();
        GameObject idleTwo = new GameObject();  
        idleOne.transform.parent = animator.transform;
        idleTwo.transform.parent = animator.transform;

        var controller = animator.AddComponent<IdleAnimator>();
        controller.idle_0 = idleOne;
        controller.idle_1 = idleTwo;

        controller.idleStates = new List<GameObject>
        {
            idleOne,
            idleTwo
        };

        controller.Idle(1);

        Assert.AreEqual(false, controller.idle_0.activeSelf);
        Assert.AreEqual(true, controller.idle_1.activeSelf);

        controller.Idle(0);

        Assert.AreEqual(true, controller.idle_0.activeSelf);
        Assert.AreEqual(false, controller.idle_1.activeSelf);

    }

}
