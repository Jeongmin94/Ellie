using System.Collections.Generic;
using Sirenix.OdinInspector;
using TheKiwiCoder;
using UnityEngine;

public abstract class BehaviourTreeData : ScriptableObject
{
    public string dataName;
    protected BehaviourTree tree;

    public BehaviourTree Tree
    {
        get { return tree; }
        set
        {
            if (tree == null)
            {
                tree = value;
            }
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
    [SerializeField] protected BehaviourTreeData rootTreeData;
    [SerializeField] private List<BehaviourTreeData> settingList;
    
    protected virtual void Awake()
    {
        // 추가사항
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

    public T Data<T>(string dataName) where T : BehaviourTreeData
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

    public BehaviourTreeData Search(string dataName)
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
        var data = Search(dataName);

        if (data != null)
        {
            data.Tree = tree;
            data.Init(tree);
        }
    }
}
