using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private string weaponName;  // Name of the weapon (e.g., "AK47", "MG42", etc.)
    [SerializeField] private string shootAnimationTrigger; // Trigger for shoot animation
    [SerializeField] private string reloadAnimationTrigger; // Trigger for reload animation
    [SerializeField] private string idleAnimationTrigger; // Trigger for idle animation

    // Properties to access the weapon's data
    public string WeaponName => weaponName;
    public string ShootAnimationTrigger => shootAnimationTrigger;
    public string ReloadAnimationTrigger => reloadAnimationTrigger;
    public string IdleAnimationTrigger => idleAnimationTrigger; // Added idle trigger
}
