using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutumnYard.ProjectParry
{
    public class Counter
    {
        private readonly float maxTime;
        private float currentTime;

        public Counter(float time)
        {
            maxTime = time;
            currentTime = 0;
        }

        public void Reset()
        {
            currentTime = maxTime;
        }

        public bool Tick(float elapsed)
        {
            currentTime -= elapsed;

            return Check();
        }

        public bool Check()
        {
            return currentTime <= 0;
        }
    }

    public sealed class PlayerAttack : MonoBehaviour, IInputReceiver
    {
        public enum State { Blocked, Normal, Attack, Parry, Defense, Hurt }
        [SerializeField] private GameObject attack;
        [SerializeField] private GameObject parry;
        [SerializeField] private GameObject defense;
        [SerializeField] private float attackSpan = .2f; // TODO: El tiempo que aguanta el parry
        [SerializeField] private float parrySpan = .4f; // TODO: El tiempo que aguanta el parry
        private Counter _attackCounter;
        private Counter _parryCounter;
        private State _state;
        private State _newState;


        private void Awake()
        {
            _attackCounter = new Counter(attackSpan);
            _parryCounter = new Counter(parrySpan);
        }

        private void OnEnable()
        {
            _state = State.Normal;
            _newState = State.Normal;
        }

        public void UpdateWithInputs(in PlayerInputs inputs)
        {
            if (inputs.attackPressed)
            {
                _parryCounter.Reset();
                _attackCounter.Reset();
                _state = State.Attack;
            }
            else if (inputs.attackMaintain)
            {
                _state = State.Attack;
            }
            else if (inputs.defensePressed)
            {
                _attackCounter.Reset();
                _parryCounter.Reset();
                _state = State.Defense;
            }
            else if (inputs.defenseMaintain)
            {
                _state = State.Defense;
            }
            else
            {
                _attackCounter.Reset();
                _parryCounter.Reset();
                _state = State.Normal;
            }

            switch (_state)
            {
                case State.Attack:
                    _newState = _attackCounter.Tick(Time.deltaTime) ? State.Normal : State.Attack;
                    break;

                case State.Defense:
                    _newState = _parryCounter.Tick(Time.deltaTime) ? State.Defense : State.Parry;
                    break;

                default:
                    _newState = State.Normal;
                    break;
            }

            attack.SetActive(_newState == State.Attack);
            parry.SetActive(_newState == State.Parry);
            defense.SetActive(_newState == State.Defense);
        }


        public void Stop() { }

    }
}
