using System.Collections;
using Assets.Scripts.Centers;
using Assets.Scripts.Managers;
using Data.GoogleSheet;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Loading
{
    public class LoadingUI : MonoBehaviour
    {
        private const int loadingDataQuantity = 4;
        private const float spareTimeToLoad = 1.0f;

        private const float generalBarSpeed = 0.001f;
        private const float fasterBarSpeed = 0.005f;

        private const int imageQuantity = 8;
        public static readonly string ImagePath = "UI/Load/LoadingImages/Loading";

        [SerializeField] private Slider loadingSlider;
        [SerializeField] private Text tipText;
        [SerializeField] private Image loadingImage;

        [SerializeField] private LoadingTipsData tipData;

        private float barSpeed;

        private void Start()
        {
            UpdateImageTip();
            StartCoroutine(LoadLevelAsync(SceneLoadManager.Instance.CurrentScene));
        }

        private void UpdateImageTip()
        {
            var imagePath = ImagePath + Random.Range(0, imageQuantity);
            Debug.Log("IMAGE NAME : " + imagePath);
            loadingImage.sprite = ResourceManager.Instance.LoadSprite(imagePath);
            var tip = tipData.tips[Random.Range(0, tipData.tips.Count)];
            tipText.text = tip;
        }

        private IEnumerator LoadLevelAsync(SceneName scene)
        {
            barSpeed = generalBarSpeed;
            var targetLoad = 0.8f / loadingDataQuantity;

            var loadOperation = SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);

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
            if (SaveLoadManager.Instance.IsLoadData)
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
            {
                loadingSlider.value += barSpeed;
            }
        }
    }
}