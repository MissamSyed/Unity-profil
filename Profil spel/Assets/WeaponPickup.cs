using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private Weapon weaponData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerWeapon playerWeapon = other.GetComponent<PlayerWeapon>();

            if (playerWeapon != null)
            {
                playerWeapon.EquipWeapon(weaponData);
                Destroy(gameObject); // Remove the pickup from the scene
            }
        }
    }
}
