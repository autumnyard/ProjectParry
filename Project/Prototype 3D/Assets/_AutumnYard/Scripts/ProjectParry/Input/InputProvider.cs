using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutumnYard.ProjectParry
{
    public class InputProvider : MonoBehaviour, IInputProvider
    {
        [SerializeField] private InputMap map;

        public void GetInputs(ref PlayerInputs inputs)
        {
            inputs.horizontalAxis = Input.GetAxis("Horizontal");
            inputs.verticalAxis = Input.GetAxis("Vertical");
            inputs.attackPressed = Input.GetKeyDown(map.attack);
            inputs.attackMaintain = Input.GetKey(map.attack);
            inputs.defensePressed = Input.GetKeyDown(map.defense);
            inputs.defenseMaintain = Input.GetKey(map.defense);
        }
    }
}
