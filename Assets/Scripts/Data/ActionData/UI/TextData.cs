using UnityEngine;

namespace Data.ActionData.UI
{
    [CreateAssetMenu(fileName = "TextData", menuName = "UI/TextData")]
    public class TextData : ScriptableObject
    {
        public Data<string> TextValue = new();
    }
}