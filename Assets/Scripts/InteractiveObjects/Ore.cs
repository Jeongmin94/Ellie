using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects
{
    public class Ore : MonoBehaviour, IInteractiveObject
    {
        public enum Tier
        {
            Tier3 = 9100,
            Tier2,
            Tier1,
        }

        [SerializeField] private GameObject stonePrefabTest;
        private Transform oreBody;
        private Transform stoneSpawnPos;
        public OreData data;
        public Tier tier;

        public int hardness;
        public int maxHp;
        public int hp;
        //체력 4분할
        public List<int> quateredHP;
        private int curHpInterval;

        public float regenerationTime = 4f;
        public bool canMine;
        private void Start()
        {
            StartCoroutine(InitOre());
            oreBody = transform.GetChild(0);
            stoneSpawnPos = transform.GetChild(1);
            canMine = true;

        }

        private IEnumerator InitOre()
        {
            yield return new WaitForSeconds(1.0f);
            data = DataManager.Instance.GetIndexData<OreData, OreDataParsingInfo>((int)tier);
            hardness = data.hardness;
            maxHp = data.HP;
            hp = maxHp;
            curHpInterval = 3;
            SetQuateredHP();
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponentInParent<PlayerController>().SetCurOre(null);
            }
        }
        public void Smith(int damage)
        {
            hp -= damage;
            int idx = 0;
            //1/4만큼 체력이 감소할 때 마다 아이템 뱉기
            for (int i = 0; i < 3; i++)
            {
                if (hp >= quateredHP[i] && hp <= quateredHP[i + 1] && curHpInterval != i)
                {
                    //구간을 두 번 지나쳤을 때 두 번 드랍시키기 위함
                    idx = curHpInterval - i;
                    curHpInterval = i;
                }
            }
            while (idx > 0)
            {
                Debug.Log("during mining");
                DropStone(data.whileMiningDropItemList);
                idx--;
            }
            if (hp <= 0)
            {
                canMine = false;
                Debug.Log("mining complete");
                //아이템 뱉기
                DropStone(data.miningEndDropItemList);
                StartCoroutine(Regenerate());
            }
        }

        private void SetQuateredHP()
        {
            int temp = maxHp;
            //숫자 더해서 4의 배수로 만들기a
            while (temp % 4 != 0)
            {
                temp++;
            }
            int segment = temp / 4;
            quateredHP = new();
            //4분할된 구간을 만들기
            for (int i = 0; i < 4; i++)
            {
                quateredHP.Add(maxHp - segment * i);
            }
            quateredHP.Add(0);
            quateredHP.Reverse();
        }
        private void DropStone(List<(int, float)> dropItemList)
        {
            float rand = Random.Range(0f, 1.0f);
            float accChance = 0f;
            foreach (var item in dropItemList)
            {
                //item.Item1 : 드롭테이블 인덱스
                //item.Item2 : 확률
                accChance += item.Item2;

                if (rand <= accChance)
                {
                    //드롭테이블 참조해서 돌맹이 생성
                    //인덱스로 드롭테이블 참조
                    List<(int, int)> dropDataList = DataManager.Instance.GetIndexData<DropTableData, DropTableDataParsingInfo>(item.Item1).stoneDropDataList;
                    foreach (var dropData in dropDataList)
                    {
                        for (int i = 0; i < dropData.Item2; i++)
                        {
                            //TODO : 돌맹이 데이터테이블에서 해당하는 티어의 돌맹이 랜덤하게 뽑아내기
                            GameObject obj = Instantiate(stonePrefabTest, stoneSpawnPos);
                            obj.GetComponent<Rigidbody>().AddForce(GetRandVector() * 4f, ForceMode.Impulse);
                            //Debug.Log("Stone Tier : " + dropData.Item1.ToString());
                        }
                    }
                    break;
                }
            }
        }

        private Vector3 GetRandVector()
        {
            Vector3 vec = new Vector3(Random.Range(-1.0f, 1.0f), 0.5f,0);
            return vec.normalized;
        }
        private IEnumerator Regenerate()
        {
            oreBody.gameObject.SetActive(false);
            yield return new WaitForSeconds(regenerationTime);
            canMine = true;
            oreBody.gameObject.SetActive(true);
            hp = maxHp;
        }

        public void Interact(GameObject obj)
        {
            obj.GetComponent<PlayerController>().SetCurOre(this);
        }
    }
}