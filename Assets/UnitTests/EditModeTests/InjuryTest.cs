using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Character;

public class InjuryTest {
    // A Test behaves as an ordinary method
    [Test]
    public void InjuryTesting() {
        Injury i = new Injury(1, 11, 15, 90, "metal-sword", "James Smith","location", 1);
        Assert.AreEqual(1, i.pd);
        Assert.AreEqual(11, i.bloodlessPD);
        Assert.AreEqual(15, i.pain);
        Assert.AreEqual(90, i.shock);
        Assert.AreEqual("location", i.anatomicalLocation);
        Assert.AreEqual(1, i.level);
    }
}
