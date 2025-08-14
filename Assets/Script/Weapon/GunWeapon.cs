using UnityEngine;

public class GunWeapon : WeaponBase
{
    [Header("Projectile Settings")]
    [SerializeField] private EffectBase bullet;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireForce = 20f;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float maxRange = 100f;

    [Header("Recoil Settings")]
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private float recoilKickback = 0.1f;
    [SerializeField] private float recoilUp = 2f;
    [SerializeField] private float recoilRecoverySpeed = 10f;
    private float lastFireTime;
    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;

    private Vector3 currentRecoilPos;
    private Quaternion currentRecoilRot;
    ObjectPooler<EffectBase> bullets;
    private Camera mainCam;

    private void Start()
    {
        if (weaponTransform == null)
            weaponTransform = transform;

        originalLocalPos = weaponTransform.localPosition;
        originalLocalRot = weaponTransform.localRotation;   
        mainCam = Camera.main;
        bullets = new ObjectPooler<EffectBase>(bullet, 100);
    }
  
    public override void Attack()
    {
        if (Time.time - lastFireTime < fireRate) return;

        Vector3 targetPoint = GetTargetPoint();

        // Tính hướng từ firePoint đến target
        Vector3 shootDir = (targetPoint - firePoint.position).normalized;

        // Spawn projectile từ pool
        var bullet =bullets.Get(firePoint.position, Quaternion.LookRotation(shootDir));
        bullet.FIRE();

        ApplyRecoil();
        lastFireTime = Time.time;
    }

    private Vector3 GetTargetPoint()
    {
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * maxRange, Color.blue, 0.1f);

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, maxRange))
        {
            targetPoint = hit.point;
            // Chỉ tô đỏ nếu trúng enemy
            Color lineColor = hit.collider.CompareTag("Enemy") ? Color.red : Color.magenta;
            Debug.DrawLine(firePoint.position, targetPoint, lineColor, 0.1f);
        }
        else
        {
            targetPoint = ray.GetPoint(maxRange);
            Debug.DrawLine(firePoint.position, targetPoint, Color.yellow, 0.1f);
        }

        return targetPoint;
    }


    private void ApplyRecoil()
    {
        currentRecoilPos -= Vector3.forward * recoilKickback;
        currentRecoilRot *= Quaternion.Euler(-recoilUp, 0, 0);
    }

    private void Update()
    {
        // Mượt hóa recoil trở về vị trí ban đầu
        weaponTransform.localPosition = Vector3.Lerp(
            weaponTransform.localPosition,
            originalLocalPos + currentRecoilPos,
            Time.deltaTime * recoilRecoverySpeed
        );

        weaponTransform.localRotation = Quaternion.Slerp(
            weaponTransform.localRotation,
            originalLocalRot * currentRecoilRot,
            Time.deltaTime * recoilRecoverySpeed
        );

        // Giảm dần recoil để không cộng dồn mãi
        currentRecoilPos = Vector3.Lerp(currentRecoilPos, Vector3.zero, Time.deltaTime * recoilRecoverySpeed);
        currentRecoilRot = Quaternion.Slerp(currentRecoilRot, Quaternion.identity, Time.deltaTime * recoilRecoverySpeed);
    }

    // Vẽ icon FirePoint trong Scene
    private void OnDrawGizmos()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(firePoint.position, 0.02f);
        }
    }
}
