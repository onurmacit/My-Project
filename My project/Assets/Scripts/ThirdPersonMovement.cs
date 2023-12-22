using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;

    float turnSmoothVelocity;
    Animator playerAnim;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            StartRunAnim();
        }
        else
        {
            controller.Move(Vector3.zero); // Hareketi durdur
            StartIdleAnim();
        }
    }

    private void StartRunAnim()
    {
        playerAnim.SetBool("IdleAnim", false);
        playerAnim.SetBool("RunAnim", true);
    }

    private void StartIdleAnim()
    {
        playerAnim.SetBool("IdleAnim", true);
        playerAnim.SetBool("RunAnim", false);
    }
}
