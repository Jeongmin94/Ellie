using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory
{
    public static class RectExtensions
    {
        public static Vector2 GetSize(this Rect rect)
        {
            return new Vector2(rect.width, rect.height);
        }

        public static Vector2 GetLeftTop(this Rect rect)
        {
            return new Vector2(rect.x, rect.y);
        }

        // -res.x/2.0f + rect.w/2.0f + rect.x
        public static Vector2 ToCanvasPos(this Rect rect)
        {
            var res = UIManager.Instance.resolution;
            return new Vector2(-res.x / 2.0f + rect.width / 2.0f + rect.x, res.y / 2.0f - rect.height / 2.0f - rect.y);
        }
    }

    public class InventoryConst
    {
        // x, y는 각각 figma layout의 left, top으로 사용
        public static Rect DescRect { get; } = new Rect(255, 131, 470, 685);
        public static Rect CtgyRect { get; } = new Rect(694, 144, 955, 667);
        public static Rect SlotRect { get; } = new Rect(400, 245, 160, 160);
    }
}