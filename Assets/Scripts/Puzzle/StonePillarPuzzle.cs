using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Puzzle
{
    public class StonePillarPuzzle : MonoBehaviour
    {
        public Vector3 center;
        public Vector3 size;
        public int count = 0;

        //public GameObject pillar1;
        //public GameObject pillar2;
        //public GameObject pillar3;

        public GameObject[] pillars;

        public List<float> pillarsInitialHeight = new();
        [SerializeField] private float constHeight = -9.0f;

        public float waitTime;

        private bool isDone;
        public float raisingSpeed;
        private void Start()
        {
            center = transform.position;
            size = transform.localScale;
            isDone = false;

            foreach (var pillar in pillars)
            {
                pillarsInitialHeight.Add(pillar.transform.position.y);
                Vector3 temp = pillar.transform.position;
                temp.y = constHeight;
                pillar.transform.position = temp;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Stone"))
                CheckStones();
        }

        private void CheckStones()
        {
            count = 0;
            Collider[] colliders = Physics.OverlapBox(center, size);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Stone"))
                {
                    count++;
                }
            }
            if (count >= 3 && !isDone)
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "puzzle1_stone1", transform.position);
                for(int i = 0; i<pillars.Length;i++)
                {
                    StartCoroutine(RaisePillar(pillars[i], pillarsInitialHeight[i]));
                }
                isDone = true;
            }
        }

        private IEnumerator RaisePillar(GameObject obj, float height)
        {
            float time = 0;
            while (time <= waitTime)
            {
                time += Time.deltaTime;
                yield return null;
            }
            SoundManager.Instance.PlaySound(SoundManager.SoundType.UISfx, "puzzle1_stone2");

            Vector3 raisePos = obj.transform.position;
            while (obj.transform.position.y < height)
            {
                raisePos.y += raisingSpeed * Time.deltaTime;
                obj.transform.position = raisePos;
                yield return null;
            }
            raisePos.y = height;
            obj.transform.position = raisePos;
        }
    }
}