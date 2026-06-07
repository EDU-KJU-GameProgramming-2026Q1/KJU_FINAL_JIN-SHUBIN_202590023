using UnityEngine;
using TMPro;

public class Actor_Shotgun : InterfaceBase_IItem
{
    [Header("Shoot Options")]
    public Transform FirePoint;
    public GameObject Bullet;
    public float BulletSpeed = 120f;
    public float BulletDamage = 2f;

    [Header("Shotgun Settings")]
    public int pelletCount = 8;
    public float spreadAngle = 8f;

    [Header("Reload Settings")]
    public int maxAmmoInMag = 8;
    public float reloadTime = 2f;

    [Header("UI Reference")]
    public TextMeshProUGUI reloadTipText;
    public GameObject crosshair;
    public TextMeshProUGUI ammoText;

    private int currentAmmoInMag;
    private bool isReloading = false;

    void Start()
    {
        currentAmmoInMag = maxAmmoInMag;
        UpdateAmmoUI();

        if (reloadTipText != null) reloadTipText.text = "";
        if (crosshair != null) crosshair.SetActive(true);
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = $"Shotgun: {currentAmmoInMag} / {maxAmmoInMag}";
    }

    public override void OnUse()
    {
        if (!isReloading && currentAmmoInMag > 0)
        {
            Fire();
        }
    }

    public override void OnStopUse()
    {
    }

    void Update()
    {
        // ✅ C 键切换激光（现在散弹枪也能用了！）
        if (Input.GetKeyDown(KeyCode.C))
        {
            gameObject.SetActive(false);
            return;
        }

        if (isReloading) return;

        if (Input.GetKeyDown(KeyCode.R) && currentAmmoInMag < maxAmmoInMag)
        {
            Reload();
            return;
        }

        if (currentAmmoInMag <= 0)
        {
            Reload();
        }
    }

    void Fire()
    {
        currentAmmoInMag--;
        UpdateAmmoUI();

        for (int i = 0; i < pelletCount; i++)
        {
            Quaternion rot = FirePoint.rotation * Quaternion.Euler(
                Random.Range(-spreadAngle, spreadAngle),
                Random.Range(-spreadAngle, spreadAngle),
                0
            );

            GameObject bulletClone = Instantiate(Bullet, FirePoint.position, rot);
            bulletClone.GetComponent<Actor_Bullet>().SetDamage(BulletDamage);

            Rigidbody rb = bulletClone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(bulletClone.transform.forward * BulletSpeed, ForceMode.VelocityChange);
            }

            Destroy(bulletClone, 1.5f);
        }
    }

    void Reload()
    {
        if (isReloading) return;

        isReloading = true;
        if (reloadTipText != null)
            reloadTipText.text = "Shotgun Reloading...";

        if (crosshair != null) crosshair.SetActive(false);

        Invoke(nameof(FinishReload), reloadTime);
    }

    void FinishReload()
    {
        currentAmmoInMag = maxAmmoInMag;
        UpdateAmmoUI();

        isReloading = false;
        if (reloadTipText != null) reloadTipText.text = "";
        if (crosshair != null) crosshair.SetActive(true);
    }
}