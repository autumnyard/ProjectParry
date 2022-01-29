using UnityEngine;

namespace AutumnYard.ProjectParry
{
    [CreateAssetMenu(fileName = "Input Map", menuName = "Autumn Yard/Create Input Map...")]
    public sealed class InputMap : ScriptableObject
    {
        public KeyCode attack;
        public KeyCode defense;
    }
}
