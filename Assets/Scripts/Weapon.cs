using NUnit.Framework;
using System;
using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
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

    [Header("Enemy or player")]
    [SerializeField] private bool isPlayer;
    [SerializeField] private EnemyAI enemy;
    [SerializeField] private Vector3 posWhenShooting;
    private Vector3 startPosition;

    [Header("Sound effect")]
    [SerializeField] private AudioSource audioSource;

    [Header("Muzzle")]
    // --- Muzzle ---
    [SerializeField] private GameObject muzzlePrefab;
    [SerializeField] private GameObject muzzlePosition;

    [Header("Ammo And Reload")]
    public int currentAmmo;
    public int ammoClip = 0;
    [SerializeField] private int akMaxAmmo = 31;
    [SerializeField] private int pistolMaxAmmo = 17;
    [SerializeField] private bool isReloading = false;
    [SerializeField] private float reloadTime = 1.5f;
    private TextMeshProUGUI ammoText;
    private Animator reloadAnimator;
    private GameObject akIcon;
    private GameObject pistolIcon;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bulletsPerBurst;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isPlayer)
        {
            ammoText = GameObject.FindGameObjectWithTag("AmmoText").GetComponent<TextMeshProUGUI>();
        }
        
        audioSource = GetComponent<AudioSource>();

        startPosition = transform.localPosition;
        //isPlayer = gameObject.tag == "Player" ? true : false;

        if (!isPlayer)//Danh cho enemy
        {
            StartCoroutine(AutoShootForEnemy(2f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Dành cho player
        if (currentShootingMode == ShootingMode.Auto && isPlayer)
        {
            //holding down left mouse button
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if ((currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst) && isPlayer)
        {
            //clicking left mouse button once
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readyToShoot && isShooting)
        {
            burstBulletLeft = bulletsPerBurst;
            FireWeapon();
        }

        if (isPlayer)
        {
            //Kiểm tra xem viên đạn còn hay không
            if (currentAmmo <= 0 && !isReloading && ammoClip != 0)
            {
                if (Input.GetKeyDown(KeyCode.R) && currentWeaponType == WeaponType.Machine)
                {
                    reloadAnimator = GameObject.FindGameObjectWithTag("AK").GetComponent<Animator>();
                    reloadAnimator.SetTrigger("Reload");

                    isReloading = true;
                    reloadAnimator.SetTrigger("Reload");
                    StartCoroutine(Reload());
                }

                else if (Input.GetKeyDown(KeyCode.R) && currentWeaponType == WeaponType.Pistol)
                {
                    reloadAnimator = GameObject.FindGameObjectWithTag("Pistol").GetComponent<Animator>();
                    reloadAnimator.SetTrigger("Reload");
                    isReloading = true;
                    reloadAnimator.SetTrigger("Reload");
                    StartCoroutine(Reload());
                }
            }
            else if (currentAmmo != 0  && !isReloading && ammoClip != 0)
            {
                if (Input.GetKeyDown(KeyCode.R) && currentWeaponType == WeaponType.Machine)
                {
                    reloadAnimator = GameObject.FindGameObjectWithTag("AK").GetComponent<Animator>();
                    reloadAnimator.SetTrigger("Reload");

                    isReloading = true;
                    reloadAnimator.SetTrigger("Reload");
                    StartCoroutine(Reload());
                }

                else if (Input.GetKeyDown(KeyCode.R) && currentWeaponType == WeaponType.Pistol)
                {
                    reloadAnimator = GameObject.FindGameObjectWithTag("Pistol").GetComponent<Animator>();
                    reloadAnimator.SetTrigger("Reload");
                    isReloading = true;
                    reloadAnimator.SetTrigger("Reload");
                    StartCoroutine(Reload());
                }
            }

            ammoText.text = currentAmmo + " / " + ammoClip;
        }
    }

    private void FireWeapon()
    {
        if (GameState.gameStateInstance.currentGameState != GameState.State.playing)
        {
            return;
        }

        if (!isPlayer)
        {
            readyToShoot = false;

            Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

            //Bắn bằng cách sinh bullet
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, transform.rotation);

            //Chay vfx
            muzzlePrefab.GetComponent<ParticleSystem>().Play();

            bullet.transform.forward = shootingDirection;

            AddSound();
        }

        else if (currentAmmo > 0)
        {
            readyToShoot = false;

            currentAmmo--;

            Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

            //Bắn bằng cách sinh bullet
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, transform.rotation);

            //Chay vfx
            muzzlePrefab.GetComponent<ParticleSystem>().Play();

            bullet.transform.forward = shootingDirection;

            AddSound();
        }

        else if (currentAmmo == 0)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(AudioManager.audioInstance.sfxSounds.Find(x => x.name.Equals("OutOfAmmo")).audioClip);
            }
            Debug.Log("Out of ammo");
        }



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

    IEnumerator AutoShootForEnemy(float waitingTime)
    {
        while (true)
        {
            if (enemy.isAttacking)
            {
                FireWeapon();
                transform.localPosition = posWhenShooting;

                yield return new WaitForSeconds(waitingTime);
                burstBulletLeft = bulletsPerBurst;
            }
            else
            {
                transform.localPosition = startPosition;
            }
            yield return null;
        }
    }

    IEnumerator Reload()
    {
        Debug.Log("Reloading...");
        audioSource.PlayOneShot(AudioManager.audioInstance.sfxSounds.Find(x => x.name.Equals("ReloadAmmo")).audioClip);

        yield return new WaitForSeconds(reloadTime);

        if (currentWeaponType == WeaponType.Machine)
        {
            int ammoToRefill = akMaxAmmo - currentAmmo;
            ammoToRefill = (ammoClip - ammoToRefill) > 0 ? ammoToRefill : ammoClip;
            currentAmmo += ammoToRefill;
            ammoClip -= ammoToRefill; ;
        }
        else if (currentWeaponType == WeaponType.Pistol)
        {
            int ammoToRefill = pistolMaxAmmo - currentAmmo;
            ammoToRefill = (currentAmmo - ammoToRefill) > 0 ? ammoToRefill : ammoClip;
            currentAmmo += ammoToRefill;
            ammoClip -= ammoToRefill; ;
        }
        
       

        isReloading = false;
    }
}
