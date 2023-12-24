using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Data.UI.Dialog;
using TMPro;
using UnityEngine;
using Utils;

namespace UI.Dialog
{
    public class DialogText : MonoBehaviour
    {
        private readonly Queue<string> dialogQueue = new();
        private readonly StringBuilder sb = new();
        private IEnumerator blinkEnumerator;
        private float currentInterval;
        private string currentText = string.Empty;
        private TextMeshProUGUI dialogText;
        private Action<bool> isPlayingAction;
        private bool onBlink;
        private bool onPause;

        private IEnumerator playingEnumerator;
        public bool IsPlaying { get; set; }
        private bool OnNext { get; set; }

        private void Awake()
        {
            blinkEnumerator = Blink(string.Empty);
        }

        private void OnDisable()
        {
            if (playingEnumerator != null)
            {
                StopCoroutine(playingEnumerator);
            }

            if (blinkEnumerator != null)
            {
                StopCoroutine(blinkEnumerator);
            }
        }

        public void InitDialogText()
        {
            dialogText = gameObject.FindChild<TextMeshProUGUI>(null, true);
        }

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

        public IEnumerator Play(string text, float interval, float duration)
        {
            playingEnumerator = ReadText(text, interval, duration);
            yield return StartCoroutine(playingEnumerator);
        }

        public bool Stop()
        {
            if (IsPlaying)
            {
                return false;
            }

            onBlink = false;
            return true;
        }

        public void Next()
        {
            if (!IsPlaying)
            {
                return;
            }

            OnNext = true;
        }

        public void SetPause(bool pause)
        {
            if (!IsPlaying)
            {
                return;
            }

            onPause = pause;
        }

        private IEnumerator ReadText(string text, float interval, float duration = 0.0f)
        {
            StopCoroutine(blinkEnumerator);

            IsPlaying = true;

            //여기서 페이로드 쏘기
            PublishIsPlayingAction(true);
            currentText = text;
            currentInterval = interval;

            var wfs = new WaitForSeconds(interval);
            var wfef = new WaitForEndOfFrame();

            foreach (var ch in text)
            {
                if (OnNext)
                {
                    break;
                }

                sb.Append(ch);
                dialogText.text = sb.ToString();

                yield return wfs;

                while (onPause)
                {
                    yield return wfef;
                }
            }

            IsPlaying = false;
            //여기서 페이로드 쏘기
            PublishIsPlayingAction(false);
            OnNext = false;
            onPause = false;
            dialogText.text = text;
            sb.Clear();

            onBlink = true;
            blinkEnumerator = Blink(text);
            StartCoroutine(blinkEnumerator);
            if (duration > 0.0f)
            {
                yield return StartCoroutine(Reserve(duration));
            }
        }

        // 현재 텍스트 출력 후 깜빡거리는 효과 추가
        // 다음 텍스트 출력이나 DialogText가 비활성화되면 멈춤
        private IEnumerator Blink(string text)
        {
            var wfs = new WaitForSeconds(0.5f);
            var isBlink = false;
            var blinkText = text + '|';

            while (onBlink)
            {
                dialogText.text = isBlink ? blinkText : text;

                isBlink = !isBlink;
                yield return wfs;
            }
        }

        private IEnumerator Reserve(float duration)
        {
            var timeAcc = 0.0f;
            var wfef = new WaitForEndOfFrame();

            while (timeAcc <= duration)
            {
                timeAcc += Time.deltaTime;
                yield return wfef;
            }

            onBlink = false;
        }

        public void SubscribeIsPlayingAction(Action<bool> listener)
        {
            isPlayingAction -= listener;
            isPlayingAction += listener;
        }

        private void PublishIsPlayingAction(bool b)
        {
            isPlayingAction?.Invoke(b);
        }
    }
}