using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Puzzle
{
    public class StonePillarPuzzle : MonoBehaviour
    {
        public Vector3 center;
        public Vector3 size;
        public int count = 0;

        public GameObject pillar1;
        public GameObject pillar2;
        public GameObject pillar3;

        public float pillar1Height;
        public float pillar2Height;
        public float pillar3Height;

        public float waitTime;

        private bool isDone;
        public float raisingSpeed;
        private void Start()
        {
            center = transform.position;
            size = transform.localScale;
            isDone = false;
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
                StartCoroutine(RaisePillar(pillar1, pillar1Height));
                StartCoroutine(RaisePillar(pillar2, pillar2Height));
                StartCoroutine(RaisePillar(pillar3, pillar3Height));
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