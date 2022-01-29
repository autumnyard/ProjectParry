using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Normal Movements Variables
    private float walkSpeed;
    private float curSpeed;
    private float maxSpeed;

    [SerializeField] private float speed = 7f;
    [SerializeField] private float agility = 7f;
    private float sprintSpeed;
    private Rigidbody rigidbody;

    void Start()
    {
        walkSpeed = (float)(agility / 5);
        sprintSpeed = walkSpeed + (walkSpeed / 2);
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        curSpeed = walkSpeed;
        maxSpeed = curSpeed;

        // Move senteces
        rigidbody.velocity = new Vector3(
            Mathf.Lerp(0, Input.GetAxis("Horizontal") * curSpeed, 0.8f),
            0f,
            Mathf.Lerp(0, Input.GetAxis("Vertical") * curSpeed, 0.8f));
    }
}
