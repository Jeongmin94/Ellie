using Assets.Scripts.ActionData;
using UnityEngine;

namespace Assets.Scripts.Data.ActionData.UI
{
    [CreateAssetMenu(fileName = "TextData", menuName = "UI/TextData")]
    public class TextData : ScriptableObject
    {
        public Data<string> TextValue = new Data<string>();
    }
}