using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private Vector3 targetWeaponBobPosition;
    private Vector3 weaponOriginalPosition;
    private float movementCounter;
    private float idleCounter;
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        weaponOriginalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isSprinting = sprint && z > 0 && playerMovement.isAiming == false;

        if (x == 0 && z == 0 && playerMovement.isAiming == false)
        {
            HeadBob(idleCounter, 0.025f, 0.025f);
            idleCounter += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);
        }
        else if (isSprinting == false && playerMovement.isAiming == false)
        {
            HeadBob(movementCounter, 0.035f, 0.035f);
            movementCounter += Time.deltaTime * 4.5f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetWeaponBobPosition, Time.deltaTime * 8f);
        }
        else if (isSprinting == true && playerMovement.isAiming == false)
        {
            HeadBob(movementCounter, 0.03f, 0.045f);
            movementCounter += Time.deltaTime * 5.5f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);
        }
        
    }

    void HeadBob(float p_z, float p_x_intensity, float p_y_intensity)
    {
        targetWeaponBobPosition = weaponOriginalPosition + new Vector3(Mathf.Cos(p_z) * p_x_intensity, Mathf.Sin(p_z * 2) * p_y_intensity, 0);
    }

}
