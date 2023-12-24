using System.Collections.Generic;

namespace Player
{
    public class PlayerStateMachine
    {
        private readonly Dictionary<PlayerStateName, PlayerBaseState> states = new();

        public PlayerStateMachine(PlayerStateName stateName, PlayerBaseState state)
        {
            AddState(stateName, state);
            CurrentState = GetState(stateName);
            CurrentStateName = stateName;
        }

        public PlayerBaseState CurrentState { get; private set; }
        public PlayerStateName CurrentStateName { get; private set; }

        public void AddState(PlayerStateName stateName, PlayerBaseState state)
        {
            if (!states.ContainsKey(stateName))
            {
                states.Add(stateName, state);
            }
        }

        public PlayerBaseState GetState(PlayerStateName stateName)
        {
            if (states.TryGetValue(stateName, out var state))
            {
                return state;
            }

            return null;
        }

        public void DeleteState(PlayerStateName stateName)
        {
            if (states.ContainsKey(stateName))
            {
                states.Remove(stateName);
            }
        }

        public void ChangeState(PlayerStateName nextStateName)
        {
            CurrentState?.OnExitState();
            if (states.TryGetValue(nextStateName, out var newState))
            {
                CurrentState = newState;
                CurrentStateName = nextStateName;
            }

            CurrentState?.OnEnterState();
        }

        public void ChangeState(PlayerStateName nextStateName, StateInfo info)
        {
            CurrentState?.OnExitState();
            if (states.TryGetValue(nextStateName, out var newState))
            {
                CurrentState = newState;
                CurrentStateName = nextStateName;
            }

            CurrentState?.OnEnterState(info);
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