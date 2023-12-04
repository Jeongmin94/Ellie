using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheatClient : OdinEditorWindow
{
    public Transform player;
    public Transform terrapupa;
    public Transform terra;
    public Transform pupa;

    [Title("사용자 지정 텔레포트")]
    [ValueDropdown("savePositionList")]
    public Vector3 savePosition = Vector3.zero;
    private List<Vector3> savePositionList = new List<Vector3>();

    [MenuItem("치트키/인게임 치트키")]
    private static void OpenWindow()
    {
        GetWindow<CheatClient>().Show();
    }

    private void Awake()
    {
        FindObject();
    }

    private void FindObject()
    {
        player = FindObject("Player");
        terrapupa = FindObject("Terrapupa");
        terra = FindObject("Terra");
        pupa = FindObject("Pupa");
    }

    private Transform FindObject(string objName)
    {
        GameObject obj = GameObject.Find(objName);
        if (obj != null)
        {
            return obj.transform;
        }
        else
        {
            Debug.LogError($"{objName} 오브젝트가 씬 안에 없습니다");
        }
        return null;
    }

    [Button("플레이어 포지션 저장", ButtonSizes.Medium)]
    public void SavePosition()
    {
        savePositionList.Add(player.position);
    }

    [Button("플레이어 저장 위치 리스트 리셋", ButtonSizes.Medium)]
    public void ResetPositionList()
    {
        savePositionList.Clear();
        savePosition = Vector3.zero;
    }

    [Button("선택한 저장위치로 플레이어 위치 이동", ButtonSizes.Medium)]
    public void SetPlayerPositionToSavedPosition()
    {
        player.position = savePosition;
    }

    [Title("지정 장소 이동")]
    [Button("시작 지점 이동", ButtonSizes.Small)]
    public void SetPlayerPosition1()
    {
        player.position = new Vector3(-5.44f, 7.03f, -10.4f);
    }

    [Button("보스 방 앞 이동", ButtonSizes.Small)]
    public void SetPlayerPosition2()
    {
        player.position = new Vector3(-152f, 13.92f, 645.51f);
    }

    [Button("돌 발판 퍼즐 이동", ButtonSizes.Small)]
    public void SetPlayerPosition3()
    {
        player.position = new Vector3(33.5f, 11.8f, 98.8f);
    }
}
