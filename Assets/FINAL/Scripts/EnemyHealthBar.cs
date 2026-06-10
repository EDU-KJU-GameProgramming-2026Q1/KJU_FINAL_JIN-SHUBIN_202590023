using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("血条UI")]
    public Image fillImage;
    private Transform targetEnemy;
    private Camera mainCamera;
    public Vector3 barOffset = new Vector3(0, 2.3f, 0);

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (targetEnemy == null || mainCamera == null) return;
        // 固定在敌人头顶
        transform.position = targetEnemy.position + barOffset;
        // 血条始终朝向镜头
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }

    // 更新血量比例
    public void UpdateHealthBar(float currentHp, float maxHp)
    {
        fillImage.fillAmount = currentHp / maxHp;
    }

    // 敌人死亡隐藏血条
    public void HideBar()
    {
        gameObject.SetActive(false);
    }

    // 绑定目标敌人
    public void SetTarget(Transform enemyTrans)
    {
        targetEnemy = enemyTrans;
    }
}