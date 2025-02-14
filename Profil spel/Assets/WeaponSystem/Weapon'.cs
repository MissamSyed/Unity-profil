using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string shootAnimationTrigger;
    public string reloadAnimationTrigger;
    public float fireRate;
    public int magazineSize;
    public int totalAmmo;
    public float reloadTime;
    public GameObject weaponObject;
}
