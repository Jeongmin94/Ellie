using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetsParser : MonoBehaviour
{
    public IEnumerator Parse(List<DataParsingInfo> dataList)
    {
        foreach (DataParsingInfo info in dataList)
        {
            yield return ParseData(info);
        }
    }

    private IEnumerator ParseData(DataParsingInfo info)
    {
        UnityWebRequest www = UnityWebRequest.Get(info.GetConvertedURL());
        yield return www.SendWebRequest();

        info.tsv = www.downloadHandler.text;
        print(info.tsv);
    }
}