using NUnit.Framework;
using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerFireManager : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private bool isShooting, readyToShoot;
    private bool allowReset = true;
    [SerializeField] private float shootingDelay = 2f;//tốc độ ra đạn (vd: cứ 2s ra đạn 1 lần)

    //Burst
    [SerializeField] private int bulletsPerBurst = 3;
    [SerializeField] private int burstBulletLeft;

    //Spread
    [SerializeField] private float spreadIntensity;//Độ giật, độ nhiễu của súng

    //Cái đống ở trên để set up cho từng loại súng như súng trường, súng lục,... đổi súng cần đổi theo (trừ cái camera với đống bool).
    //Muốn đổi tốc độ đạn => qua viên đạn mà đổi

    [Header("Muzzle")]
    // --- Muzzle ---
    public GameObject muzzlePrefab;
    public GameObject muzzlePosition;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public enum WeaponType
    {
        Machine,
        Pistol
    }

    public ShootingMode currentShootingMode;
    public WeaponType currentWeaponType;

    [Header("Bullet and shoot")]
    [SerializeField] private GameObject bulletPrefab;
    private Bullet bullet;
    [SerializeField] private Transform bulletSpawnPoint;

    //Các thông số của viên đạn đã được đưa qua script Bullet.cs
    //private float bulletSpeed = 10f;
    //private float bulletLifeTime = 3f;

    //Các thông số để bắn bằng RaycastHit
    //[SerializeField] private GameObject enemyPrefab;
    //[SerializeField] private float range = 100f;

    [Header("Sound effect")]
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bulletsPerBurst;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //isPlayer = gameObject.tag == "Player" ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        //Dành cho player
        if (currentShootingMode == ShootingMode.Auto)
        {
            //holding down left mouse button
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if ((currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst))
        {
            //clicking left mouse button once
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readyToShoot && isShooting)
        {
            burstBulletLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        Debug.Log("Shoot");
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //Bắn bằng cách sinh bullet
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, transform.rotation);
        var muzzleVFX = Instantiate(muzzlePrefab, muzzlePosition.transform.position, 
            Quaternion.LookRotation(new Vector3(-transform.forward.z, transform.forward.y, transform.forward.x)), transform);
        //Destroy(muzzleVFX, 100f);
        bullet.transform.forward = shootingDirection;

        //Dòng ghi chú phía dưới là cách làm viên đạn di chuyển ở script này đã được đưa qua script Bullet.cs
        //var bulletRigidbody = bullet.GetComponent<Rigidbody>();
        ////Lam bullet di chuyen
        //bulletRigidbody.AddForce(bulletSpawnPoint.forward.normalized * bulletSpeed, ForceMode.Impulse);// ForceMode.Impulse is used to apply force instantly
        ////Destroy bullet sau 1 khoang thoi gian
        //Destroy(bullet, bulletLifeTime);

        //Bắn bằng cách sử dụng raycast hit để xác định hướng bắn của viên đạn
        //RaycastHit hit;
        //if (Physics.Raycast(bulletSpawnPoint.position, bulletSpawnPoint.forward, out hit, range))
        //{ 
        //    Debug.Log(hit.transform.gameObject.tag);
        //}

        //check if we are done shooting
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //Burst mode
        if (currentShootingMode == ShootingMode.Burst && burstBulletLeft > 1)//we already shoot once before this check
        {
            burstBulletLeft--;
            Invoke("FireWeapon", shootingDelay);
        }

        AddSound();
        ;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray;
        if (playerCamera != null)
        {
            ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        }
        else
        {
            ray = new Ray(bulletSpawnPoint.position, transform.forward);
        }

        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            //hitting something
            targetPoint = hit.point;
        }
        else
        {
            //shooting at the air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawnPoint.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //returning the shooting direction and spread
        return direction + new Vector3(x, y, 0);
    }

    void AddSound()
    {
        if ((currentShootingMode == ShootingMode.Auto || currentShootingMode == ShootingMode.Burst) && currentWeaponType == WeaponType.Machine)
        {
            AudioClip clip = AudioManager.audioInstance.sfxSounds.Find(x => x.name == "MachineAutoGun").audioClip;
            audioSource.PlayOneShot(clip);
        }
        else if (currentShootingMode == ShootingMode.Single && currentWeaponType == WeaponType.Pistol)
        {
            AudioClip clip = AudioManager.audioInstance.sfxSounds.Find(x => x.name == "PistolSingleGun").audioClip;
            audioSource.PlayOneShot(clip);
        }
    }
}
