using UnityEngine;

public class Actor_EnemyShooter : MonoBehaviour
{
    [Header("Shoot Options")]
    public Transform FirePoint;
    public GameObject Bullet;
    public float BulletSpeed = 100f;
    public float BulletDamage = 5f;
    public float FireRate = 0.2f;

    [Header("Enemy Fire Audio")]
    public AudioClip enemyFireSound;
    private AudioSource enemyAudioSource;

    private bool isFiring = false;
    private float lastFireTime;

    private void Awake()
    {
        enemyAudioSource = GetComponent<AudioSource>();
        if (enemyAudioSource == null)
            enemyAudioSource = gameObject.AddComponent<AudioSource>();

        enemyAudioSource.playOnAwake = false;
        enemyAudioSource.loop = false;
        // 敌人纯3D音效，远距离自动衰减释放音频通道
        enemyAudioSource.spatialBlend = 1f;
        enemyAudioSource.minDistance = 5;
        enemyAudioSource.maxDistance = 25;
    }

    private void Update()
    {
        if (isFiring)
        {
            if (Time.time >= lastFireTime + FireRate)
            {
                Fire();
                lastFireTime = Time.time;
            }
        }
    }

    public void SetFire(bool shouldFire)
    {
        isFiring = shouldFire;
    }

    void Fire()
    {
        if (FirePoint == null || Bullet == null) return;

        if (enemyFireSound != null)
        {
            enemyAudioSource.PlayOneShot(enemyFireSound);
        }

        Vector3 pos = FirePoint.position;
        Quaternion dir = FirePoint.rotation;

        GameObject bulletClone = Instantiate(Bullet, pos, dir);
        Actor_Bullet bulletScript = bulletClone.GetComponent<Actor_Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDamage(BulletDamage);
        }

        Rigidbody rb = bulletClone.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(FirePoint.forward * BulletSpeed, ForceMode.VelocityChange);
        }

        Destroy(bulletClone, 2f);
    }
}