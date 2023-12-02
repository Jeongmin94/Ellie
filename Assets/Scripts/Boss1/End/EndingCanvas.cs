using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCanvas : MonoBehaviour
{
    public float extensionTime = 2.0f;
    
    private RectTransform whiteArea;

    private void Awake()
    {
        whiteArea = GetComponentInChildren<RectTransform>();
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
        Vector3 targetScale = new Vector3(11.0f, 11.0f, 11.0f);
        float currentTime = 0.0f;

        do
        {
            whiteArea.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }
        while (currentTime <= time);

        whiteArea.localScale = targetScale;
    }
}
