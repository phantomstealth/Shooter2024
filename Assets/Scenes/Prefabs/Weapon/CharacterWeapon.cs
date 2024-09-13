using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;
using UnityEngine.UI;


public class CharacterWeapon : MonoBehaviour
{
    public Text countButtonTXT;

    [Header("Audio")]
    public AudioClip shot;
    public AudioClip emptyClip;
    public AudioClip reloadClip;

    private AudioSource source;

    [Header("Weapon_Characteristics")]
    public float weaponRange = 1000f;
    public bool automaticGun;
    public int maxClipAmmo;
    public int currentAmmo;
    public int currentClipAmmo;
    public int maxDamage;

    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        countButtonTXT = GameObject.Find("numBullet").GetComponent<Text>();
        source = gameObject.GetComponent<AudioSource>();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 100, 30), "Ammo: " + currentClipAmmo + " / " + currentAmmo);
    }


    void PlayAudio(AudioClip clip, bool loop)
    {
        source.loop = loop;
        source.clip = clip;
        source.Play();
    }

    public bool Shot()
    {
        if (currentClipAmmo <= 0)
        {
            PlayAudio(emptyClip, false);
            return false;
        }
        PlayAudio(shot, false);
        currentAmmo = currentAmmo - 1;
        currentClipAmmo = currentClipAmmo - 1;
        syncWeapon();
        return true;
    }

    void syncWeapon()
    {
        int numCurWeapon = Player.GetComponent<ControllerWeapon>().numCurrentWeapon;
        Player.GetComponent<ControllerWeapon>().currentArsenal.AmmoTotal = currentAmmo;
        Player.GetComponent<ControllerWeapon>().currentArsenal.AmmoIntoClip = currentClipAmmo;
        Player.GetComponent<ControllerWeapon>().arsenal[numCurWeapon].AmmoTotal = currentAmmo;
        Player.GetComponent<ControllerWeapon>().arsenal[numCurWeapon].AmmoIntoClip = currentClipAmmo;
    }
    void Reload_Weapon()
    {
        if (currentAmmo <= 0 || maxClipAmmo < currentClipAmmo) return;
        if (maxClipAmmo < currentAmmo)
        {
            currentClipAmmo = maxClipAmmo;
            syncWeapon();
        }
        else currentClipAmmo = currentAmmo;
        PlayAudio(reloadClip, false);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Reload_Weapon();
        countButtonTXT.text = currentClipAmmo.ToString()+"/"+currentAmmo;
    }
}
