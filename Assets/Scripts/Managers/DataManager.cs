using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class DataManager : Singleton<DataManager>
    {
        [SerializeField] private List<DataParsingInfo> dataList;

        private readonly Dictionary<Type, DataParsingInfo> dataDictionary = new Dictionary<Type, DataParsingInfo>();

        private GoogleSheetsParser parser;
        private bool isParseDone = false;

        public override void Awake()
        {
            base.Awake();

            parser = Instance.gameObject.GetOrAddComponent<GoogleSheetsParser>();
            StartCoroutine(ParseData());
        }

        private IEnumerator ParseData()
        {
            yield return parser.Parse(dataList);

            foreach (var data in dataList)
            {
                dataDictionary[data.GetType()] = data;
            }

            isParseDone = true;
        }

        public T GetData<T>() where T : DataParsingInfo
        {
            if (dataDictionary.TryGetValue(typeof(T), out DataParsingInfo data))
            {
                return data as T;
            }
            else
            {
                Debug.LogError("반환 실패");
                return null;
            }
        }

        public T GetIndexData<T, U>(int index) where U : DataParsingInfo where T : class
        {
            var data = GetData<U>();
            if (data != null)
            {
                return data.GetIndexData<T>(index);
            }
            else
            {
                return default(T);
            }
        }

        public IEnumerator CheckIsParseDone()
        {
            var wfs = new WaitForEndOfFrame();
            while (!isParseDone)
                yield return wfs;
        }
    }
}