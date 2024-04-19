using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public string nameWeapon;
    public GameObject startBullet;
    public bool debugDrawLine_Weapon = true;
    public tLayer nameLayer;
    private CharacterWeapon charWeapon;
    private GameObject target_object;

    public enum tLayer
    {
        Rifle_Aim,
        Pistol_Aim
    }
    public enum tWeapon
    { 
        None,
        Pistol,
        Rifle
    }
    [SerializeField]
    public tWeapon typeWeaponList;

    private GameObject trail;
    [SerializeField]
    private GameObject ImpactParticleSystem;

    [SerializeField]
    private LayerMask Mask;

    [SerializeField]
    private bool AddBulletSpread = true;
    [SerializeField]
    private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField]
    private GameObject BulletTrail;
    [SerializeField]
    private float BulletSpeed = 100;


    private float LastShootTime;
    [SerializeField]
    private float ShootDelay = 0.5f;

    private Vector3 startPosition;
    private float distance;
    private float remainingDistance;
    private Vector3 hitPoint;
    private bool madeImpact;
    private Vector3 hitNormal;

    private RaycastHit hitWeapon;


    private void Start()
    {
        charWeapon =gameObject.GetComponent<CharacterWeapon>();
    }

    // Start is called before the first frame update
    void CheckRaycast()
    {
        Physics.Raycast(startBullet.transform.position, startBullet.transform.forward, out hitWeapon, 1000, 1);

        if (hitWeapon.transform != null)
        {
            if (debugDrawLine_Weapon) Debug.DrawLine(startBullet.transform.position, hitWeapon.point, Color.red);
        }
        else
        {
            if (debugDrawLine_Weapon) Debug.DrawLine(startBullet.transform.position, startBullet.transform.position + startBullet.transform.forward * 1000, Color.red);
        }
    }

    private void cmdTrail_Create(Vector3 point, Vector3 normal, bool MadeImpact)
    {
        trail = Instantiate(BulletTrail, startBullet.transform.position, startBullet.transform.rotation);
        //NetworkServer.Spawn(trail);
        startPosition = trail.transform.position;
        distance = Vector3.Distance(trail.transform.position, point);
        remainingDistance = distance;
        hitPoint = point;
        hitNormal = normal;
        madeImpact = MadeImpact;
    }


    public void Shoot()
    {
        if (LastShootTime + ShootDelay < Time.time)
        {
            if (!charWeapon.Shot()) return;
            Vector3 direction = GetDirection(startBullet.transform.forward);

            //Debug.DrawLine(BulletSpawnPoint.transform.position, BulletSpawnPoint.transform.position + direction * 1000, Color.cyan);

            if (Physics.Raycast(startBullet.transform.position, direction, out RaycastHit hit, float.MaxValue, Mask))
            {
                cmdTrail_Create(hit.point, hit.normal, true);
                LastShootTime = Time.time;
                target_object = hit.transform.gameObject;
            }
            else
            {
                cmdTrail_Create(startBullet.transform.position + GetDirection(startBullet.transform.forward) * 100, Vector3.zero, false);
                LastShootTime = Time.time;
                target_object = null;
            }
        }
    }

    private Vector3 GetDirection(Vector3 direction)
    {
        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
            );

            direction.Normalize();
        }

        return direction;
    }

    void DestroySelf()
    {
        Destroy(trail, trail.GetComponent<TrailRenderer>().time);
    }

    private void DamageObject(int damage)
    { 
        if (target_object.GetComponent<Info_Object>() != null)
        {
            target_object.GetComponent<Info_Object>().Damage(damage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Mouse0)) Shoot();

        CheckRaycast();
        if (trail != null)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, 1 - (remainingDistance / distance));
            remainingDistance -= BulletSpeed * Time.deltaTime;
            if (remainingDistance <= 0 & trail.transform.position != hitPoint)
            {
                trail.transform.position = hitPoint;
                if (madeImpact)
                {
                    GameObject ImpactGo = Instantiate(ImpactParticleSystem, hitPoint, Quaternion.LookRotation(hitNormal));
                    ImpactGo.transform.SetParent(hitWeapon.transform.gameObject.transform);
                    madeImpact = false;
                    DamageObject(charWeapon.maxDamage);
                }
                DestroySelf();
            }
        }

    }
}
