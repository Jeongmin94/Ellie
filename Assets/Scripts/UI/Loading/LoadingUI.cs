using System.Collections;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Centers;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LoadingUI : MonoBehaviour
{
    public static readonly string ImagePath = "UI/Load/LoadingImages/Loading";

    [SerializeField] private Slider loadingSlider;
    [SerializeField] private Text tipText;
    [SerializeField] private Image loadingImage;

    [SerializeField] private LoadingTipsData tipData;

    const int loadingDataQuantity = 4;
    const float spareTimeToLoad = 1.0f;

    const float generalBarSpeed = 0.001f;
    const float fasterBarSpeed = 0.005f;

    private float barSpeed;

    const int imageQuantity = 8;

    private void Start()
    {
        UpdateImageTip();
        StartCoroutine(LoadLevelAsync(SceneLoadManager.Instance.CurrentScene));
    }

    void UpdateImageTip()
    {
        string imagePath = ImagePath + Random.Range(0, imageQuantity).ToString();
        Debug.Log("IMAGE NAME : " + imagePath);
        loadingImage.sprite = ResourceManager.Instance.LoadSprite(imagePath);
        string tip = tipData.tips[Random.Range(0, tipData.tips.Count)];
        tipText.text = tip;
    }

    private IEnumerator LoadLevelAsync(SceneName scene)
    {
        barSpeed = generalBarSpeed;
        float targetLoad = 0.8f / loadingDataQuantity;

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);

        //Load Map
        while (!loadOperation.isDone)
        {
            UpdateProgressBar(targetLoad);
            yield return null;
        }
        barSpeed = fasterBarSpeed;
        while (loadingSlider.value < targetLoad)
        {
            UpdateProgressBar(targetLoad);
            yield return null;
        }
        targetLoad += targetLoad;

        //Load Parsing
        yield return DataManager.Instance.CheckIsParseDone();
        barSpeed = fasterBarSpeed;
        while (loadingSlider.value < targetLoad)
        {
            UpdateProgressBar(targetLoad);
            yield return null;
        }
        targetLoad += targetLoad;
        // + Add Load
        if(SaveLoadManager.Instance.IsLoadData == true)
        {
            SaveLoadManager.Instance.LoadData();
            yield return SaveLoadManager.Instance.CheckIsLoadDone();
        }

        barSpeed = fasterBarSpeed;
        while (loadingSlider.value < targetLoad)
        {
            UpdateProgressBar(targetLoad);
            yield return null;
        }

        yield return new WaitForSeconds(spareTimeToLoad);

        while (loadingSlider.value < 1.0f)
        {
            UpdateProgressBar(1.0f);
            yield return null;
        }
        yield return new WaitForSeconds(spareTimeToLoad);

        SceneLoadManager.Instance.FinishLoading();
        SceneManager.UnloadSceneAsync((int)SceneName.LoadingScene);
    }

    private void UpdateProgressBar(float max)
    {
        if (loadingSlider.value < max)
            loadingSlider.value += barSpeed;
    }
}
