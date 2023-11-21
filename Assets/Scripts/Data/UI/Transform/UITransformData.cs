using Assets.Scripts.ActionData;
using UnityEngine;

namespace Assets.Scripts.Data.UI.Transform

{
    [CreateAssetMenu(fileName = "UITransform", menuName = "UI/TransformData", order = 0)]
    public class UITransformData : ScriptableObject, ISerializationCallbackReceiver
    {
        [Header("x, y, width, height")] public Rect rect;
        [Header("x, y scale")] public Vector2 scale;

        public readonly Data<Rect> actionRect = new Data<Rect>();
        public readonly Data<Vector2> actionScale = new Data<Vector2>();

        public void OnBeforeSerialize()
        {
            rect = actionRect.Value;
            scale = actionScale.Value;
        }

        public void OnAfterDeserialize()
        {
            actionRect.Value = rect;
            actionScale.Value = scale;
        }
    }
}