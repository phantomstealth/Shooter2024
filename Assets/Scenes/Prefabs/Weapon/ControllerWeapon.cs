using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ControllerWeapon : MonoBehaviour
{
    public GameObject currentWeapon;
    public Arsenal currentArsenal;
    public Weapon.tWeapon typeWeapon = 0;
    public GameObject branchRifle;
    public GameObject branchPistol;
    public Arsenal[] arsenal;
    private Animator anim;

    public RigBuilder rigbuilder;

    private GameObject weaponInHand;

    int baseLayerAnim;
    int pistolLayerAnim;
    int rifleLayerAnim;
    public int numCurrentWeapon;

    [System.Serializable]
    public struct Arsenal
    {
        public GameObject Weapon;
        public int AmmoIntoClip;
        public int AmmoTotal;
    }

    void Start()
    {
        anim = gameObject.GetComponent<ControllerUnit>().anim;
        baseLayerAnim = anim.GetLayerIndex("Base Layer");
        pistolLayerAnim = anim.GetLayerIndex("Pistol_Aim");
        rifleLayerAnim = anim.GetLayerIndex("Rifle_Aim");
        rigbuilder = GetComponent<RigBuilder>();
    }

    private void ChangeWeapon(int numWeapon)
    {
        List<RigLayer> layers = rigbuilder.layers;
        numCurrentWeapon = numWeapon;
        currentWeapon = arsenal[numWeapon].Weapon;
        currentArsenal = arsenal[numWeapon];
        if (currentWeapon != null)
        {
            typeWeapon = currentWeapon.GetComponent<Weapon>().typeWeaponList;
            currentWeapon.GetComponent<CharacterWeapon>().currentAmmo = arsenal[numWeapon].AmmoTotal;
            currentWeapon.GetComponent<CharacterWeapon>().currentClipAmmo = arsenal[numWeapon].AmmoIntoClip;
            currentWeapon.GetComponent<CharacterWeapon>().Player = gameObject;
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
            layers[1].active = true;
            layers[0].active = false;
        }
        else if (typeWeapon == Weapon.tWeapon.Rifle)
        {
            branchRifle.SetActive(true);
            branchPistol.SetActive(false);
            Destroy(weaponInHand);
            weaponInHand = Instantiate(currentWeapon,branchRifle.transform);
            anim.SetLayerWeight(pistolLayerAnim, 0);
            anim.SetLayerWeight(rifleLayerAnim, 1);
            layers[0].active = true;
            layers[1].active = false;

        }
        else
        {
            Destroy(weaponInHand);
            branchRifle.SetActive(false);
            branchPistol.SetActive(false);
            anim.SetLayerWeight(pistolLayerAnim, 0);
            anim.SetLayerWeight(rifleLayerAnim, 0);
            layers[0].active = true;
            layers[1].active = false;
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
