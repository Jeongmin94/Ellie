using UnityEngine;

namespace Assets.Scripts.Data.UI
{
    [CreateAssetMenu(fileName = "DialogCanvasData", menuName = "UI/Dialog/DialogCanvasData", order = 1)]
    public class DialogCanvasData : ScriptableObject
    {
        [SerializeField] private float dialogInterval;
    }
}