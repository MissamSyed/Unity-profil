using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLootin : MonoBehaviour
{
    public GameObject weaponDrop; 

    private bool isDead = false;

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        DropWeapon();
        Destroy(gameObject); 
    }

    void DropWeapon()
    {
        if (weaponDrop != null)
        {
            Instantiate(weaponDrop, transform.position, Quaternion.identity);
        }
    }
}
