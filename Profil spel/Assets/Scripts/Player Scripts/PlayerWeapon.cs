using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Weapon currentWeapon;  // Reference to the current weapon

    private int currentAmmo;

    void Start()
    {
        if (currentWeapon != null)
        {
            currentAmmo = currentWeapon.maxAmmo;  // Initialize ammo
        }
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public void UseAmmo(int amount)
    {
        currentAmmo -= amount;
        if (currentAmmo < 0) currentAmmo = 0;
    }

    public bool CanReload()
    {
        return currentAmmo < currentWeapon.maxAmmo;
    }

    public void Reload()
    {
        currentAmmo = currentWeapon.maxAmmo;
    }

    public void SwitchWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        currentAmmo = currentWeapon.maxAmmo;  // Reset ammo when switching
    }

    public void PickupWeapon(Weapon weapon)
    {
        SwitchWeapon(weapon);
    }
}
