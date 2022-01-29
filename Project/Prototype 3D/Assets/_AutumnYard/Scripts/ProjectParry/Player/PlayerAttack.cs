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
        private bool _isAttacking;
        private bool _isDefending;
        private bool _isParrying;


        public void UpdateWithInputs(in PlayerInputs inputs)
        {
            if (inputs.attack)
            {
                _isAttacking = true;
                _isParrying = false;
                _isDefending = false;
            }
            else if (inputs.defensePressed)
            {
                _isAttacking = false;
                _isParrying = true;
                _isDefending = false;
            }
            else if (inputs.defenseReleased)
            {
                _isAttacking = false;
                _isParrying = false;
                _isDefending = false;
            }
            else if (inputs.defenseMaintain)
            {
                _isAttacking = false;
                _isParrying = false;
                _isDefending = true;
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
        }

        public void EndFrame()
        {

        }


        public void Stop() { }

    }
}
