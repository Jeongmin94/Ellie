﻿using Assets.Scripts.InteractiveObjects;
using Assets.Scripts.Item.Stone;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.UI.Inventory.Test;
using Centers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Centers.Test
{
    //Build세팅 할 때 Scene 넘버 확인해서 추가 / 수정
    public enum SceneName
    {
        Opening,
        InGame,
        Boss,
        Closing,
        LoadingScene,
    }

    public class TestCenterWithScene : Singleton<TestCenterWithScene>
    {
        [SerializeField] public SceneName CurrentScene { get; private set; }
        public bool IsLoading { get; private set; }

        public void LoadScene(SceneName sceneName)
        {
            IsLoading = true;
            CurrentScene = sceneName;

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                LoadScene(SceneName.Opening);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                LoadScene(SceneName.InGame);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                LoadScene(SceneName.Boss);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                LoadScene(SceneName.Closing);
            }
        }
    }


}