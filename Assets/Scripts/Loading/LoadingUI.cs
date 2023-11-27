using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Loading;
using Assets.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    public static readonly string ImagePath = "UI/Load/Loading";

    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private Image loadingImage;

    [SerializeField] private TipsParshingInfo data;

    const int imageQuantity = 4;
    const int tipQuantity = 3;

    bool isLoad = false;

    void Update()
    {
        loadingSlider.value = AsyncLoadManager.Instance.GetLoadingProgress();
        if (!isLoad)
        {
            UpdateImageTip();
            isLoad = true;
        }
    }

    void UpdateImageTip()
    {
        string imagePath = ImagePath + Random.Range(0, imageQuantity).ToString();
        loadingImage.sprite = ResourceManager.Instance.LoadSprite(imagePath);
        string tip = data.datas[Random.Range(0, tipQuantity)].tip;
        tipText.text = tip;
    }
}
