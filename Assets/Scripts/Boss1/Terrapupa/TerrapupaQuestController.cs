using Boss1.DataScript.Terrapupa;
using Channels.Boss;
using Channels.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Boss1.Terrapupa
{
    public class TerrapupaQuestController : SerializedMonoBehaviour
    {
        private TerrapupaRootData terrapupaData;
        private TicketMachine ticketMachine;
        
        private readonly int stoneHitCompareCount = 5;
        private readonly int stoneHitCompareCountFaint = 3;
        
        private bool isFirstReachCompareCount;
        private bool isFirstReachCompareCountFaint;
        private int stoneHitCount;
        private int stoneHitCountFaint;

        public void InitData(TerrapupaRootData data)
        {
            terrapupaData = data;
        }
        
        public void InitTicketMachine(TicketMachine ticketMachine)
        {
            this.ticketMachine = ticketMachine;
        }

        public void CheckWeakPoint()
        {
            if (terrapupaData.isStart.Value && !isFirstReachCompareCountFaint)
            {
                stoneHitCountFaint++;
                if (stoneHitCountFaint >= stoneHitCompareCountFaint)
                {
                    isFirstReachCompareCountFaint = true;
                    TerrapupaDialogChannel.SendMessage(TerrapupaDialogTriggerType.DontAttackBossWeakPoint,
                        ticketMachine);
                }
            }
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (terrapupaData.isStart.Value &&
                !isFirstReachCompareCount && collision.gameObject.CompareTag("Stone"))
            {
                stoneHitCount++;
                if (stoneHitCount >= stoneHitCompareCount)
                {
                    isFirstReachCompareCount = true;
                    TerrapupaDialogChannel.SendMessage(
                        TerrapupaDialogTriggerType.AttackBossWithNormalStone,
                        ticketMachine);
                }
            }
        }
    }
}
