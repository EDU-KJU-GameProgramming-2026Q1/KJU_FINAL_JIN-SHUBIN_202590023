using UnityEngine;

public class Actor_Bullet : MonoBehaviour
{
    public GameObject MissEffect, HitEffect;
    public GameObject ShootSound, HitSound;
    private float bulletDamage = 5f;
    private bool isHit = false;

    private Rigidbody rb;
    private Vector3 lastVelocity; // [추가] 물리 엔진이 속도를 0으로 만들기 전의 속도를 기억할 변수

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // [추가] 충돌하기 직전 프레임까지의 실제 속도를 매 프레임 안전하게 기록합니다.
        if (rb != null && rb.velocity.sqrMagnitude > 0.1f)
        {
            lastVelocity = rb.velocity;
        }
    }

    public void SetDamage(float amount)
    {
        bulletDamage = amount;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isHit) return;

        ContactPoint contactPoint = collision.contacts[0];
        if (collision.gameObject.CompareTag("Player"))
        {
            // 全局查找场景内盾牌
            Actor_Shield shield = FindObjectOfType<Actor_Shield>();
            // 判断：存在盾牌 + 正在举盾 + 盾牌有耐久
            if (shield != null && shield.IsBlockActive())
            {
                isHit = true;
                shield.TakeShieldDamage(bulletDamage);
                Destroy(gameObject);
                return; // 直接销毁子弹，不执行玩家扣血
            }

            Debug.Log($"[OnTriggerEnter] Hit {collision.gameObject.name}! Damage: " + bulletDamage);
            isHit = true;
            ShowEffect(HitEffect, contactPoint);
            //ScoreManager.Instance.AddScore(-10);    
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"[OnTriggerEnter] Hit {collision.gameObject.name}! Damage: " + bulletDamage);
            isHit = true;
            ShowEffect(HitEffect, contactPoint);
            //ScoreManager.Instance.AddScore(10);          
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Shootable"))
        {
            // Debug.Log("[OnCollisionEnter] Miss Target! No Damage");
            ShowEffect(MissEffect, contactPoint);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isHit) return;

        Vector3 contactPoint = other.ClosestPoint(transform.position);
        if (other.CompareTag("Player"))
        {
            // 全局查找场景内盾牌
            Actor_Shield shield = FindObjectOfType<Actor_Shield>();
            if (shield != null && shield.IsBlockActive())
            {
                isHit = true;
                shield.TakeShieldDamage(bulletDamage);
                Destroy(gameObject);
                return; // 格挡成功，跳过玩家受伤
            }

            isHit = true;
            Debug.Log($"[OnTriggerEnter] Hit {other.name}! Damage: " + bulletDamage);
            ShowEffect(HitEffect, contactPoint);

            //ScoreManager.Instance.AddScore(-10);
            other.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            isHit = true;
            Debug.Log($"[OnTriggerEnter] Hit {other.name}! Damage: " + bulletDamage);
            ShowEffect(HitEffect, contactPoint);

            //ScoreManager.Instance.AddScore(10);
            other.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Shootable"))
        {
            Debug.Log($"[OnTriggerEnter] Miss {other.name} ");
            ShowEffect(MissEffect, contactPoint);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }

    void ShowEffect(GameObject Effect, ContactPoint contactPoint)
    {
        Vector3 pos = contactPoint.point + (contactPoint.normal * 0.05f);
        Quaternion dir = Quaternion.LookRotation(contactPoint.normal);
        GameObject hitEffectClone = Instantiate(Effect, pos, dir);
        Destroy(hitEffectClone, 2f);
    }

    void ShowEffect(GameObject Effect, Vector3 contactPoint)
    {
        Vector3 surfaceNormal = (transform.position - contactPoint).normalized;
        Vector3 pos = contactPoint + (surfaceNormal * 0.05f);
        Quaternion dir = Quaternion.LookRotation(surfaceNormal);
        GameObject hitEffectClone = Instantiate(Effect, pos, dir);
        Destroy(hitEffectClone, 2f);
    }

    void ShowEffect(GameObject Effect, float calibTime)
    {
        Vector3 bulletSpeed = lastVelocity;

        if (bulletSpeed.sqrMagnitude < 0.001f && rb != null)
        {
            bulletSpeed = rb.velocity;
        }

        Vector3 pos = transform.position;
        Quaternion dir = transform.rotation;

        if (bulletSpeed.sqrMagnitude > 0.001f)
        {
            pos = transform.position - (bulletSpeed * calibTime);
            dir = Quaternion.LookRotation(bulletSpeed.normalized);
        }

        GameObject hitEffectClone = Instantiate(Effect, pos, dir);
        Destroy(hitEffectClone, 2f);
    }


    void ShowEffect(GameObject Effect)
    {
        Vector3 pos = transform.position;
        Quaternion dir = transform.rotation;

        GameObject hitEffectClone = Instantiate(Effect, pos, dir);
        Destroy(hitEffectClone, 2f);
    }
}