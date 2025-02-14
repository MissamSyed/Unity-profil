using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Animator playerAnimator;
    private Weapon currentWeapon; // Reference to the current weapon
    private GameObject currentWeaponObject; // Reference to the current weapon GameObject

    [SerializeField] private GameObject[] weaponPrefabs; // Array to store the different weapon prefabs (AK47, MG42, MP40)

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        EquipWeapon(0); // Start with the first weapon (AK47, for example)
    }

    void Update()
    {
        // Switch weapons (this is just an example, you could use any input method you prefer)
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Switch to AK47 (index 0)
        {
            EquipWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Switch to MG42 (index 1)
        {
            EquipWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Switch to MP40 (index 2)
        {
            EquipWeapon(2);
        }
    }

    private void EquipWeapon(int weaponIndex)
    {
        // Destroy the old weapon if it exists
        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        // Instantiate the new weapon
        currentWeaponObject = Instantiate(weaponPrefabs[weaponIndex], transform.position, Quaternion.identity);
        currentWeapon = currentWeaponObject.GetComponent<Weapon>();

        // Set the idle animation for the new weapon
        playerAnimator.SetTrigger(currentWeapon.IdleAnimationTrigger);
    }
}
