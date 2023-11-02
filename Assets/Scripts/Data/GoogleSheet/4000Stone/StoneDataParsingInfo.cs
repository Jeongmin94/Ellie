using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data.GoogleSheet
{
    [Serializable]
    public class StoneData
    {
        public int index;
        public string name;
        public string description;
        public int appearanceStage;
        public int tear;

        public string imageName;
    }

    [CreateAssetMenu(fileName = "StoneData", menuName = "GameData List/StoneData")]
    public class StoneDataParsingInfo : DataParsingInfo
    {
        public List<StoneData> items;

        public override void Parse()
        {
            throw new NotImplementedException();
        }

        public override T GetIndexData<T>(int index)
        {
            if (typeof(T) == typeof(StoneData))
            {
                return items.Find(m => m.index == index) as T;
            }

            return default(T);
        }
    }
}