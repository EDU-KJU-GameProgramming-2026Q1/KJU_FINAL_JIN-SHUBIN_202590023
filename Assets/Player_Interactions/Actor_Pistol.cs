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

    [Header("Audio Sound Options")]
    public AudioClip fireAudioClip;
    public AudioClip reloadAudioClip;
    private AudioSource fireAudioSource;
    private AudioSource reloadAudioSource;

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

        // 开火音源
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

        // 换弹音源
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
            reloadTipText.text = "Pistol Reloading...";

        if (crosshair != null) crosshair.SetActive(false);

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