using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private FloatingJoystick joystick;

    [SerializeField] private Animator animator;

    [SerializeField] private float playerSpeed;

    private float rotationSpeed = 5f;
    private float gravity = -9.8f;
    private Vector3 velocity;
    private bool isGrounded;

    [SerializeField] private Transform groundCheck;
    
    private float groundDistance = 0.4f;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpHeight = 3f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jump;
    void Update()
    {
        Movement();
    }
    private void Movement()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        Vector3 move = new Vector3(-joystick.Horizontal, 0, -joystick.Vertical).normalized;
        if (move.magnitude >= 0.1f)
        {
            controller.Move(move.normalized * playerSpeed * Time.deltaTime);
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(move);
            toRotation = Quaternion.Euler(0, toRotation.eulerAngles.y, 0);
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void Jump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Debug.Log(isGrounded);
        if (isGrounded)
        {
            animator.CrossFade("Jump", 0.17f );

            audioSource.PlayOneShot(jump, 0.7f);
            velocity.y = Mathf.Sqrt(jumpHeight * -4f * gravity);
        }

    }
   

}
