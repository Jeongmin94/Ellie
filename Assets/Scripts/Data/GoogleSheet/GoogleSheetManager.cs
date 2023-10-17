using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetManager : MonoBehaviour
{
    public List<DataParsing> dataList = new List<DataParsing>();

    private const string URL = "https://docs.google.com/spreadsheets/d/1GYuPFPnkAFIRdZj5y3eVHvvbrY4T2yDQCfcae04vraI/export?format=tsv&gid=976557839&range=B4:L";

    IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        print(data);
    }
}
