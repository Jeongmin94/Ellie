using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Data.UI.Dialog;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Dialog
{
    public class DialogText : MonoBehaviour
    {
        private bool IsPlaying { get; set; } = false;
        private bool OnNext { get; set; } = false;

        private IEnumerator playingEnumerator;
        private IEnumerator blinkEnumerator;
        private TextMeshProUGUI dialogText;

        private readonly Queue<string> dialogQueue = new Queue<string>();
        private readonly StringBuilder sb = new StringBuilder();
        private string currentText = string.Empty;
        private float currentInterval = 0.0f;
        private bool onPause = false;

        private void Awake()
        {
            blinkEnumerator = Blink(string.Empty);
        }

        private void OnDisable()
        {
            if (playingEnumerator != null)
                StopCoroutine(playingEnumerator);

            if (blinkEnumerator != null)
                StopCoroutine(blinkEnumerator);
        }

        public void InitDialogText()
        {
            dialogText = gameObject.FindChild<TextMeshProUGUI>(null, true);
        }

        // !TODO: 큐를 이용한 넘기기 기능 추가
        public void InitTypography(DialogTypographyData typographyData)
        {
            dialogText.font = typographyData.fontAsset;
            dialogText.fontSize = typographyData.fontSize;
            dialogText.lineSpacing = typographyData.lineSpacing;
            dialogText.color = typographyData.color;
        }

        public void AddDialog(string text)
        {
            dialogQueue.Enqueue(text);
        }

        public void ClearDialog()
        {
            dialogQueue.Clear();
        }

        public void Play(string text, float interval)
        {
            playingEnumerator = ReadText(text, interval);
            StartCoroutine(playingEnumerator);
        }

        public void Stop()
        {
            if (!IsPlaying)
                return;

            OnNext = true;
        }

        public void Next()
        {
            if (!IsPlaying)
                return;

            OnNext = true;
        }

        public void Pause()
        {
            if (!IsPlaying)
                return;

            onPause = true;
        }

        public void Resume()
        {
            if (!IsPlaying)
                return;

            onPause = false;
        }

        private IEnumerator ReadText(string text, float interval)
        {
            IsPlaying = true;
            currentText = text;
            currentInterval = interval;

            WaitForSeconds wfs = new WaitForSeconds(interval);
            WaitForFixedUpdate wff = new WaitForFixedUpdate();

            foreach (var ch in text)
            {
                while (onPause)
                    yield return wff;

                if (OnNext)
                    break;

                sb.Append(ch);
                dialogText.text = sb.ToString();

                yield return wfs;
            }

            IsPlaying = false;
            OnNext = false;
            onPause = false;
            dialogText.text = text;
            sb.Clear();

            blinkEnumerator = Blink(text);
            StartCoroutine(blinkEnumerator);
        }

        // 현재 텍스트 출력 후 깜빡거리는 효과 추가
        // 다음 텍스트 출력이나 DialogText가 비활성화되면 멈춤
        private IEnumerator Blink(string text)
        {
            WaitForSeconds wfs = new WaitForSeconds(0.5f);
            bool isBlink = false;
            string onBlink = text + '|';

            while (!IsPlaying)
            {
                dialogText.text = isBlink ? onBlink : text;

                isBlink = !isBlink;
                yield return wfs;
            }
        }
    }
}