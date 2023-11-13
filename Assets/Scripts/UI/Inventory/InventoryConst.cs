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

        public static Vector2 ToCanvasPos(this Rect rect)
        {
            var res = UIManager.Instance.resolution;
            return new Vector2(-res.x / 2.0f + rect.width / 2.0f + rect.x, res.y / 2.0f - rect.height / 2.0f - rect.y);
        }
    }

    public class InventoryConst
    {
        #region Rect

        // x, y는 각각 figma layout의 left, top으로 사용
        // 카테고리 패널
        public static Rect CtgyRect { get; } = new Rect(694, 144, 955, 667);
        public static Rect SlotAreaRect { get; } = new Rect(759, 256, 824, 315);
        public static Rect EquipSlotAreaRect { get; } = new Rect(914, 596, 515, 103);
        public static Rect SlotRect { get; } = new Rect(400, 245, 160, 160);

        // 설명 패널
        public static Rect DescRect { get; } = new Rect(255, 131, 470, 685);
        public static Rect DescTextRect { get; } = new Rect(314, 467, 359, 159);
        public static Rect DescNameRect { get; } = new Rect(302, 131, 375, 102);
        public static Rect DescImageRect { get; } = new Rect(273, 200, 428, 396);

        // 금 및 돌멩이 패널
        public static Rect GoldRect { get; } = new Rect(1355, 714, 199, 35);
        public static Rect GoldAreaRect { get; } = new Rect(1355, 714, 35, 35);
        public static Rect GoldAreaCountRect { get; } = new Rect(1392, 720, 56, 25);
        public static Rect StonePieceAreaRect { get; } = new Rect(1460, 714, 35, 35);
        public static Rect StonePieceAreaCountRect { get; } = new Rect(1498, 720, 56, 25);

        // 닫기 버튼
        public static Rect CloseButtonRect { get; } = new Rect(1632, 194, 64, 62);

        #endregion

        #region Toggle

        public static readonly int ToggleOnFontSize = 45;
        public static readonly int ToggleOffFontSize = 31;
        public static Color ToggleOnFontColor { get; } = new Color32(229, 168, 61, 255);
        public static Color ToggleOffFontColor { get; } = new Color32(152, 135, 106, 255);

        #endregion
    }
}