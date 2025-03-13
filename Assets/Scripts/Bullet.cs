using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody bulletRigidbody;

    [SerializeField] private float bulletSpeed = 200f;
    [SerializeField] private float bulletLifeTime = 3f;

    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject explosionEffect;

    private void Start()
    {
        BulletMovement();
    }

    private void BulletMovement()
    {
        //Lam bullet di chuyen
        bulletRigidbody.AddForce(transform.forward.normalized * bulletSpeed, ForceMode.Impulse);// ForceMode.Impulse is used to apply force instantly
        DestroyBullet();    
    }

    private void DestroyBullet()
    {
        //Destrpy bullet sau 1 khoang thoi gian
        Destroy(gameObject, bulletLifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log("Exactly");
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit");
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            enemy.TakeDamage(20);
            if (enemy.currentHealth <= 0)
            {
                enemy.Die();
            }
        }

        if (collision.gameObject.CompareTag("HeadEnemy"))
        {
            Debug.Log("Head hit");
            EnemyHealth enemy = collision.gameObject.GetComponentInParent<EnemyHealth>();
            enemy.TakeDamage(1000);
            if (enemy.currentHealth <= 0)
            {
                enemy.Die();
            }
        }

        if (collision.gameObject.CompareTag("Hostage"))
        {
            Hostage hos = collision.gameObject.GetComponent<Hostage>();
            if (hos.isRescue)//Neu da duoc cuu thi khong bi ban chet nua
            {
                return;
            }

            Debug.Log("Hostage hit");

            HostageHealth hostage = collision.gameObject.GetComponent<HostageHealth>();
            hostage.TakeDamage(20);
            if (hostage.currentHealth <= 0)
            {
                hostage.Die();
            }
        }

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Hostage") ||
            collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("HeadEnemy"))
        {
            var effect = Instantiate(bloodEffect, transform.position, bloodEffect.transform.rotation);
            Destroy(effect, 0.2f);
        }
        else
        {
            var effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.2f);
        }

        Destroy(gameObject);
    }
}
