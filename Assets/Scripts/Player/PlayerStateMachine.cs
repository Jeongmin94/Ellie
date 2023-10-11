using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets.Scripts.Player
{
    public class PlayerStateMachine
    {
        public PlayerBaseState CurrentState { get; private set; }
        private Dictionary<PlayerStateName, PlayerBaseState> states = new();

        public PlayerStateMachine(PlayerStateName stateName, PlayerBaseState state)
        {
            AddState(stateName, state);
            CurrentState = GetState(stateName);
        }

        public void AddState(PlayerStateName stateName, PlayerBaseState state)
        {
            if (!states.ContainsKey(stateName))
                states.Add(stateName, state);
        }

        public PlayerBaseState GetState(PlayerStateName stateName)
        {
            if (states.TryGetValue(stateName, out PlayerBaseState state))
                return state;
            return null;
        }

        public void DeleteState(PlayerStateName stateName)
        {
            if (states.ContainsKey(stateName))
                states.Remove(stateName);
        }

        public void ChangeState(PlayerStateName nextStateName)
        {
            CurrentState?.OnExitState();
            if (states.TryGetValue(nextStateName, out PlayerBaseState newState))
                CurrentState = newState;
            CurrentState?.OnEnterState();
        }

        public void UpdateState()
        {
            CurrentState?.OnUpdateState();
        }
        public void FixedUpdateState()
        {
            CurrentState?.OnFixedUpdateState();
        }
    }
}
