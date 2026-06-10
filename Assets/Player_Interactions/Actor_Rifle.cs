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

    [Header("Audio Sound Options")]
    public AudioClip fireAudioClip;
    public AudioClip reloadAudioClip;
    // 分离双音源：开火、换弹互不抢占通道
    private AudioSource fireAudioSource;
    private AudioSource reloadAudioSource;

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

        // 自动创建开火专用AudioSource
        Transform fireAudioTrans = transform.Find("FireAudioSource");
        if (fireAudioTrans == null)
        {
            GameObject fireObj = new GameObject("FireAudioSource");
            fireObj.transform.SetParent(transform, false);
            fireAudioSource = fireObj.AddComponent<AudioSource>();
        }
        else
        {
            fireAudioSource = fireAudioTrans.GetComponent<AudioSource>();
        }
        fireAudioSource.playOnAwake = false;
        fireAudioSource.loop = false;
        fireAudioSource.spatialBlend = 0.2f;
        fireAudioSource.minDistance = 1;
        fireAudioSource.maxDistance = 30;

        // 自动创建换弹专用AudioSource
        Transform reloadAudioTrans = transform.Find("ReloadAudioSource");
        if (reloadAudioTrans == null)
        {
            GameObject reloadObj = new GameObject("ReloadAudioSource");
            reloadObj.transform.SetParent(transform, false);
            reloadAudioSource = reloadObj.AddComponent<AudioSource>();
        }
        else
        {
            reloadAudioSource = reloadAudioTrans.GetComponent<AudioSource>();
        }
        reloadAudioSource.playOnAwake = false;
        reloadAudioSource.loop = false;
        reloadAudioSource.spatialBlend = 0.2f;
        reloadAudioSource.minDistance = 1;
        reloadAudioSource.maxDistance = 30;
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

        // 开火音源独立播放，不会被换弹音打断
        if (fireAudioClip != null)
        {
            fireAudioSource.PlayOneShot(fireAudioClip);
        }

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

        // 换弹独立音源
        if (reloadAudioClip != null)
        {
            reloadAudioSource.PlayOneShot(reloadAudioClip);
            reloadTime = reloadAudioClip.length;
        }

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