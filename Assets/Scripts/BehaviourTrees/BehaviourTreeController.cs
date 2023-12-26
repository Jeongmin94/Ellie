using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public abstract class BaseBTData : ScriptableObject
{
    public string dataName;
    protected BehaviourTree tree;

    public BehaviourTree Tree
    {
        get
        {
            return tree;
        }
        set
        {
            if (tree == null)
                tree = value;
        }
    }

    public string DataName
    {
        get { return dataName; }
    }

    public BlackboardKey<T> FindBlackboardKey<T>(string keyName, BehaviourTree tree)
    {
        if (tree)
        {
            return tree.blackboard.Find<T>(keyName);
        }
        return null;
    }

    public void SetBlackboardValue<T>(string keyName, T value, BehaviourTree tree)
    {
        if (tree)
        {
            tree.blackboard.SetValue(keyName, value);
        }
    }

    public abstract void Init(BehaviourTree tree);

}

public class BehaviourTreeController : SerializedMonoBehaviour
{
    [SerializeField] protected BehaviourTreeInstance behaviourTreeInstance;
    [SerializeField] protected BaseBTData rootTreeData;
    [SerializeField] protected Transform billboardObject;
    [SerializeField] private List<BaseBTData> settingList;

    [SerializeField] private Transform cameraObj;
    private bool isBillboardOn;

    protected virtual void Awake()
    {
        cameraObj = Camera.main.transform;
    }

    private void LateUpdate()
    {
        MonsterOnPlayerForward();
    }

    public void InitRootTree(BehaviourTree tree)
    {
        if (tree != null)
        {
            rootTreeData.Tree = tree;
            rootTreeData.Init(tree);
        }
        else
        {
            Debug.Log("RootTree Null");
        }
    }

    public T Data<T>(string dataName) where T : BaseBTData
    {
        foreach (var setting in settingList)
        {
            if (setting.DataName == dataName)
            {
                return setting as T;
            }
        }
        return null;
    }

    public BaseBTData Search(string dataName)
    {
        foreach (var setting in settingList)
        {
            if (setting.DataName == dataName)
            {
                return setting;
            }
        }
        return null;
    }

    public void RegisterBlackboardData(string dataName, BehaviourTree tree)
    {
        BaseBTData data = Search(dataName);

        if (data != null)
        {
            data.Tree = tree;
            data.Init(tree);
        }
    }

    public void ShowBillboard()
    {
        billboardObject.transform.localScale = Vector3.one;
        isBillboardOn = true;
    }

    public void HideBillboard()
    {
        billboardObject.transform.localScale = Vector3.zero;
    }

    public void MonsterOnPlayerForward()
    {
        if (isBillboardOn)
        {
            Vector3 direction = transform.position - cameraObj.position;
            float dot = Vector3.Dot(direction.normalized, cameraObj.forward.normalized);

            if (dot > -0.6f)
            {
                ShowBillboard();
            }
            else
            {
                HideBillboard();
            }
        }
    }
}