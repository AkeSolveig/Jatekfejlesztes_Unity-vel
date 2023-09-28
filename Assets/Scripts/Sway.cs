using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    public float intensity;
    public float smooth;
    [SerializeField] WeaponSystem weaponSystem;
    private float adsIntensity = 1;

    private Quaternion origin_rotation,  t_x_adj,  t_y_adj;



    private void Start()
    {
        origin_rotation = transform.localRotation;
    }
    private void Update()
    {
        UpdateSway();
    }

    private void UpdateSway()
    {
        //controls
        float t_x_mouse = Input.GetAxis("Mouse X");
        float t_y_mouse = Input.GetAxis("Mouse Y");

        //calculate target rotation
        if (weaponSystem.isAiming == false)
        {
            t_x_adj = Quaternion.AngleAxis(-intensity * t_x_mouse, Vector3.up);
            t_y_adj = Quaternion.AngleAxis(intensity * t_y_mouse, Vector3.right);
        }
        else
        {
            t_x_adj = Quaternion.AngleAxis(-adsIntensity * t_x_mouse, Vector3.up);
            t_y_adj = Quaternion.AngleAxis(adsIntensity * t_y_mouse, Vector3.right);
        }
         

        Quaternion target_rotation = origin_rotation * t_x_adj * t_y_adj;

        //rotate towards target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Time.deltaTime * smooth);


    }
}
