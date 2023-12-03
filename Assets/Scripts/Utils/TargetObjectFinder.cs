using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class TargetObjectFinder : MonoBehaviour
    {
        // 특정 레이어 가진 모든 오브젝트들을 찾아준다
        [InfoBox("씬 내의 모든 오브젝트들 중에\n레이어 마스크에 해당하는 애들만 리스트에 담김")]
        public LayerMask layerMask;
        public string findTag;

        // 결과 리스트
        public List<GameObject> targetObjects;

        [Button("레이어와 태그에 해당하는 모든 오브젝트 검색", ButtonSizes.Large)]
        public void FindObjectsByLayerAndTag()
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            targetObjects = new List<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (((1 << obj.layer) & layerMask) != 0 && (findTag == "" || obj.CompareTag(findTag)))
                {
                    targetObjects.Add(obj);
                }
            }
        }

        [Button("리스트 삭제", ButtonSizes.Large)]
        public void InitList()
        {
            targetObjects.Clear();
        }
    }
}