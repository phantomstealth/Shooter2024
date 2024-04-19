using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdController : MonoBehaviour {

    public float speed = 2f;
    public float gravity;
    public float jumppower;

    Vector3 direction;
    CharacterController controller;
    Animator anim;
    bool Squat;
    bool Jump;
    bool AimMode;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMove();
        GamingGravity();
    }

    private void CharacterMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.C)) Squat = !Squat;
        if (Input.GetKeyDown(KeyCode.Space)) Jump = true; else Jump = false;
        //if (Input.GetKeyDown(KeyCode.)


        anim.SetFloat("Speed", z);
        anim.SetBool("Squat", Squat);
        if (Jump) anim.SetTrigger("Jump");


        if (z < 0.5) z = 0;
        if (controller.isGrounded)
        {
            direction = new Vector3(x, 0f, z);
            direction = transform.TransformDirection(direction) * speed;
        }
        direction.y -= gravity;
        controller.Move(direction * Time.deltaTime);
    }

    private void GamingGravity()
    {
        if (!controller.isGrounded) gravity -= 5f * Time.deltaTime;
        else gravity = 10f;
    }
}
