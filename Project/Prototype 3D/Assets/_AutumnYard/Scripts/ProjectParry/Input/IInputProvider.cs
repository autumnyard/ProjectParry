using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutumnYard.ProjectParry
{
    public interface IInputProvider
    {
        void GetInputs(ref PlayerInputs inputs);
    }
}
