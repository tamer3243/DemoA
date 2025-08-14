using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] protected string weaponName;
    [SerializeField] protected Animator animator;
  
    public string WeaponName => weaponName;

    public virtual void Equip(Transform handTransform)
    {
        transform.SetParent(handTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        gameObject.SetActive(true);
    }

    public virtual void Unequip()
    {
        gameObject.SetActive(false);
    }

    public abstract void Attack();
}

