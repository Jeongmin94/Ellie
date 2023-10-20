using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Monsters.Characters
{

    public class TestPlayer : MonoBehaviour
    {
        private float forward;
        private float side;
        static private Vector3 playerPosition;

        public void Damaged(float attackValue)
        {
            Debug.Log(gameObject.ToString() + " Damaged :" + attackValue);
        }
        private void FixedUpdate()
        {
            side = Input.GetAxis("Horizontal");
            forward = Input.GetAxis("Vertical");

            transform.Translate(new Vector3(side, 0, forward) * 10 * Time.deltaTime);

            playerPosition = transform.position;
        }
        static public Vector3 GetPlayerPosition()
        {
            return playerPosition;
        }
    }

}