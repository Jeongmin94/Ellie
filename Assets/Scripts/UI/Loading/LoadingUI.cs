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

    [SerializeField] private TipsParshingInfo data;

    const int loadingDataQuantity = 2;
    const float spareTimeToLoad = 1.0f;

    const float generalBarSpeed = 0.001f;
    const float fasterBarSpeed = 0.005f;

    private float barSpeed;

    const int imageQuantity = 8;
    const int tipQuantity = 10;

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
        string tip = data.datas[Random.Range(0, tipQuantity)].tip;
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
        barSpeed = generalBarSpeed;
        yield return new WaitForSeconds(spareTimeToLoad);
        targetLoad += targetLoad;

        // + Add Load
        if(SaveLoadManager.Instance.IsLoadData)
        {
            // 로드 데이터 ㅇㅋ, 로딩바는 버그걸려서 못넣었읍니다
            yield return StartCoroutine(LoadSaveDataAsync());
        }
        else
        {
            // player save point

            // Finish Loading
            while (loadingSlider.value < 1.0f)
            {
                UpdateProgressBar(1.0f);
                yield return null;
            }
            yield return new WaitForSeconds(spareTimeToLoad);
        }
        SaveLoadManager.Instance.IsLoadData = false;

        loadingSlider.value = 0.0f;
        SceneLoadManager.Instance.FinishLoading();

        SceneManager.UnloadSceneAsync((int)SceneName.LoadingScene);
    }

    private void UpdateProgressBar(float max)
    {
        if (loadingSlider.value < max)
            loadingSlider.value += barSpeed;
    }

    private IEnumerator LoadSaveDataAsync()
    {
        Debug.Log("로드 시작");
        var loadDataTask = SaveLoadManager.Instance.LoadData();
        while (!loadDataTask.IsCompleted)
        {
            yield return null; // Task가 완료될 때까지 매 프레임 대기
        }

        if (loadDataTask.IsFaulted)
        {
            // 오류 처리
            Debug.LogError($"로드 중 오류 발생: {loadDataTask.Exception}");
        }
        else
        {
            Debug.Log("로드 완료");
        }
    }
}
