using UnityEngine;

namespace AutumnYard.ProjectParry
{
    public sealed class PlayerMovement : IInputReceiver
    {
        private readonly PlayerConfiguration _configuration;
        private readonly Rigidbody _rb;

        public PlayerMovement(PlayerConfiguration configuration, Rigidbody rb)
        {
            _configuration = configuration;
            _rb = rb;
        }

        public void UpdateWithInputs(in PlayerInputs inputs)
        {
            var asd = new Vector3(
                Mathf.Lerp(0, inputs.horizontalAxis * _configuration.speed, 0.8f),
                0f,
                Mathf.Lerp(0, inputs.verticalAxis * _configuration.speed, 0.8f));

            var qwe = asd.normalized * Mathf.Clamp(asd.magnitude, 0, _configuration.maxSpeed);

            //Debug.Log($"Movement vec: {asd} --> {qwe}  ({qwe.magnitude})");

            _rb.velocity = asd;
            //_rb.AddForce(asd, ForceMode.Acceleration);
            //_rb.AddForce(asd, ForceMode.Force);
            //_rb.AddForce(asd, ForceMode.Impulse);
            //_rb.AddForce(asd, ForceMode.VelocityChange);
        }

        public void Stop()
        {
            _rb.velocity = Vector3.zero;
        }
    }
}