using System.Collections;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    class TestData
    {
        public string nickname;
        public int level;
        public int coin;
        public bool skill;
    }

    public class TestSaveData : MonoBehaviour
    {
        private string path;
        private string filename = "EllieTest";

        private TestData player = new TestData()
        {
            nickname = "플레이어",
            level = 5,
            coin = 200,
            skill = true,
        };

        private void Awake()
        {
            // 경로 설정
            path = Application.persistentDataPath + "/";

        }

        private void Start()
        {
            SaveData();
            LoadData();
        }

        public void SaveData()
        {
            string jsonData = JsonUtility.ToJson(player);
            Debug.Log(jsonData);

            // 자동 생성 경로
            Debug.Log(path);
            File.WriteAllText(path + filename, jsonData);
        }

        public void LoadData()
        {
            string data = File.ReadAllText(path + filename);
            TestData player2 = JsonUtility.FromJson<TestData>(data);
            Debug.Log(player2.nickname);
            Debug.Log(player2.level);
            Debug.Log(player2.coin);
            Debug.Log(player2.skill);
        }
    }
}