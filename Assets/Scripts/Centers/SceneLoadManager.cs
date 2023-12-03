using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Centers
{
    //Build세팅 할 때 Scene 넘버 확인해서 추가 / 수정
    public enum SceneName
    {
        Opening,
        InGame,
        Closing,
        LoadingScene,
    }

    public class SceneLoadManager : Singleton<SceneLoadManager>
    {
        [SerializeField] public SceneName CurrentScene { get; private set; }
        public bool IsLoading { get; private set; } = true;

        public void LoadScene(SceneName sceneName)
        {
            IsLoading = true;
            CurrentScene = sceneName;

            //SaveLoadManager의 액션 구독 전부 해제
            SaveLoadManager.Instance.ClearActions();

            //로딩 화면이 필요한 경우 if 문에 추	
                //opening -> ingame, death->restart, savefileload 시 로드 필요 *기획
            if (CurrentScene == SceneName.InGame)
            {
                SceneManager.LoadScene((int)SceneName.LoadingScene);
            }
            else
            {
                SceneManager.LoadScene((int)sceneName);
                IsLoading = false;
            }
        }

        public void FinishLoading()
        {
            IsLoading = false;
        }

        //씬 전환을 위한 테스트용
        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    LoadScene(SceneName.Opening);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    LoadScene(SceneName.InGame);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha3))
            //{
            //    LoadScene(SceneName.Boss);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha4))
            //{
            //    LoadScene(SceneName.Closing);
            //}
        }

        public IEnumerator CheckIsLoadDone()
        {
            var wait = new WaitForSeconds(0.5f);
            while (IsLoading)
            {
                yield return wait;
            }
            Debug.Log("씬 로드 완료");
        }
    }


}