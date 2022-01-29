using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutumnYard.ProjectParry
{
    [CreateAssetMenu(fileName = "Player Configuration", menuName = "Autumn Yard/Create Player Configuration...")]
    public sealed class PlayerConfiguration : ScriptableObject
    {
        public float speed = 7f;
        public float maxSpeed = 15f;
    }
}