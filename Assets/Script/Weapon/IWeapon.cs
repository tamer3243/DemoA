using UnityEngine;

public interface IWeapon
{
    string WeaponName { get; }
    void Attack();
    void Equip(Transform handTransform);
    void Unequip();
}
