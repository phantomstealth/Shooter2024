using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerWeapon : MonoBehaviour
{
    public GameObject currentWeapon;
    public Weapon.tWeapon typeWeapon = 0;
    public GameObject branchRifle;
    public GameObject branchPistol;
    public Arsenal[] arsenal;
    private Animator anim;

    private GameObject weaponInHand;

    int baseLayerAnim;
    int pistolLayerAnim;
    int rifleLayerAnim;

    [System.Serializable]
    public struct Arsenal
    {
        public GameObject Weapon;
        public int Ammo;
    }

    private void Awake()
    {
    }
    void Start()
    {
        anim = gameObject.GetComponent<ControllerUnit>().anim;
        baseLayerAnim = anim.GetLayerIndex("Base Layer");
        pistolLayerAnim = anim.GetLayerIndex("Pistol_Aim");
        rifleLayerAnim = anim.GetLayerIndex("Rifle_Aim");
        for (int i = 0; i <= 3; i++)
        {
           if (arsenal[i].Weapon!=null) arsenal[i].Ammo = arsenal[i].Weapon.GetComponent<CharacterWeapon>().currentAmmo;
           Debug.Log(i);
        }

    }

    private void ChangeWeapon(int numWeapon)
    {
        currentWeapon = arsenal[numWeapon].Weapon;
        if (currentWeapon != null)
        {
            typeWeapon = currentWeapon.GetComponent<Weapon>().typeWeaponList;
        }
        else
        {
            typeWeapon = Weapon.tWeapon.None;
        }
        if (typeWeapon == Weapon.tWeapon.Pistol)
        {
            branchPistol.SetActive(true);
            branchRifle.SetActive(false);
            Destroy(weaponInHand);
            weaponInHand = Instantiate(currentWeapon,branchPistol.transform);
            anim.SetLayerWeight(pistolLayerAnim, 1);
            anim.SetLayerWeight(rifleLayerAnim, 0);

        }
        else if (typeWeapon == Weapon.tWeapon.Rifle)
        {
            branchRifle.SetActive(true);
            branchPistol.SetActive(false);
            Destroy(weaponInHand);
            weaponInHand = Instantiate(currentWeapon,branchRifle.transform);
            anim.SetLayerWeight(pistolLayerAnim, 0);
            anim.SetLayerWeight(rifleLayerAnim, 1);

        }
        else
        {
            Destroy(weaponInHand);
            branchRifle.SetActive(false);
            branchPistol.SetActive(false);
            anim.SetLayerWeight(pistolLayerAnim, 0);
            anim.SetLayerWeight(rifleLayerAnim, 0);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) ChangeWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeWeapon(3);

    }
}
