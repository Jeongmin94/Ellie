using Assets.Scripts.Managers;
using Data.GoogleSheet;
using UnityEngine;

namespace Equipments
{
    public class Pickaxe : MonoBehaviour
    {
        public enum Tier
        {
            Tier5 = 9000,
            Tier4,
            Tier3,
            Tier2,
            Tier1
        }

        public PickaxeData data;
        public Material[] pickaxeMaterials;
        public GameObject[] smithingEffects;
        public Transform effectPos;
        public Tier tier;
        [SerializeField] private int durability;

        private readonly string[] pickaxeSounds = new string[3];
        private int soundIdx;

        public int Durability
        {
            get => durability;
            set => durability = value;
        }

        public int MinSmithPower => data.minSmithPower;
        public int MaxSmithPower => data.maxSmithPower;

        private void Start()
        {
            pickaxeMaterials = Resources.LoadAll<Material>("Materials/PickaxeMaterials");
            //일단 이펙트 다 꺼주기
            foreach (var obj in smithingEffects)
            {
                obj.SetActive(false);
                obj.GetComponent<ParticleSystem>().Stop();
            }

            for (var i = 0; i < pickaxeSounds.Length; i++)
            {
                var soundName = "pickaxe_sound";
                soundName += (i + 1).ToString();
                pickaxeSounds[i] = soundName;
            }
        }

        private void Update()
        {
            //테스트
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                LoadPickaxeData(Tier.Tier5);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                LoadPickaxeData(Tier.Tier4);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                LoadPickaxeData(Tier.Tier3);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                LoadPickaxeData(Tier.Tier2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                LoadPickaxeData(Tier.Tier1);
            }
        }

        public void LoadPickaxeData(Tier tier)
        {
            this.tier = tier;
            data = DataManager.Instance.GetIndexData<PickaxeData, PickaxeDataParsingInfo>((int)tier);
            durability = data.durability;
            var materialIdx = (int)tier % pickaxeMaterials.Length;
            GetComponent<Renderer>().material = pickaxeMaterials[materialIdx];
        }

        public void PrintSmithingEffect()
        {
            var idx = Random.Range(0, smithingEffects.Length);
            var effect = smithingEffects[idx];
            effect.SetActive(true);
            effect.transform.position = effectPos.position;
            effect.GetComponent<ParticleSystem>().Play();

            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, pickaxeSounds[soundIdx], transform.position);
            soundIdx = (soundIdx + 1) % pickaxeSounds.Length;
        }
    }
}