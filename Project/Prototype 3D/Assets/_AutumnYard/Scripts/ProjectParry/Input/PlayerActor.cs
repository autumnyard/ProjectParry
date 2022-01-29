using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutumnYard.ProjectParry
{
    public sealed class PlayerActor : MonoBehaviour, IPlayer
    {
        [SerializeField, Range(1, 2)] private int playerNumber = 1;
        [SerializeField] private PlayerConfiguration configuration;
        private PlayerMovement _movement;
        private PlayerInputs _inputs;

        private void Awake()
        {
            _movement = new PlayerMovement(configuration, GetComponent<Rigidbody>());
            _inputs = new PlayerInputs();
        }

        private void Update()
        {
            if (playerNumber == 2) return;

            _inputs.horizontalAxis = Input.GetAxis("Horizontal");
            _inputs.verticalAxis = Input.GetAxis("Vertical");
            _inputs.attack = Input.GetKeyDown(KeyCode.Z);
            _inputs.defense = Input.GetKeyDown(KeyCode.X);

            _movement.Update(in _inputs);
        }

        public void Spawn(Vector3 where)
        {
            transform.position = where;
        }
    }
}
