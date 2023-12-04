using Assets.Scripts.Puzzle;
using System.Collections.Generic;
using UnityEngine;

public class StoneFootboardPuzzleController : MonoBehaviour
{
    [SerializeField] private GameObject stalactitePrefab;
    [SerializeField] private List<Transform> spawnTransformList = new List<Transform>();

    private void Awake()
    {
        if(stalactitePrefab == null)
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
        Debug.Log(spawnTransformList.Count);
        foreach (var spawnTransform in spawnTransformList)
        {
            Vector3 position = spawnTransform.position;

            GameObject stalactite = Instantiate(stalactitePrefab, position, Quaternion.identity, transform);
            var instantStalactite = stalactite.GetComponent<StoneFootboardPuzzleStalactite>();
            instantStalactite.SetLineRendererPosition();
        }
    }
}