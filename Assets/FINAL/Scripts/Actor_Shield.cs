using UnityEngine;
using TMPro;

public class Actor_Shield : InterfaceBase_IItem
{
    [Header("Shield Settings")]
    public float maxDurability = 100f;

    [Header("UI")]
    public TextMeshProUGUI durabilityText;
    public TextMeshProUGUI statusText;
    public GameObject shieldModel;

    private float currentDurability;
    private bool isBlocking = false;

    void Start()
    {
        currentDurability = maxDurability;
        UpdateUI();

        if (shieldModel != null)
            shieldModel.SetActive(true);

        if (statusText != null)
            statusText.text = "";
    }

    void UpdateUI()
    {
        if (durabilityText != null)
            durabilityText.text = $"Shield: {Mathf.Round(currentDurability)} / {maxDurability}";
    }

    public override void OnUse()
    {
        if (currentDurability <= 0) return;
        isBlocking = true;
        if (statusText != null) statusText.text = "Blocking...";
    }

    public override void OnStopUse()
    {
        isBlocking = false;
        if (statusText != null) statusText.text = "";
    }

    void Update() { }

    public bool IsBlockActive()
    {
        return isBlocking && currentDurability > 0;
    }

    public void TakeShieldDamage(float damage)
    {
        currentDurability -= damage;
        UpdateUI();

        if (currentDurability <= 0)
        {
            currentDurability = 0;
            isBlocking = false;
            if (statusText != null) statusText.text = "Shield Broken!";
        }
    }
}