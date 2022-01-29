using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutumnYard.ProjectParry
{
    public class InputProvider_placeholder : IInputProvider
    {
        public void GetInputs(ref PlayerInputs inputs)
        {
            inputs.horizontalAxis = Input.GetAxis("Horizontal");
            inputs.verticalAxis = Input.GetAxis("Vertical");
            inputs.attack = Input.GetKeyDown(KeyCode.Z);
            inputs.defensePressed = Input.GetKeyDown(KeyCode.X);
            inputs.defenseMaintain = Input.GetKey(KeyCode.X);
            inputs.defenseReleased = Input.GetKeyUp(KeyCode.X);
        }
    }
}
