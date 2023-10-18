using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetsParser : MonoBehaviour
{
    public List<DataParsingInfo> dataList = new List<DataParsingInfo>();

    //private const string URL = "https://docs.google.com/spreadsheets/d/1GYuPFPnkAFIRdZj5y3eVHvvbrY4T2yDQCfcae04vraI/export?format=tsv&gid=976557839&range=B4:L";

    void Start()
    {
        foreach (DataParsingInfo info in dataList)
        {
            StartCoroutine(ParseData(info));
        }
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
