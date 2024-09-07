using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;

public class Turret : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip shot;
    public AudioClip startRotate;
    public AudioClip stopRotate;
    public AudioSource source;
    public bool rotateSound;
    public bool blnRotateCurrent;
    public float prevYTransform;

    public GameObject startBullet;
    [Header("Скорость вращения башни от 1 до 100")]
    public float SpeedRotateTurret=100f;
    private RaycastHit curDirectionWeapon;
    private RaycastHit hitDirectionTarget;

    private Vector3 direction;
    private Transform TargetDirection;



    // Start is called before the first frame update
    void Start()
    {
        source=gameObject.GetComponent<AudioSource>(); 
        if (GameObject.FindGameObjectWithTag("Player") != null)
            TargetDirection = GameObject.FindGameObjectWithTag("Player").transform;
        else
            Debug.Log("Не найден экземпляр игрока");
    }

    void RotateForPlayer(bool Instantly)
    {
        direction = TargetDirection.transform.position - gameObject.transform.position;
        direction.y = 0;
        if (Instantly)
        {
            transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.LookRotation(direction), SpeedRotateTurret / 30);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void VerifySoundRotation()
    {
        if (transform.rotation.y != prevYTransform)
        {
            if (!blnRotateCurrent &! source.isPlaying)
            {
                PlayAudio(startRotate, false);
            }
            prevYTransform = transform.rotation.y;
            blnRotateCurrent = true;
        }
        else
        {
            if (blnRotateCurrent)
            {
                PlayAudio(stopRotate, false);
            }
            blnRotateCurrent = false;

        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = GetDirection(startBullet.transform.forward);
        //определяем куда направлена башня, рисуем на сцене направление красной полосой
        Physics.Raycast(startBullet.transform.position, startBullet.transform.forward, out curDirectionWeapon, 1000, 1);
        if (curDirectionWeapon.transform != null)
        {
            Debug.DrawLine(startBullet.transform.position, curDirectionWeapon.point, Color.red);
        }
        else
        {
            Debug.DrawLine(startBullet.transform.position, startBullet.transform.position + startBullet.transform.forward * 1000, Color.red);
        }
        //определяем в каком направлении находится цель рисуем зеленой полосой
        VerifySoundRotation();
        if (TargetDirection!= null)
        {
            Debug.DrawLine(transform.position, TargetDirection.transform.position, Color.green);
            RotateForPlayer(true);
        }
    }
    void PlayAudio(AudioClip clip, bool loop)
    {
        source.loop = loop;
        source.clip = clip;
        source.Play();
    }

    private Vector3 GetDirection(Vector3 direction)
    {
        direction.Normalize();
        return direction;
    }

}
