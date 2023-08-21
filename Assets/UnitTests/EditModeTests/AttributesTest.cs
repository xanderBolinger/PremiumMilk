using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

public class AttributesTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void AttributeTest()
    {
        Attributes attributes = new Attributes(10,11,12,13,14);


        Assert.AreEqual(10, attributes.str);
        Assert.AreEqual(11, attributes.hlt);
        Assert.AreEqual(12, attributes.agl);
        Assert.AreEqual(13, attributes.per);
        Assert.AreEqual(14, attributes.wil);
    }
}
