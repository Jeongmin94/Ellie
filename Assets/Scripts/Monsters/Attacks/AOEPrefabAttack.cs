using System.Collections;
using Assets.Scripts.Monsters.AbstractClass;
using Assets.Scripts.Monsters.Others;
using UnityEngine;

namespace Assets.Scripts.Monsters.Attacks
{

    public class AOEPrefabAttack : AbstractAttack
    {
        private AOE prefabObject;
        private Vector3 position;
        private Vector3 offset;
        private float damageInterval;

        public override void InitializeAOE(AOEAttackData data)
        {
            InitializedBase(data.attackValue, data.durationTime, data.attackInterval, data.attackableDistance);
            Debug.Log("ParameterOBJ : " + prefabObject);
            damageInterval = data.damageInterval;
            prefabObject = data.prefabObject.GetComponent<AOE>();
            if (prefabObject == null)
                Debug.Log("[AOEPrefabAttack] PrefabObject is Null");
        }

        public override void ActivateAttack()
        {
            AOE obj = Instantiate(prefabObject, position + offset, transform.rotation);
            obj.SetPrefabData(attackValue, durationTime, damageInterval, gameObject.tag.ToString());
            StartCoroutine(StartAttackReadyCount());
        }
        private IEnumerator StartAttackReadyCount()
        {
            IsAttackReady = false;
            yield return new WaitForSeconds(AttackInterval);
            IsAttackReady = true;
        }

        public void SetPrefabPosition(Vector3 position, Vector3 offset)
        {
            this.position = position;
            this.offset = offset;
        }
    }

}