using UnityEngine;

public class HealingZone : MonoBehaviour
{
    [Header("治疗设置")]
    public float healAmount = 5f;      // 每次回多少血
    public float healInterval = 1f;    // 每隔几秒回一次
    public float maxHealth = 100f;     // 玩家最大血量

    private float timer;
    private PlayerHealth playerHealth; // 自动匹配你的脚本

    // 进入治疗区域
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<PlayerHealth>();
            Debug.Log("✅ 玩家进入治疗区域");
        }
    }

    // 离开治疗区域
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = null;
            Debug.Log("❌ 玩家离开治疗区域");
        }
    }

    // 持续治疗
    private void Update()
    {
        // 只有玩家在区域内 + 血量未满 才回血
        if (playerHealth != null && playerHealth.currentHealth < maxHealth)
        {
            timer += Time.deltaTime;

            if (timer >= healInterval)
            {
                HealPlayer();
                timer = 0;
            }
        }
    }

    // 核心回血方法（同步UI + 显示日志）
    void HealPlayer()
    {
        // 回血
        float newHealth = Mathf.Min(playerHealth.currentHealth + healAmount, maxHealth);
        float healValue = newHealth - playerHealth.currentHealth;

        playerHealth.currentHealth = newHealth;

        // 关键！同步血量到UI显示（你原来缺的就是这句）
        ScoreManager.Instance.AddPlayerHealth(healValue);

        // 控制台显示回血（你能看到）
        Debug.Log($"💚 成功回血：+{healValue} | 当前血量：{playerHealth.currentHealth}/{maxHealth}");
    }
}