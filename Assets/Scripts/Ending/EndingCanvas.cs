using Assets.Scripts.Centers;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingCanvas : MonoBehaviour
{
    public Color originColor = Color.black;
    public Color targetColor = Color.black;
    public RectTransform whiteArea;
    public float extensionTime = 2.0f;

    private Image image;

    private TicketMachine ticketMachine;

    private void Awake()
    {
        ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();

        ticketMachine.AddTickets(ChannelType.UI);

        image = whiteArea.gameObject.GetComponent<Image>();
        image.color = originColor;
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
        StopAllCoroutines();
    }

    private IEnumerator ScaleOverTime(float time)
    {
        float currentTime = 0.0f;
        do
        {
            image.color = Color.Lerp(originColor, targetColor, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);

        image.color = targetColor;
        SoundManager.Instance.ClearAction();

        SceneManager.LoadScene((int)SceneName.Closing);
    }
}