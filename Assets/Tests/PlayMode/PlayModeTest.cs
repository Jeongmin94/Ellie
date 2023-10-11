using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayModeTest
{
    [UnityTest]
    public IEnumerator MoveRightDirection()
    {
        float speed = 1.0f;

        var gameObject = new GameObject();
        gameObject.transform.position = Vector3.zero;

        int maxCount = 2;
        int count = maxCount;
        while (count > 0)
        {
            gameObject.transform.Translate(Vector3.right * speed);
            count--;
            yield return null;
        }

        Assert.AreEqual(new Vector3(1 * maxCount, 0, 0), gameObject.transform.position);
    }

    [UnityTest]
    public IEnumerator PlayerControllerTest()
    {
        var go = new GameObject();
        go.AddComponent<Rigidbody>();
        go.AddComponent<Animator>();
        var pc = go.AddComponent<PlayerController>();

        var so = new SerializedObject(pc);
        var prop = so.FindProperty("crossHair");

        yield return null;

        Assert.NotNull(pc);
    }
}
