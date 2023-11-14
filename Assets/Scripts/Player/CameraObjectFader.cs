using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CameraObjectFader : MonoBehaviour
    {
        public float fadeSpeed, fadeAmount;
        private float originalOpacity;

        private Material mat;
        public bool DoFade = false;


        private void Start()
        {
            mat = GetComponent<Material>();
            originalOpacity = mat.color.a;
        }

        private void Update()
        {
            if (DoFade)
                FadeNow();
            else
                ResetFade();
        }
        private void FadeNow()
        {

        }

        private void ResetFade()
        {

        }
    }
}