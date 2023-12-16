using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class DataManager : Singleton<DataManager>
    {
        [InfoBox("체크 시 실시간 데이터 파싱을 진행합니다!\n빌드 시에는 꼭 체크해주세요!!!")]
        [SerializeField] private bool isDataParseInit = true;
        [SerializeField] private List<DataParsingInfo> dataList = new();

        [ShowInInspector][ReadOnly] private readonly Dictionary<Type, DataParsingInfo> dataDictionary = new Dictionary<Type, DataParsingInfo>();

        private GoogleSheetsParser parser;

        public bool isParseDone { get; private set; } = false;

        public override void Awake()
        {
            base.Awake();

            if(this.gameObject != null)
            {
                parser = Instance.gameObject.GetOrAddComponent<GoogleSheetsParser>();

                if (isDataParseInit)
                {
                    StartCoroutine(ParseData());
                }
                else
                {
                    Debug.LogError("현재 실시간 데이터 파싱을 진행하지 않습니다!!");
                    Debug.LogError("실시간 데이터 파싱이 필요하시면 체크를 풀어주세요.");

                    AddDataList();
                }
            }
        }

        private IEnumerator ParseData()
        {
            yield return parser.Parse(dataList);

            AddDataList();
        }

        private void AddDataList()
        {
            foreach (var data in dataList)
            {
                data.Parse();
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
            var wfs = new WaitForSeconds(0.5f);
            while (!isParseDone)
                yield return wfs;
        }
    }
}