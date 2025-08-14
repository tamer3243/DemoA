using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private WeaponBase[] availableWeapons = new WeaponBase[2]; // Giới hạn 2 slot
    [SerializeField] private WeaponBase currentWeapon;

    private int currentWeaponIndex = 0;

    private void Start()
    {
        EquipWeapon(currentWeaponIndex);
    }

    private void Update()
    {
        if (InputPc.Instance.IsSwapPressed())
        {
            SwapWeapon();
        }
    }

    public void EquipWeapon(int index)
    {
        if (index >= 0 && index < availableWeapons.Length)
        {
            if (currentWeapon != null)
                currentWeapon.gameObject.SetActive(false);

            currentWeaponIndex = index;
            currentWeapon = availableWeapons[index];

            if (currentWeapon != null)
                currentWeapon.gameObject.SetActive(true);
        }
        currentWeapon.gameObject.SetActive(true);
    }

    public void SwapWeapon()
    {
        int nextIndex = (currentWeaponIndex + 1) % availableWeapons.Length;
        currentWeapon.gameObject.SetActive(false);
        EquipWeapon(nextIndex);
    }

    public void Attack()
    {
        currentWeapon?.Attack();
    }

    public void StopShooting()
    {

    }

    public WeaponBase GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
