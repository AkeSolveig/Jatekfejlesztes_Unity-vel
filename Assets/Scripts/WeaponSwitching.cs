using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;

    private GameObject primary;
    private GameObject secondary;
    private GameObject pistol;
    private GameObject primaryChild;
    private GameObject secondaryChild;
    private string primaryChildName, secondaryChildName;
    public PlayerMovement playerMovement;
    private Points pointsScript;

    public bool hasPrimary = false, hasSecondary = false, weaponAlreadyInUse;

    [SerializeField] private Vector3 sheathPosition;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private float sheathPositionSpeed;


    // Start is called before the first frame update
    void Start()
    {
        pointsScript = GameObject.FindGameObjectWithTag("PointsController").GetComponent<Points>();
        
        pistol = transform.GetChild(0).gameObject;
        primary = transform.GetChild(1).gameObject;
        secondary = transform.GetChild(2).gameObject;

        /*primaryChild = primary.transform.GetChild(0).gameObject;
        secondaryChild = secondary.transform.GetChild(0).gameObject;*/
        originalPosition = transform.localPosition;
        SelectWeapon();
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

            if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2 && transform.GetChild(1).gameObject.transform.childCount != 0)
            {
                selectedWeapon = 1;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3 && transform.GetChild(2).gameObject.transform.childCount != 0)
            {
                selectedWeapon = 2;
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
                StartCoroutine(Unsheath());
            }
                
            else

                weapon.gameObject.SetActive(false);
            i++;
        }
    }

    private IEnumerator Unsheath()
    {
        transform.localPosition = sheathPosition;
        while (transform.localPosition != originalPosition)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * sheathPositionSpeed);
            yield return null;
        }
    }
    private void BuyWeapon(GameObject whichSlot,Collider other, int price)
    {
        if(whichSlot == primary)
        {
            pointsScript.SubstractScore(price);
            primaryChild = Instantiate(other.gameObject.transform.GetChild(0).gameObject, primary.transform);
            primaryChild.SetActive(true);
            primaryChildName = other.gameObject.transform.GetChild(0).gameObject.name;
        }
        else
        {
            pointsScript.SubstractScore(price);
            secondaryChild = Instantiate(other.gameObject.transform.GetChild(0).gameObject, secondary.transform);
            secondaryChild.SetActive(true);
            secondaryChildName = other.gameObject.transform.GetChild(0).gameObject.name;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        int price = other.gameObject.GetComponent<Value>().price;
        
        if (other.gameObject.tag == "BuyableWeapon")
        {
            if(hasPrimary && hasSecondary)
            {
                //Debug.Log(other.gameObject.transform.GetChild(0).gameObject.name + "    ||    " + primaryChildName +"   ||    "+secondaryChildName);
                if (other.gameObject.transform.GetChild(0).gameObject.name.Equals(primaryChildName)
                    || other.gameObject.transform.GetChild(0).gameObject.name.Equals(secondaryChildName))
                {
                    weaponAlreadyInUse = true;
                }
            }
            print(other.gameObject.name);
            if (selectedWeapon == 0 && Input.GetKeyDown(KeyCode.G) && pointsScript.score >= price)
            {
                if (!hasPrimary)
                {
                    BuyWeapon(primary, other, price);
                    selectedWeapon = 1;
                    SelectWeapon();
                    hasPrimary = true;
                }
                else if (!hasSecondary)
                {
                    BuyWeapon(secondary, other, price);
                    selectedWeapon = 2;
                    SelectWeapon();
                    hasSecondary = true;
                }
                
                
            }
            else if (selectedWeapon == 1 && Input.GetKeyDown(KeyCode.G) && pointsScript.score >= price && !weaponAlreadyInUse)
            {
                if (!hasSecondary)
                {
                    BuyWeapon(secondary, other, price);
                    selectedWeapon = 2;
                    SelectWeapon();
                    hasSecondary = true;
                }
                else
                {
                    Destroy(primaryChild);
                    BuyWeapon(primary, other, price);
                }
                
            }
            else if (selectedWeapon == 2 && Input.GetKeyDown(KeyCode.G) && pointsScript.score >= price && !weaponAlreadyInUse)
            {
                
                Destroy(secondaryChild);
                BuyWeapon(secondary, other, price);
                /*pointsScript.SubstractScore(price);
                secondaryChild = Instantiate(other.gameObject.transform.GetChild(0).gameObject, secondary.transform);
                secondaryChildName = other.gameObject.transform.GetChild(0).gameObject.name;
                secondaryChild.SetActive(true);*/
            }

        }
        if(other.gameObject.tag == "AmmoBox")
        {
            Debug.Log(other.gameObject.name);
            if (Input.GetKeyDown(KeyCode.G) && pointsScript.score >= price)
            {
                WeaponSystem[] weaponScripts = GetComponentsInChildren<WeaponSystem>();
                foreach(WeaponSystem script in weaponScripts)
                {
                    pointsScript.SubstractScore(price);
                    script.reserverdBulletsLeft = script.maxAmmo;
                }
            }
        }
        if(other.gameObject.tag == "WeaponUpgrade")
        {
            Debug.Log(other.gameObject.name);
            if(selectedWeapon == 0 && Input.GetKeyDown(KeyCode.G) && pointsScript.score >= price)
            {
                WeaponSystem weaponScript = pistol.GetComponentInChildren<WeaponSystem>();
                if (weaponScript.isUpgraded == false)
                    UpgradeWeapon(weaponScript, price);
            }
            else if (selectedWeapon == 1 && Input.GetKeyDown(KeyCode.G) && pointsScript.score >= price)
            {
                WeaponSystem weaponScript = primary.GetComponentInChildren<WeaponSystem>();
                if (weaponScript.isUpgraded == false)
                    UpgradeWeapon(weaponScript, price);
            }
            else if (selectedWeapon == 2 && Input.GetKeyDown(KeyCode.G) && pointsScript.score >= price)
            {
                WeaponSystem weaponScript = secondary.GetComponentInChildren<WeaponSystem>();
                if (weaponScript.isUpgraded == false)
                    UpgradeWeapon(weaponScript, price);
            }
        }        
    }

    private void UpgradeWeapon(WeaponSystem weaponScript, int price)
    {
        weaponScript.isUpgraded = true;
        weaponScript.damage += 50;
        weaponScript.timeBetweenShooting -= 0.02f;
        weaponScript.maxAmmo += 100;
        weaponScript.reserverdBulletsLeft = weaponScript.maxAmmo;
        weaponScript.magazineSize += 10;
        weaponScript.bulletsLeft = weaponScript.magazineSize;
        pointsScript.SubstractScore(price);
        StartCoroutine(Unsheath());
    }
    private void OnTriggerExit(Collider other)
    {
        weaponAlreadyInUse = false;
    }




}
