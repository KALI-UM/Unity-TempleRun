using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
   
    public float Speed
    {
        get
        {
            return forwardSpeed * (input.Slide == 0 ? 1f : slidingSpeedRate * input.Slide);
        }
    }

    public float forwardSpeed = 10f;
    public float slidingSpeedRate = 0.8f;
    public float rotateSpeed = 5f;
    public float jumpPower = 5f;

    private Rigidbody rb;
    private Animator animator;
    private PlayerInput input;

    public readonly int hashJump = Animator.StringToHash("Jump");
    public readonly int hashSlide = Animator.StringToHash("Slide");
    public readonly int hashGrounded = Animator.StringToHash("IsGrounded");

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        input = GetComponent<PlayerInput>();
    }

    private void LateUpdate()
    {
        Move();
        Rotate();
        Jump();
        Slide();

        animator.SetBool(hashGrounded, IsGrounded);
    }

    void FixedUpdate()
    {
    }

    private void Move()
    {
        //var nextpos = transform.position + transform.forward * input.Move * forwardSpeed * Time.deltaTime;
        //rb.MovePosition(nextpos);
    }

    private void Rotate()
    {
        var nextrot = rb.rotation * Quaternion.Euler(transform.up * input.Rotate * 90f);
        rb.MoveRotation(nextrot);
    }

    private void Jump()
    {
        if (input.Jump && IsGrounded)
        {
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            animator.SetTrigger(hashJump);
        }
    }

    private void Slide()
    {
        animator.SetFloat(hashSlide, input.Slide);
    }

    private void OnCollisionStay(Collision collision)
    {
        IsGrounded = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        IsGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        IsGrounded = false;
    }


}
