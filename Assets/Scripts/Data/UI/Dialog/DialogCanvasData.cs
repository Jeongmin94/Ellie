using UnityEngine;

namespace Data.UI.Dialog
{
    [CreateAssetMenu(fileName = "DialogCanvasData", menuName = "UI/Dialog/DialogCanvasData", order = 1)]
    public class DialogCanvasData : ScriptableObject
    {
        [SerializeField] private float dialogInterval;
    }
}