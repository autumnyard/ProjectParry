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
        [SerializeField] private GameObject attack;
        [SerializeField] private GameObject parry;
        [SerializeField] private GameObject defense;
        [SerializeField] private float attackSpan = .2f; // TODO: El tiempo que aguanta el parry
        [SerializeField] private float parrySpan = .4f; // TODO: El tiempo que aguanta el parry
        private State _state;
        private Counter attackCounter;
        private Counter parryCounter;
        private bool _isAttacking;
        private bool _isParrying;
        private bool _isDefending;

        public enum State { Blocked, Normal, Attack, Parry, Defense, Hurt }

        private void Awake()
        {
            attackCounter = new Counter(attackSpan);
            parryCounter = new Counter(parrySpan);
        }

        private void OnEnable()
        {
            _state = State.Normal;
        }

        public void UpdateWithInputs(in PlayerInputs inputs)
        {
            if (inputs.attackPressed)
            {
                parryCounter.Reset();
                attackCounter.Reset();
                _state = State.Attack;
            }
            else if(inputs.attackMaintain)
            {
                _state = State.Attack;
            }
            else if (inputs.defensePressed)
            {
                attackCounter.Reset();
                parryCounter.Reset();
                _state = State.Defense;
            }
            else if (inputs.defenseMaintain)
            {
                _state = State.Defense;
            }
            else
            {
                attackCounter.Reset();
                parryCounter.Reset();
                _state = State.Normal;
            }

            if (_state == State.Attack)
            {
                if (attackCounter.Tick(Time.deltaTime))
                {
                    _isAttacking = false;
                    _isParrying = false;
                    _isDefending = false;
                }
                else
                {
                    _isAttacking = true;
                    _isParrying = false;
                    _isDefending = false;
                }
            }
            else if (_state == State.Defense)
            {
                if (parryCounter.Tick(Time.deltaTime))
                {
                    _isAttacking = false;
                    _isParrying = false;
                    _isDefending = true;
                }
                else
                {
                    _isAttacking = false;
                    _isParrying = true;
                    _isDefending = false;
                }
            }
            else
            {
                _isAttacking = false;
                _isParrying = false;
                _isDefending = false;
            }

            attack.SetActive(_isAttacking);
            parry.SetActive(_isParrying);
            defense.SetActive(_isDefending);
            //parry.SetActive(state == State.Parry);
            //defense.SetActive(state == State.Defense);
        }


        public void EndFrame()
        {

        }


        public void Stop() { }

    }
}
