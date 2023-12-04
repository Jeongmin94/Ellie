using Assets.Scripts.Player;
using Channels.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects
{
    public class MineCart : MonoBehaviour, IInteractiveObject
    {
        [SerializeField] private Transform playerStandingPos;
        [SerializeField] private BezierSpline spline;
        [SerializeField] private BezierSpline successSpline;
        [SerializeField] private GameObject player;
        [SerializeField] private Transform playerEndPos;
        [SerializeField] private bool isActivated;
        [SerializeField] private float duration;
        private SplineWalker walker = null;

        private bool canJump = false;

        InteractiveType interactiveType = InteractiveType.Chatting;
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
            if(canJump && Input.GetKeyDown(KeyCode.Space))
            {
                JumpToSuccessRail();
            }
        }
        //TODO : 특정 구간에서 Space를 입력해서 구간 넘어가기
        
        public void Interact(GameObject obj)
        {
            if (!obj.CompareTag("Player")) return;
            player = obj;
            isActivated = true;
            StartRailSystem();
            player.GetComponent<PlayerInteraction>().DeactivateInteractiveUI();
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
            player.GetComponent<PlayerInteraction>().interactiveObject = null;
            player.GetComponent<PlayerInteraction>().SetCanInteract(false);
            Destroy(walker);
            gameObject.tag = "Untagged";
        }
        private void LockPlayerPos()
        {
            player.transform.position = playerStandingPos.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.name == "SuccessInterval")
            {
                Debug.Log("enter interval");
                canJump = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == "SuccessInterval")
            {
                Debug.Log("exit interval");
                canJump = false;
            }
        }
        

        private void JumpToSuccessRail()
        {
            Destroy(walker);
            walker = gameObject.AddComponent<SplineWalker>();
            walker.duration = duration;
            walker.spline = successSpline;
            walker.lookForward = true;
            walker.mode = SplineWalkerMode.Once;
            canJump = false;

        }

        public InteractiveType GetInteractiveType()
        {
            return interactiveType;
        }
    }
}