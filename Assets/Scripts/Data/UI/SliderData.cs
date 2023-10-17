using UnityEngine;

namespace Assets.Scripts.Data.UI
{
    [CreateAssetMenu(fileName = "SliderData", menuName = "UI/SliderData")]
    public class SliderData : ScriptableObject
    {
        public Data<float> SliderValue = new Data<float>();
    }
}
