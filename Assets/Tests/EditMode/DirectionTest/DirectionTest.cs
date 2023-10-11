using NUnit.Framework;
using UnityEngine;

public class DirectionTest
{
    [Test]
    public void Dir()
    {
        Assert.AreEqual(new Vector3(0, 1, 0), Vector3.up);
    }
}
