using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutumnYard.ProjectParry
{
    public sealed class PlayerActor : MonoBehaviour, IPlayer
    {
        [SerializeField, Range(1, 2)] private int playerNumber = 1;
        [SerializeField] private PlayerConfiguration configuration;
        private IInputProvider _inputProvider;
        private PlayerMovement _movement;
        private PlayerAttack _attack;
        private PlayerInputs _inputs;

        private void Awake()
        {
            if (_inputProvider == null)
            {
                _inputProvider = GetComponent<IInputProvider>();
            }
            if (_inputProvider == null)
            {
                _inputProvider = new InputProvider_placeholder();
            }

            _movement = new PlayerMovement(configuration, GetComponent<Rigidbody>());
            _attack = GetComponent<PlayerAttack>();
            _inputs = new PlayerInputs();
        }

        private void Update()
        {
            _inputProvider.GetInputs(ref _inputs);
            _movement.UpdateWithInputs(in _inputs);
            _attack.UpdateWithInputs(in _inputs);
        }

        public void Spawn(Vector3 where)
        {
            transform.position = where;
        }
    }
}
