using UnityEngine;

public class Actor_Item_Pistol : InterfaceBase_IItem
{
    public override void OnUse()
    {
        Debug.Log("Pistol Shoot!");
    }
}