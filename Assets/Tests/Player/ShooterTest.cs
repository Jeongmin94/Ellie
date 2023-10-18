using System;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tests.Player
{
    public class ShooterTest
    {
        private readonly string referencePath = "TestReferences/Player/ShooterTestReference";

        private GameObject shooterPrefab;
        private Shooter shooter;

        [SetUp]
        public void Setup()
        {
            var go = Resources.Load(referencePath, typeof(GameObject)) as GameObject;
            shooterPrefab = go.GetComponent<ShooterTestReference>().shooterPrefab;
        }

        private void ClearScene()
        {
            Transform[] objects = Object.FindObjectsOfType<Transform>();
            foreach (Transform obj in objects)
            {
                if (obj != null)
                    Object.DestroyImmediate(obj.gameObject);
            }
        }

        [Test]
        public void _01_ShooterPrefabExists()
        {
            Assert.NotNull(shooterPrefab);
        }

        [Test]
        public void _02_InitShooter()
        {
            ClearScene();
            var go = (GameObject)Object.Instantiate(shooterPrefab);
            shooter = go.GetComponent<Shooter>();

            Assert.NotNull(shooter);
        }

        // !TODO: 테스트 코드에서 Start, Awake 등의 event functions 사용하는 방법 리서치
        // !TODO: or private 메서드 테스트 하는 방법 리서치
        [Test]
        public void _03_TestChargingRatio()
        {
            ClearScene();

            var go = (GameObject)Object.Instantiate(shooterPrefab);
            shooter = go.GetComponent<Shooter>();
            shooter.SubscribeAction();

            var steps = shooter.ChargingData.timeSteps;
            var percentages = shooter.ChargingData.percentages;

            int count = Math.Min(steps.Length, percentages.Length);
            for (int i = 0; i < count; i++)
            {
                shooter.ChargingData.ChargingValue.Value = steps[i];
                Debug.Log($"step: {steps[i]}, expected: {percentages[i]}, actual: {shooter.ChargingRatio}");

                Assert.GreaterOrEqual(shooter.ChargingRatio, percentages[i]);
            }
        }
    }
}