using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UI
{
    public class UITest
    {
        private void EnumTypeTest(Type type)
        {
            if (type.IsEnum)
            {
                Debug.Log($"{type.Name} is Enum");
            }
            string[] names = Enum.GetNames(type);
            foreach (var name in names)
            {
                Debug.Log($"name: {name}");
            }
        }

        private enum TestEnum
        {
            Idle, Walk, Die
        }
        
        [Test]
        public void EnumGetNamesTest()
        {
            // Use the Assert class to test conditions.
            EnumTypeTest(typeof(int));
        }
    }
}