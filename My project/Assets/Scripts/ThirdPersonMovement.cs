using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float normalSpeed = 6f;
    public float boostedSpeed = 9f;
    public float turnSmoothTime = 0.1f;

    private float currentSpeed;
    private float turnSmoothVelocity;
    private Animator playerAnim;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool isBoosted = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        currentSpeed = isBoosted ? boostedSpeed : normalSpeed;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

            if (isBoosted)
            {
                StartSprintAnim();
            }
            else
            {
                StartRunAnim();
            }
        }
        else
        {
            controller.Move(Vector3.zero);
            StartIdleAnim();
        }
    }

    private void StartRunAnim()
    {
        playerAnim.SetBool("IdleAnim", false);
        playerAnim.SetBool("RunAnim", true);
        playerAnim.SetBool("SprintAnim", false);
    }

    private void StartSprintAnim()
    {
        playerAnim.SetBool("IdleAnim", false);
        playerAnim.SetBool("RunAnim", false);
        playerAnim.SetBool("SprintAnim", true);
    }

    private void StartIdleAnim()
    {
        playerAnim.SetBool("IdleAnim", true);
        playerAnim.SetBool("RunAnim", false);
        playerAnim.SetBool("SprintAnim", false);
    }
}