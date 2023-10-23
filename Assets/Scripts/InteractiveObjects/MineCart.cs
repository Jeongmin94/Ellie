using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects
{
    public class MineCart : MonoBehaviour, IInteractiveObject
    {
        [SerializeField] private Transform playerStandingPos;
        [SerializeField] private BezierSpline spline;
        [SerializeField] private GameObject player;
        [SerializeField] private bool isActivated;
        [SerializeField] private float duration;
        // Use this for initialization
        private void Start()
        {
            isActivated = false;
        }

        // Update is called once per frame
        private void Update()
        {
            
            if(Input.GetKeyDown(KeyCode.O))
            {
                StartRailSystem();
                isActivated = true;
            }
            if (isActivated)
            {
                LockPlayerPos();
            }
        }
        //TODO : 특정 구간에서 Space를 입력해서 구간 넘어가기

        public void Interact(GameObject obj)
        {
            if (!obj.CompareTag("Player")) return;
            player = obj;
            isActivated = true;
            StartRailSystem();
        }
        private void StartRailSystem()
        {
            player.GetComponent<PlayerController>().PlayerObj.transform.rotation = playerStandingPos.rotation;
            SplineWalker walker = gameObject.AddComponent<SplineWalker>();
            walker.duration = duration;
            walker.spline = spline;
            walker.lookForward = true;
            walker.mode = SplineWalkerMode.Once;
            isActivated = true;
        }
        private void LockPlayerPos()
        {
            player.transform.position = playerStandingPos.position;
        }
    }
}