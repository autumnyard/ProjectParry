using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutumnYard.ProjectParry
{
    public sealed class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerConfiguration configuration;
        private Rigidbody _rb;
        private float _sprintSpeed;
        private float _walkSpeed;
        private float _curSpeed;
        private float _maxSpeed;

        void Start()
        {
            _walkSpeed = (float)(configuration.agility / 5);
            _sprintSpeed = _walkSpeed + (_walkSpeed / 2);
            _rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            _curSpeed = _walkSpeed;
            _maxSpeed = _curSpeed;

            // Move senteces
            var asd = new Vector3(
                Mathf.Lerp(0, Input.GetAxis("Horizontal") * _curSpeed, 0.8f),
                0f,
                Mathf.Lerp(0, Input.GetAxis("Vertical") * _curSpeed, 0.8f));

            //_rb.velocity = asd;
            _rb.AddForce(asd, ForceMode.VelocityChange);
        }

        public void Spawn(Vector3 position)
        {
            transform.position = position;
        }
    }
}