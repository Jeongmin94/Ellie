using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Puzzle
{
    public class StoneFootboardPuzzleController : MonoBehaviour
    {
        [SerializeField] private GameObject stalactitePrefab;
        [SerializeField] private List<Transform> spawnTransformList = new();

        private void Awake()
        {
            if (stalactitePrefab == null)
            {
                Debug.LogError("������ ����");
            }
        }

        private void Start()
        {
            InitStalactites();
        }

        private void InitStalactites()
        {
            foreach (var spawnTransform in spawnTransformList)
            {
                var position = spawnTransform.position;

                var stalactite = Instantiate(stalactitePrefab, position, Quaternion.identity, transform);
                var instantStalactite = stalactite.GetComponent<StoneFootboardPuzzleStalactite>();
                instantStalactite.SetLineRendererPosition();
            }
        }
    }
}