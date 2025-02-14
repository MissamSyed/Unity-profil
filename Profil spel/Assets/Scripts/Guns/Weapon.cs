using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName = "DefaultWeapon";  // Name of the weapon
    public int maxAmmo = 30;  // Maximum ammo the weapon can hold
    public float reloadTime = 1.5f;  // Time it takes to reload
    public float fireRate = 0.1f;  // Time between shots (semi-auto)
    public int damage = 25;  // Damage per shot
    public string shootTrigger = "Shoot";  // Animator trigger for shooting
    public string reloadTrigger = "Reload";  // Animator trigger for reloading
}
