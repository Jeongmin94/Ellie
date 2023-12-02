using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public abstract class AlphaController<T> : MonoBehaviour where T : Component
    {
        public abstract IEnumerator ChangeAlpha(Color start, Color end, float duration);
    }
}