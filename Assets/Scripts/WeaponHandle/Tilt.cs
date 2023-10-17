using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilt : MonoBehaviour
{
    [SerializeField] WeaponSystem weaponySystem;
    //position
    public float amount = 0.02f;
    public float maxAmount = 0.06f;
    public float smoothAmount = 6f;

    //rotation
    public float rotationAmount = 4f;
    public float maxRotationAmount = 5f;
    public float smoothRotation = 12f;

    public bool rotationX = true, rotationY = true, rotationZ = true;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private float InputX, InputY;

    private void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        CalculateSway();
        MoveSway();
        TiltSway();
    }

    private void CalculateSway()
    {
        InputX = Input.GetAxis("Mouse X");
        InputY = Input.GetAxis("Mouse Y");
    }

    private void MoveSway()
    {
        if (weaponySystem.isAiming == false)
        {
            float moveX = Mathf.Clamp(InputX * amount, -maxAmount, maxAmount);
            float moveY = Mathf.Clamp(InputY * amount, -maxAmount, maxAmount);

            Vector3 finalPosition = new Vector3(moveX, moveY, 0);

            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
        }
        else transform.localPosition = initialPosition;
        
    }

    private void TiltSway()
    {
        if (weaponySystem.isAiming == false)
        {
            float tiltY = Mathf.Clamp(InputX * rotationAmount, -maxRotationAmount, maxRotationAmount);
            float tiltX = Mathf.Clamp(InputY * rotationAmount, -maxRotationAmount, maxRotationAmount);

            Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationX ? -tiltX : 0f, rotationY ? tiltY : 0f, rotationZ ? tiltY : 0f));

            transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, smoothRotation * Time.deltaTime);
        }
        else transform.localPosition = initialPosition;

    }
}