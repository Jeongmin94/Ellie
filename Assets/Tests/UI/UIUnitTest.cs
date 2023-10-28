using System;
using Assets.Scripts.UI.Framework.Test;
using Assets.Scripts.Utils;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tests.UI
{
    public class UIUnitTest
    {
        private const string ReferencePath = "TestReferences/UI/UITestReference";

        private GameObject uiPrefab;
        private UITest uiTest;

        [SetUp]
        public void Setup()
        {
            var go = Resources.Load(ReferencePath, typeof(GameObject)) as GameObject;
            uiPrefab = go.GetComponent<UITestReference>().UIPrefab;
        }

        private void ClearScene()
        {
            Transform[] objects = Object.FindObjectsOfType<Transform>();
            foreach (Transform obj in objects)
            {
                if (obj != null)
                    Object.DestroyImmediate(obj.gameObject);
            }

            uiTest = null;
        }

        [Test]
        public void _01_UIPrefabExists()
        {
            Assert.NotNull(uiPrefab);
        }

        [Test]
        public void _02_InitUIPrefab()
        {
            ClearScene();
            var go = Object.Instantiate(uiPrefab);
            uiTest = go.GetComponent<UITest>();

            Assert.NotNull(uiTest);
        }
    }
}