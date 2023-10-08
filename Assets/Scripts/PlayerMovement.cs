using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jump = 1f;
    public float sprintModifier;
    public float aimingModifier;
    public Camera fpsCam;

    private float baseFOV;
    private float sprintFOVModifier = 1.10f;

    public Transform weaponParent;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    //sprinting
    private bool sprint;
    private bool isSprinting;

    //idle headbob
    private Vector3 targetWeaponBobPosition;
    public Vector3 weaponParentOrigin;
    private float movementCounter;
    private float idleCounter;

    //stamina
    public float stamina;
    public float maxStamina;
    public float dValue;
    public float iValue;
    public float staminaRegenTimer;
    private bool hasEnoughStamina = true;

    //footsteps
    public bool isFootstepOver = true;
    public AudioSource footstepSource;
    public AudioClip[] footsteps;
    public AudioClip lastAudioClip;
    public float timeBetweenSteps;

    Vector3 velocity;
    bool isGrounded;
    public bool isAiming;


    private void Start()
    {
        
        QualitySettings.vSyncCount = 1;
        baseFOV = fpsCam.fieldOfView;
        maxStamina = stamina;
    }

    void Update()
    {
        //Debug.Log(Time.frameCount / Time.time);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        isSprinting = sprint && z > 0 && !isAiming && stamina != 0 && hasEnoughStamina;
        float t_adjustSpeed = speed;
        
        if(stamina < 1)
        {
            hasEnoughStamina = false;
            StartCoroutine(SufficentStaminaToRun());
        }

        if (isSprinting)
        {
            t_adjustSpeed *= sprintModifier;
        }
        if (isAiming)
        {
            t_adjustSpeed *= aimingModifier;
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
            DecreaseEnergy();
        }
        else if (stamina != maxStamina && !isSprinting)
        {
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, baseFOV, Time.deltaTime * 8f);
            IncreaseEnergy();
        }

        if (move != new Vector3(0, 0, 0) && isFootstepOver)
        {
           StartCoroutine(Footsteps());
        }
       

        if (Input.GetKey(KeyCode.Mouse1))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }
    }
    private IEnumerator Footsteps()
    {
        isFootstepOver = false;
        while (footstepSource.clip == lastAudioClip)
        {
            footstepSource.clip = footsteps[Random.Range(0, footsteps.Length)];
        }
        lastAudioClip = footstepSource.clip;
        footstepSource.Play();
        Debug.Log(isSprinting);
        if(!isSprinting && !isAiming)
            yield return new WaitForSeconds(timeBetweenSteps);
        else if (isSprinting)
            yield return new WaitForSeconds(timeBetweenSteps / sprintModifier);
        else if (!isSprinting && isAiming)
            yield return new WaitForSeconds(timeBetweenSteps / aimingModifier);
        isFootstepOver = true;
    }

    private IEnumerator SufficentStaminaToRun()
    {
        yield return new WaitForSeconds(staminaRegenTimer);
        hasEnoughStamina = true;
    }
    private void DecreaseEnergy()
    {
        if (stamina != 0)
            stamina -= dValue * Time.deltaTime;
        if (stamina <= -1)
            stamina = 0;
    }

    private void IncreaseEnergy()
    {
        stamina += iValue * Time.deltaTime;
        if (stamina >= maxStamina)
            stamina = maxStamina;
    }
}
