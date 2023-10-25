using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GoogleSheetsParser : MonoBehaviour
{
    public List<DataParsingInfo> dataList = new List<DataParsingInfo>();

    private void Start()
    {
        foreach (DataParsingInfo info in dataList)
        {
            StartCoroutine(ParseData(info));
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    foreach (DataParsingInfo info in dataList)
        //    {
        //        StartCoroutine(ParseData(info));
        //    }
        //}
        //if(Input.GetKeyDown(KeyCode.S))
        //{
        //    MonsterDataParsingInfo data = dataList[0] as MonsterDataParsingInfo;
        //    data.monsters.Clear();
        //}
    }

    private IEnumerator ParseData(DataParsingInfo info)
    {
        UnityWebRequest www = UnityWebRequest.Get(info.GetConvertedURL());
        print(info.GetConvertedURL());
        yield return www.SendWebRequest();

        info.tsv = www.downloadHandler.text;
        print(info.tsv);
        info.Parse();
    }
}
