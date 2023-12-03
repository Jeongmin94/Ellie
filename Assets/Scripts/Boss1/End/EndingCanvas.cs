using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCanvas : MonoBehaviour
{
    public RectTransform whiteArea;
    public float extensionTime = 2.0f;

    private TicketMachine ticketMachine;

    private void Awake()
    {
        ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();

        ticketMachine.AddTickets(ChannelType.UI);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(ScaleOverTime(extensionTime));
    }

    private void OnDisable()
    {
        whiteArea.localScale = Vector3.zero;
        StopAllCoroutines();
    }

    private IEnumerator ScaleOverTime(float time)
    {
        Vector3 originalScale = whiteArea.localScale;
        Vector3 targetScale = new Vector3(24.0f, 24.0f, 24.0f);
        float currentTime = 0.0f;

        do
        {
            whiteArea.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }
        while (currentTime <= time);

        whiteArea.localScale = targetScale;

        SoundManager.Instance.StopAllSounds();

        UIPayload payload = UIPayload.Notify();
        payload.actionType = ActionType.PlayVideo;
        ticketMachine.SendMessage(ChannelType.UI, payload);

        yield return new WaitForSeconds(1.0f);

        gameObject.SetActive(false);
    }
}
