using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponSystem : MonoBehaviour
{
    //Basic stats
    public int damage;
    public float timeBetweenShooting, range, reloadTime, timeBetweenShots;
    public float defaultSpread, adsSpread;
    private float spread;

    public int magazineSize, bulletsPerTap, maxAmmo, reserverdBulletsLeft, bulletsLeft, bulletsShot;

    public bool allowButtonHold;
    public bool isUpgraded = false;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    private TextMeshProUGUI ammoCountText;

    //muzzle flash
    public GameObject bulletHoleGraphic;
    public GameObject bulletHoleGraphicBody;
    public GameObject muzzleLight;
    [SerializeField] VisualEffect muzzleFlash2;

    //camera shake
    [SerializeField] CameraRecoil cameraRecoil;
    [SerializeField] RecoilSystem recoilSystem;

    //ads
    public Vector3 originalPosition;
    public Vector3 adsPosition;
    public float adsSpeed = 8f;
    public bool isAiming;

    //weapon sound
    public AudioSource source;
    public AudioClip fireClip;
    public AudioClip reloadClip;

    //reload position
    public Vector3 reloadPosition;
    public float reloadPositionSpeed = 20f;

    //enemy body part hitbox
    private ZombieDamageHitbox damageEnemy; 

    private void Awake()
    {
        ammoCountText = GameObject.FindGameObjectWithTag("AmmoCountUI").GetComponent<TextMeshProUGUI>();
        fpsCam = GameObject.Find("Camera").GetComponent<Camera>();
        cameraRecoil = GameObject.Find("CamRecoil").GetComponent<CameraRecoil>();
        reserverdBulletsLeft = maxAmmo;
        bulletsLeft = magazineSize;
        readyToShoot = true;
        originalPosition = transform.localPosition;
        source = GetComponent<AudioSource>();
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading && reserverdBulletsLeft != 0) Reload();

        //shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void ADS()
    {
        if (Input.GetKey(KeyCode.Mouse1) && !reloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, adsPosition, Time.deltaTime * adsSpeed);
            spread = adsSpread;
            isAiming = true;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * adsSpeed);
            spread = defaultSpread;
            isAiming = false;
        }
    }
  
    private void Update()
    {
        ammoCountText.text = bulletsLeft + " / " + reserverdBulletsLeft;
        MyInput();
        ADS();
        if (reloading) 
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, reloadPosition, Time.deltaTime * reloadPositionSpeed);
        }
        
            
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);

        //calc direction
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, z);
        
        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit))
        {
            if(rayHit.collider.gameObject.GetComponent<ZombieDamageHitbox>() != null)
            {
                GameObject obj = Instantiate(bulletHoleGraphicBody, rayHit.point, Quaternion.LookRotation(rayHit.normal));
                obj.transform.position += obj.transform.forward / 1000;
                
                damageEnemy = rayHit.collider.GetComponent<ZombieDamageHitbox>();
                if(damageEnemy.bodyPart == ZombieDamageHitbox.collisonType.head)
                {
                    damageEnemy.BodyPartHit(damage * 3, true);
                }
                else if (damageEnemy.bodyPart == ZombieDamageHitbox.collisonType.body)
                {
                    damageEnemy.BodyPartHit(damage, false);
                }
                else if (damageEnemy.bodyPart == ZombieDamageHitbox.collisonType.limbs)
                {
                    damageEnemy.BodyPartHit(damage/2, false);
                }
            }
            else
            {
                GameObject obj = Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.LookRotation(rayHit.normal));
                obj.transform.position += obj.transform.forward / 1000;
            }
        }
        //Debug.DrawRay(fpsCam.transform.position, direction, Color.green);

       

        //muzzleflash
        muzzleLight.SetActive(true);
        muzzleFlash2.Play();
        Invoke("LightOff", 0.1f);

        //camerashake
        cameraRecoil.Fire();

        //recoil
        recoilSystem.Fire();

        if(source.isPlaying)
        {
            source.Stop();
            source.Play();
        }
        source.PlayOneShot(fireClip);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        cameraRecoil.Fire();
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
        
    }


    private void LightOff()
    {
        muzzleLight.SetActive(false);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        //GetComponent<Animator>().Play("reload_m1a1", -1, 0f);
        //GetComponent<Animator>().SetBool("isReloading", true);
        
        
        //transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * reloadPositionSpeed);
        reloading = true;
        source.PlayOneShot(reloadClip);
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        //GetComponent<Animator>().SetBool("isReloading", false);
        int shotsFired = magazineSize - bulletsLeft;
        int totalBullets = reserverdBulletsLeft + bulletsLeft;

        if(totalBullets < magazineSize)
        {
            bulletsLeft = totalBullets;
            reserverdBulletsLeft = 0;
            totalBullets = 0;
        }
        else
        {
            bulletsLeft = magazineSize;
            reserverdBulletsLeft -= shotsFired;
            totalBullets = 0;
        }
        reloading = false;
    }
}
