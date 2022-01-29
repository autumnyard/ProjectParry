using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutumnYard.ProjectParry
{
    public sealed class PlayerAttack : MonoBehaviour, IInputReceiver
    {
        [SerializeField] private GameObject attack;
        [SerializeField] private GameObject parry;
        [SerializeField] private GameObject defense;
        [SerializeField] private float attackSpan = .2f; // TODO: El tiempo que aguanta el parry
        [SerializeField] private float parrySpan = .4f; // TODO: El tiempo que aguanta el parry
        private State _state;

        public enum State { Blocked, Normal, Attack, Parry, Defense, Hurt }

        private void OnEnable()
        {
            _state = State.Normal;
        }

        public void UpdateWithInputs(in PlayerInputs inputs)
        {
            if (inputs.attack)
            {
                _state = State.Attack;
            }
            else if (inputs.defensePressed)
            {
                _state = State.Parry;
            }
            else if (inputs.defenseMaintain)
            {
                _state = State.Defense;
            }
            else
            {
                _state = State.Normal;
            }

            HandleState(in _state);
        }

        private void HandleState(in State state)
        {
            attack.SetActive(state == State.Attack);
            parry.SetActive(state == State.Parry);
            defense.SetActive(state == State.Defense);
        }

        public void EndFrame()
        {

        }


        public void Stop() { }

    }
}
