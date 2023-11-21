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

public class BehaviourTreeController : MonoBehaviour
{
    [SerializeField] protected BehaviourTreeInstance behaviourTreeInstance;
    [SerializeField] protected BaseBTData rootTreeData;
    [SerializeField] private List<BaseBTData> settingList;

    protected virtual void Awake()
    {
        StartCoroutine(FallCheck());
    }

    private IEnumerator FallCheck()
    {
        LayerMask groundMask = LayerMask.GetMask("Ground");
        float checkDistance = 30.0f;
        float fallCheckLatency = 5.0f;
        float rayStartOffset = 10.0f;

        while (true)
        {
            RaycastHit hit;

            Vector3 rayStart = transform.position + Vector3.up * rayStartOffset;

            bool hitGround = Physics.Raycast(rayStart, -Vector3.up, out hit, checkDistance, groundMask);

            if (!hitGround)
            {
                Debug.Log("추락 방지, 포지션 초기화");
                transform.position = Vector3.zero;
            }

            yield return new WaitForSeconds(fallCheckLatency);
        }
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
}