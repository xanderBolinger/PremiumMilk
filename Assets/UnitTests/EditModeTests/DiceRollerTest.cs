using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DiceRollerTest
{
    
    [Test]
    public void RollTest()
    {
        Assert.AreEqual(1, DiceRoller.Roll(1, 1));
        DiceRoller.SetNextTestValue(0);
        Assert.AreEqual(0, DiceRoller.Roll(1,1));
        Assert.AreEqual(10, DiceRoller.Roll(10, 10));

        
    }

    [Test]
    public void MultiNextValueTest() {
        DiceRoller.AddNextTestValue(1);
        DiceRoller.AddNextTestValue(2);
        Assert.AreEqual(1, DiceRoller.Roll(0, 0));
        Assert.AreEqual(2, DiceRoller.Roll(0, 0));
    }

    [Test]
    public void DicePoolSuccessTest() {

        DiceRoller.AddNextTestValue(1);
        DiceRoller.AddNextTestValue(2);
        DiceRoller.AddNextTestValue(6);
        DiceRoller.AddNextTestValue(6);

        Assert.AreEqual(2, DiceRoller.GetSuccess(4, 6));
        
    }


}
