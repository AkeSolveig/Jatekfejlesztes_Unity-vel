using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponSystem : MonoBehaviour
{
    //Basic stats
    public int damage;
    public float timeBetweenShooting, range, reloadTime, timeBetweenShots;
    public float defaultSpread, adsSpread;
    private float spread;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    public bool isAiming;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //muzzle flash
    public GameObject bulletHoleGraphic;
    public GameObject muzzleLight;
    [SerializeField] VisualEffect muzzleFlash2;

    //camera shake
    [SerializeField] CameraRecoil cameraRecoil;
    [SerializeField] RecoilSystem recoilSystem;

    //ads
    public Vector3 originalPosition;
    public Vector3 adsPosition;
    public float adsSpeed = 8f;


    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        originalPosition = transform.localPosition;
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

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
        MyInput();
        ADS();
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
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

            //if (rayHit.collider.CompareTag("Enemy")) rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
        }
        //Debug.DrawRay(fpsCam.transform.position, direction, Color.green);

        GameObject obj = Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.LookRotation(rayHit.normal));
        obj.transform.position += obj.transform.forward / 1000;
        //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        //muzzleflash
        muzzleLight.SetActive(true);
        muzzleFlash2.Play();
        Invoke("LightOff", 0.1f);

        //camerashake
        cameraRecoil.Fire();

        //recoil
        recoilSystem.Fire();

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
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
