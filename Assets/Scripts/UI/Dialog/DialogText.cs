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
        public bool IsPlaying { get; set; } = false;
        public bool OnNext { get; set; } = false;

        private IEnumerator playingEnumerator;
        private TextMeshProUGUI dialogText;

        private readonly Queue<string> dialogQueue = new Queue<string>();
        private readonly StringBuilder sb = new StringBuilder();
        private string currentText;
        private float currentInterval;

        public void InitDialogText()
        {
            dialogText = gameObject.FindChild<TextMeshProUGUI>(null, true);
        }

        // !TODO: 마지막 글자 커서 깜빡임 추가
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
            StopCoroutine(playingEnumerator);

            IsPlaying = false;
            OnNext = false;
            sb.Clear();

            dialogText.text = currentText;
        }

        public void Pause()
        {
            StopCoroutine(playingEnumerator);
        }

        public void Resume()
        {
            playingEnumerator = ReadText(currentText.Substring(dialogText.text.Length), currentInterval);
            StartCoroutine(playingEnumerator);
        }

        private IEnumerator ReadText(string text, float interval)
        {
            IsPlaying = true;
            currentText = text;
            currentInterval = interval;
            WaitForSeconds wfs = new WaitForSeconds(interval);

            foreach (var ch in text)
            {
                if (OnNext)
                {
                    dialogText.text = text;
                    break;
                }

                sb.Append(ch);
                dialogText.text = sb.ToString();

                yield return wfs;
            }

            IsPlaying = false;
            OnNext = false;
            sb.Clear();
        }
    }
}