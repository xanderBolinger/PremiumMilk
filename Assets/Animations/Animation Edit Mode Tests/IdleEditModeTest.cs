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
        controller.states = new List<GameObject> { idleOne , idleTwo};

        controller.IdleStates = controller.states;

        controller.Idle(1);

        Assert.AreEqual(false, controller.IdleStates[0].activeSelf);
        Assert.AreEqual(true, controller.IdleStates[1].activeSelf);

        controller.Idle(0);

        Assert.AreEqual(true, controller.IdleStates[0].activeSelf);
        Assert.AreEqual(false, controller.IdleStates[1].activeSelf);

    }

}
