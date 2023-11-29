using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISaturation : MonoBehaviour
{
    private const float saturationTime = 2.0f;
    private const float saturationAmount = 50.0f;

    private Image image;
    private float accumTime = 0.0f;
    private int saturationUp = -1;

    private void Start()
    {
        image = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        accumTime += Time.deltaTime;
        if (accumTime > saturationTime)
        {
            saturationUp *= -1;
            accumTime = 0.0f;
        }
        image.color = AdjustColor(image.color);
    }

    private Color AdjustColor(Color color)
    {
        float saturation = color.r + saturationAmount / saturationTime * Time.deltaTime * saturationUp;
        Debug.Log(saturation);
        return new Color(saturation, color.g, color.b, color.a);
    }
}
