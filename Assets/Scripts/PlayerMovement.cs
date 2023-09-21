using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jump = 1f;
    public float sprintModifier;
    public Camera fpsCam;

    private float baseFOV;
    private float sprintFOVModifier = 1.10f;

    public Transform weaponParent;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    //idle headbob
    private Vector3 targetWeaponBobPosition;
    private Vector3 weaponParentOrigin;
    private float movementCounter;
    private float idleCounter;


    Vector3 velocity;
    bool isGrounded;
    private bool isSpinting;

    private void Start()
    {
        baseFOV = fpsCam.fieldOfView;
        weaponParentOrigin = weaponParent.localPosition;    
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isSprinting = sprint && z >0;

        float t_adjustSpeed = speed;
        if (isSprinting) {
            t_adjustSpeed *= sprintModifier;
        }


        Vector3 move = transform.right * x + transform.forward * z;
        //
        if (move.magnitude > 1)
            move /= move.magnitude;

        controller.Move(move * t_adjustSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (isSprinting)
        {
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f);
        }
        else { fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, baseFOV, Time.deltaTime * 8f); }


            //headbobbing
            if (x == 0 && z == 0)
        {
            HeadBob(idleCounter, 0.025f, 0.025f);
            idleCounter += Time.deltaTime;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);
        }
            else if (!isSprinting)
        {
            HeadBob(movementCounter, 0.035f, 0.035f);
            movementCounter += Time.deltaTime * 3f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 6f);
        }
            else{
            HeadBob(movementCounter, 0.03f, 0.045f);
            movementCounter += Time.deltaTime *4.5f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);
        }
    }

    void HeadBob (float p_z, float p_x_intensity, float p_y_intensity)
    {
        targetWeaponBobPosition = weaponParentOrigin + new Vector3(Mathf.Cos(p_z) * p_x_intensity, Mathf.Sin(p_z*2) * p_y_intensity, 0);
    }
}
