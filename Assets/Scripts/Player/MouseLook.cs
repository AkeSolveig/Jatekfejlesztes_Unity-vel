using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void LateUpdate()
    {
        float MouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if(PauseMenu.gameIsPaused == false && DeathScreen.gameHasEnded == false)
        {
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * MouseX);
        }
        
    }
}