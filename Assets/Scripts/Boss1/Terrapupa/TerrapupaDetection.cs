using UnityEngine;

namespace Boss1.Terrapupa
{
    public class TerrapupaDetection : MonoBehaviour
    {
        [SerializeField] private Transform myTerrapupa;

        public Transform MyTerrapupa
        {
            get { return myTerrapupa; }
        }

        private void Start()
        {
            if (myTerrapupa == null)
            {
                Debug.LogError($"{transform} 테라푸파 트랜스폼 정보가 없습니다");
            }
        }
    }
}
