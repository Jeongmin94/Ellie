using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.Managers;
using TMPro;

namespace Assets.Scripts.Loading
{
    public class AsyncLoadManager : Singleton<AsyncLoadManager>
    {
        const int loadingDataQuantity = 2;
        const float spareTimeToLoad = 1.0f;

        const float generalBarSpeed = 0.001f;
        const float fasterBarSpeed = 0.005f;

        private float barSpeed;
        float loadingSlider;

        private bool isLoading;

        public void LoadScene(string levelToLoad)
        {
            SceneManager.LoadScene("LoadingScene");

            isLoading = true;

            StartCoroutine(LoadLevelAsync(levelToLoad));

            loadingSlider = 0.0f;
        }

        private IEnumerator LoadLevelAsync(string levelToLoad)
        {
            barSpeed = generalBarSpeed;
            float targetLoad = 0.8f / loadingDataQuantity;

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad,LoadSceneMode.Additive);


            //Load Map
            while (!loadOperation.isDone)
            {
                UpdateProgressBar(targetLoad);
                yield return null;
            }

            barSpeed = fasterBarSpeed;
            while (loadingSlider<targetLoad)
            {
                UpdateProgressBar(targetLoad);
                yield return null;
            }
            targetLoad += targetLoad;

            //Load Parsing
            yield return DataManager.Instance.CheckIsParseDone();

            barSpeed = fasterBarSpeed;
            while(loadingSlider< targetLoad)
            {
                UpdateProgressBar(targetLoad);
                yield return null;
            }
            barSpeed = generalBarSpeed;
            yield return new WaitForSeconds(spareTimeToLoad);
            targetLoad += targetLoad;

            // + Add Load
            // player save point

            // Finish Loading
            while (loadingSlider<1.0f)
            {
                UpdateProgressBar(1.0f);
                yield return null;
            }
            yield return new WaitForSeconds(spareTimeToLoad);

            isLoading = false;

            SceneManager.UnloadSceneAsync("LoadingScene");
        }

        public bool CheckIsLoading()
        {
            return isLoading;
        }

        private void UpdateProgressBar(float max)
        {
            if (loadingSlider < max)
                loadingSlider += barSpeed;
        }

        public float GetLoadingProgress()
        {
            return loadingSlider;
        }

    }
}
