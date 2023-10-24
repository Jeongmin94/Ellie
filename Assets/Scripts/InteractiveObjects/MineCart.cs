﻿using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects
{
    public class MineCart : MonoBehaviour, IInteractiveObject
    {
        [SerializeField] private Transform playerStandingPos;
        [SerializeField] private BezierSpline spline;
        [SerializeField] private GameObject player;
        [SerializeField] private Transform playerEndPos;
        [SerializeField] private bool isActivated;
        [SerializeField] private float duration;
        private SplineWalker walker = null;
        // Use this for initialization
        private void Start()
        {
            isActivated = false;
        }

        // Update is called once per frame
        private void Update()
        {
            if (isActivated)
            {
                LockPlayerPos();
            }
            if (walker && isActivated && walker.Progress == 1f)
            {
                EndRailSystem();
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
            player.GetComponent<PlayerController>().canJump = false;
            walker = gameObject.AddComponent<SplineWalker>();
            walker.duration = duration;
            walker.spline = spline;
            walker.lookForward = true;
            walker.mode = SplineWalkerMode.Once;
            isActivated = true;
        }
        private void EndRailSystem()
        {
            isActivated = false;
            player.transform.position = playerEndPos.position;
            player.GetComponent<PlayerController>().canJump = true;
            Destroy(walker);

        }
        private void LockPlayerPos()
        {
            player.transform.position = playerStandingPos.position;
        }
    }
}