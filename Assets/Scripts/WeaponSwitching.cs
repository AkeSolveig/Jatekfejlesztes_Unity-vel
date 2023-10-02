using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;

    private GameObject primary;
    private GameObject secondary;
    private GameObject primaryChild;
    private GameObject secondaryChild;
    public PlayerMovement playerMovement;


    public GameObject akm;
    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
        primary = transform.GetChild(0).gameObject;
        secondary = transform.GetChild(1).gameObject;
        primaryChild = primary.transform.GetChild(0).gameObject;
        secondaryChild = secondary.transform.GetChild(0).gameObject;

        Debug.Log(primaryChild);
        Debug.Log(primary.name);
        Debug.Log(secondary.name);
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;
        
        


        if (!Input.GetKey(KeyCode.Mouse1))
        {
            /*if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (selectedWeapon >= transform.childCount - 1)
                    selectedWeapon = 0;
                else
                    selectedWeapon++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (selectedWeapon <= 0)
                    selectedWeapon = transform.childCount - 1;
                else
                    selectedWeapon--;
            }*/

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectedWeapon = 0;

            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
            {
                selectedWeapon = 1;


            }

            if (previousSelectedWeapon != selectedWeapon)
            {
                SelectWeapon();
            }
        }
            
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                
                weapon.gameObject.SetActive(true);
            }
                
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "BuyableWeapon")
        {
            print(other.gameObject.name);
            if (selectedWeapon == 0 && Input.GetKeyDown(KeyCode.G))
            {
                Destroy(primaryChild);
                primaryChild = Instantiate(other.gameObject.transform.GetChild(0).gameObject, primary.transform);
                primaryChild.SetActive(true);
            }
            else if (selectedWeapon == 1 && Input.GetKeyDown(KeyCode.G))
            {
                Destroy(secondaryChild);
                secondaryChild = Instantiate(other.gameObject.transform.GetChild(0).gameObject, secondary.transform);
                secondaryChild.SetActive(true);
            }

        }
        
    }




}
