using UnityEngine;
using TMPro;

public class Actor_Rifle : InterfaceBase_IItem
{
    [Header("Shoot Options")]
    public Transform FirePoint;
    public GameObject Bullet;
    public float BulletSpeed = 100f;
    public float BulletDamage = 5f;

    [Header("Reload Options")]
    public int maxAmmoInMag = 30;
    public float reloadTime = 1.5f;

    [Header("UI Reference")]
    public TextMeshProUGUI reloadTipText;
    public GameObject crosshair;
    public TextMeshProUGUI ammoText;

    private int currentAmmoInMag;
    private bool isReloading = false;
    private bool isFiring = false;
    private float lastFireTime;

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
            ammoText.text = $"Rifle: {currentAmmoInMag} / {maxAmmoInMag}";
    }

    public override void OnUse()
    {
        isFiring = true;
    }

    public override void OnStopUse()
    {
        isFiring = false;
    }

    private void Update()
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
            return;
        }

        if (isFiring && Time.time >= lastFireTime + itemData.FireRate)
        {
            Fire();
            lastFireTime = Time.time;
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
            reloadTipText.text = "Rifle Reloading...";

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