using UnityEngine;
using TMPro;

public class Actor_Pistol : InterfaceBase_IItem
{
    [Header("Shoot Options")]
    public Transform FirePoint;
    public GameObject Bullet;
    public float BulletSpeed = 100f;
    public float BulletDamage = 1f;

    [Header("Reload Settings")]
    public int maxAmmoInMag = 12;
    public float reloadTime = 1f;

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
            ammoText.text = $"Pistol: {currentAmmoInMag} / {maxAmmoInMag}";
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
        // 空实现，防止报错
    }

    void Update()
    {
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

        Vector3 pos = FirePoint.position;
        Quaternion dir = FirePoint.rotation;

        GameObject bulletClone = Instantiate(Bullet, pos, dir);
        bulletClone.GetComponent<Actor_Bullet>().SetDamage(BulletDamage);

        Rigidbody rb = bulletClone.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(FirePoint.forward * BulletSpeed, ForceMode.VelocityChange);
        }

        Destroy(bulletClone, 2f);
    }

    void Reload()
    {
        if (isReloading) return;

        isReloading = true;
        if (reloadTipText != null)
            reloadTipText.text = "Pistol Reloading...";

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