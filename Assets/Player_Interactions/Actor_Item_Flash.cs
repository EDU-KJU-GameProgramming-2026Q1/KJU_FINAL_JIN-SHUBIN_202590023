using UnityEngine;

public class Actor_Item_Flash : InterfaceBase_IItem
{
    [Header("Flashlight Settings")]
    public Light spotLight;

    private void Start()
    {
        OnStopUse();
    }
    public override void OnUse()
    {
        spotLight.enabled = true;
        Debug.Log($"Flash Light {(spotLight.enabled ? "On" : "Off")}");
    }

    public override void OnStopUse()
    {
        spotLight.enabled = false;
        Debug.Log($"Flash Light {(spotLight.enabled ? "On" : "Off")}");
    }
}