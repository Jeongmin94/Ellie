using UnityEngine;

namespace Data.ActionData.UI
{
    [CreateAssetMenu(fileName = "SliderData", menuName = "UI/SliderData")]
    public class SliderData : ScriptableObject
    {
        public Data<float> SliderValue = new();
    }
}