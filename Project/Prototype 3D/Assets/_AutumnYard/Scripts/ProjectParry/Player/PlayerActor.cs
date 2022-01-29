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
        private PlayerAttack _attack;
        private PlayerInputs _inputs;

        private void Awake()
        {
            _movement = new PlayerMovement(configuration, GetComponent<Rigidbody>());
            _attack = GetComponent<PlayerAttack>();
            _inputs = new PlayerInputs();
        }

        private void Update()
        {
            if (playerNumber == 1)
            {
                _inputs.horizontalAxis = Input.GetAxis("Horizontal");
                _inputs.verticalAxis = Input.GetAxis("Vertical");
                _inputs.attack = Input.GetKeyDown(KeyCode.Z);
                _inputs.defensePressed = Input.GetKeyDown(KeyCode.X);
                _inputs.defenseMaintain = Input.GetKey(KeyCode.X);
                _inputs.defenseReleased = Input.GetKeyUp(KeyCode.X);
            }
            else
            {

            }

            _movement.UpdateWithInputs(in _inputs);
            _attack.UpdateWithInputs(in _inputs);
        }

        public void Spawn(Vector3 where)
        {
            transform.position = where;
        }
    }
}
