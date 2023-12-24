using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Data.GoogleSheet
{
    public class GoogleSheetsParser : MonoBehaviour
    {
        public IEnumerator Parse(List<DataParsingInfo> dataList)
        {
            foreach (var info in dataList)
            {
                yield return ParseData(info);
            }
        }

        private IEnumerator ParseData(DataParsingInfo info)
        {
            var www = UnityWebRequest.Get(info.GetConvertedURL());
            yield return www.SendWebRequest();

            info.tsv = www.downloadHandler.text;
            print(info.tsv);
        }
    }
}