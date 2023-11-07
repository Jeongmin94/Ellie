using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data.GoogleSheet
{
    [Serializable]
    public class NPCData
    {
        public int idx;
        public string name;
        public string description;
        public bool isInteractable;
        public bool isTakingControl;
        public string speechBubble;
        public int questIdx;
        public List<int> dialogIndexList = new();
    }
    public class NPCDataParsingInfo : DataParsingInfo
    {
        public override T GetIndexData<T>(int index)
        {
            throw new NotImplementedException();
        }

        public override void Parse()
        {
            throw new NotImplementedException();
        }
    }
}
