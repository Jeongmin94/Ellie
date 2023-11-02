using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public abstract class BaseBTData : ScriptableObject
{
    protected string dataName;

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

    abstract public void Init(BehaviourTree tree);

}

public class BehaviourTreeController : MonoBehaviour
{
    public List<BaseBTData> settingList;

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
        Debug.Log(data);

        if(data != null)
        {
            data.Init(tree);
        }
    }
}