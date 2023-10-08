using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;

    //child references
    private GameObject primary, primaryChild;
    private GameObject secondary, secondaryChild;
    private GameObject pistol;

    private string primaryChildName, secondaryChildName;

    // default references
    public PlayerStats playerStats;
    public PlayerMovement playerMovement;
    private Points pointsScript;

    //weapon buy
    public bool hasPrimary = false, hasSecondary = false, weaponAlreadyInUse;

    //Sheath weapon
    [SerializeField] private Vector3 sheathPosition;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private float sheathPositionSpeed;

    //interacting
    private bool playerInteractionKey, doublesShotUpgraded = false, staminUpgraded = false;
    private Collider currentTrigger;

    //UI
    public GameObject healthUI;
    public GameObject DoubleShotUI;
    public GameObject StaminaUI;

    // Start is called before the first frame update
    void Start()
    {
        
        //weaponswitch
        pistol = transform.GetChild(0).gameObject;
        primary = transform.GetChild(1).gameObject;
        secondary = transform.GetChild(2).gameObject;

        //sheath
        originalPosition = transform.localPosition;
        SelectWeapon();
        Debug.Log(primaryChild);
        Debug.Log(primary.name);
        Debug.Log(secondary.name);

        //interaction
        pointsScript = GameObject.FindGameObjectWithTag("PointsController").GetComponent<Points>();
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        playerInteractionKey = Input.GetKeyDown(KeyCode.F);

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
        //interactions
        if(currentTrigger != null)
        {
            int price = currentTrigger.gameObject.GetComponent<Value>().price;
            bool canAfford = pointsScript.score >= price;

            if (currentTrigger.gameObject.tag == "BuyableWeapon")
            {
                if (hasPrimary && hasSecondary)
                {
                    //Debug.Log(other.gameObject.transform.GetChild(0).gameObject.name + "    ||    " + primaryChildName +"   ||    "+secondaryChildName);
                    if (currentTrigger.gameObject.transform.GetChild(0).gameObject.name.Equals(primaryChildName)
                        || currentTrigger.gameObject.transform.GetChild(0).gameObject.name.Equals(secondaryChildName))
                    {
                        weaponAlreadyInUse = true;
                    }
                }
                if (!weaponAlreadyInUse)
                {
                    if (selectedWeapon == 0 && playerInteractionKey && canAfford)
                    {
                        if (!hasPrimary)
                        {
                            BuyWeapon(primary, currentTrigger, price);
                            selectedWeapon = 1;
                            SelectWeapon();
                            hasPrimary = true;
                        }
                        else if (!hasSecondary)
                        {
                            BuyWeapon(secondary, currentTrigger, price);
                            selectedWeapon = 2;
                            SelectWeapon();
                            hasSecondary = true;
                        }


                    }
                    else if (selectedWeapon == 1 && playerInteractionKey && canAfford)
                    {
                        if (!hasSecondary)
                        {
                            BuyWeapon(secondary, currentTrigger, price);
                            selectedWeapon = 2;
                            SelectWeapon();
                            hasSecondary = true;
                        }
                        else
                        {
                            Destroy(primaryChild);
                            BuyWeapon(primary, currentTrigger, price);
                        }

                    }
                    else if (selectedWeapon == 2 && playerInteractionKey && canAfford)
                    {

                        Destroy(secondaryChild);
                        BuyWeapon(secondary, currentTrigger, price);
                        /*pointsScript.SubstractScore(price);
                        secondaryChild = Instantiate(other.gameObject.transform.GetChild(0).gameObject, secondary.transform);
                        secondaryChildName = other.gameObject.transform.GetChild(0).gameObject.name;
                        secondaryChild.SetActive(true);*/
                    }
                }
                

            }
            if (currentTrigger.gameObject.tag == "AmmoBox")
            {
                Debug.Log(currentTrigger.gameObject.name);
                if (playerInteractionKey && canAfford)
                {
                    WeaponSystem[] weaponScripts = GetComponentsInChildren<WeaponSystem>(true);
                    foreach (WeaponSystem script in weaponScripts)
                    {
                        script.reserverdBulletsLeft = script.maxAmmo;
                    }
                    pointsScript.SubstractScore(price);
                }
            }
            if (currentTrigger.gameObject.tag == "WeaponUpgrade")
            {
                //Debug.Log(currentTrigger.gameObject.name);
                if (selectedWeapon == 0 && playerInteractionKey && canAfford)
                {
                    Debug.Log("inside if pistol");
                    WeaponSystem weaponScript = pistol.GetComponentInChildren<WeaponSystem>();
                    Debug.Log(weaponScript);
                    if (weaponScript.isUpgraded == false)
                    {
                        UpgradeWeapon(weaponScript, price);
                    }
                        
                }
                else if (selectedWeapon == 1 && playerInteractionKey && canAfford)
                {
                    WeaponSystem weaponScript = primary.GetComponentInChildren<WeaponSystem>();
                    if (weaponScript.isUpgraded == false)
                        UpgradeWeapon(weaponScript, price);
                }
                else if (selectedWeapon == 2 && playerInteractionKey && canAfford)
                {
                    WeaponSystem weaponScript = secondary.GetComponentInChildren<WeaponSystem>();
                    if (weaponScript.isUpgraded == false)
                        UpgradeWeapon(weaponScript, price);
                }
            }
            if (currentTrigger.gameObject.tag == "HealthUpgrade")
            {
                if (playerInteractionKey && canAfford && playerStats.isHealthUpgraded == false)
                {
                    playerStats.SetMaxHealthTo(200);
                    pointsScript.SubstractScore(price);
                    playerStats.isHealthUpgraded = true;
                    healthUI.SetActive(true);
                }
            }
            if (currentTrigger.gameObject.tag == "DoubleShotUpgrade" )
            {
                if (playerInteractionKey && canAfford && doublesShotUpgraded == false)
                {
                    WeaponSystem[] weaponScripts = GetComponentsInChildren<WeaponSystem>(true);
                    Debug.Log(weaponScripts);
                    foreach (WeaponSystem script in weaponScripts)
                    {
                        script.damage *= 2;
                    }
                    pointsScript.SubstractScore(price);
                    doublesShotUpgraded = true;
                    DoubleShotUI.SetActive(true);
                }
            }
            if(currentTrigger.gameObject.tag == "StaminaUpgrade" && staminUpgraded == false)
            {
                if(playerInteractionKey && canAfford)
                {
                    pointsScript.SubstractScore(price);
                    playerMovement.maxStamina *= 1.5f;
                    playerMovement.staminaRegenTimer = 2f;
                    playerMovement.dValue = 3;
                    playerMovement.iValue = 5;
                    playerMovement.sprintModifier = 1.4f;
                    staminUpgraded = true;
                    StaminaUI.SetActive(true);
                }
            }
            if(currentTrigger.gameObject.tag == "Door")
            {
                
                if(playerInteractionKey && canAfford)
                {
                    pointsScript.SubstractScore(price);
                    Debug.Log("door");
                    currentTrigger.gameObject.GetComponent<NavMeshObstacle>().enabled = false;
                    Door doorScript = currentTrigger.gameObject.GetComponent<Door>();
                    doorScript.Open(transform.position);              
                }
            }
            if (currentTrigger.gameObject.tag == "DoubleDoor")
            {

                if (playerInteractionKey && canAfford)
                {
                    pointsScript.SubstractScore(price);
                    Debug.Log("door");
                    Door doorScript1 = currentTrigger.transform.GetChild(0).GetComponent<Door>();
                    Door doorScript2 = currentTrigger.transform.GetChild(1).GetComponent<Door>();
                    doorScript1.Open(transform.position);
                    doorScript2.Open(transform.position);
                }
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
            if (doublesShotUpgraded == true)
                DoubleShot(primaryChild);
        }
        else
        {
            pointsScript.SubstractScore(price);
            secondaryChild = Instantiate(other.gameObject.transform.GetChild(0).gameObject, secondary.transform);
            secondaryChild.SetActive(true);
            secondaryChildName = other.gameObject.transform.GetChild(0).gameObject.name;
            if (doublesShotUpgraded == true)
                DoubleShot(secondaryChild);
        }
    }

    private void DoubleShot(GameObject weapon)
    {
        WeaponSystem weaponScript = weapon.GetComponentInChildren<WeaponSystem>();
        weaponScript.damage *= 2;
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
    private void OnTriggerEnter(Collider other)
    {
        currentTrigger = other;
    }
    private void OnTriggerExit(Collider other)
    {
        if (currentTrigger == other)
        {
            currentTrigger = null;
        }
        weaponAlreadyInUse = false;
    }




}
