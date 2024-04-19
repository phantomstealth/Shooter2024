using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor.XR.LegacyInputHelpers;


public class ControllerUnit : MonoBehaviour
{
    public Text infoObjectTXT;

    [Header("Animation")]
    public Animator anim;
    private AnimatorClipInfo[] m_CurrentClipInfo;
    private float m_CurrentClipLength;
    private string m_ClipName;
    private float animationInterpolation = 1f;


    [Header("Base setup")]
    public float walkingSpeed = 3.0f;
    public float runningSpeed = 4.0f;
    public float jumpSpeed = 5.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 35.0f;

    [Header("Audio")]
    public AudioClip Walk_a;
    public AudioClip hitme_a;
    public AudioClip LandingAudioClip;


    [HideInInspector]
    public bool canMove = true;

    [Header("Character Controller")]
    CharacterController characterController;
    public Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;



    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;
    [Tooltip("What layers the character uses as ground")]
    //Слой должен быть выбран, иначе не работает по умолчанию Default слой
    public LayerMask GroundLayers;
    //public GameObject SphereGround;

    [Header("Настройки камеры")]
    public bool cameroOnPlayer = false;
    public GameObject CrossHair;
    public Camera playerCamera;
    [SerializeField]
    private float cameraXOffset = 1.41f;
    [SerializeField]
    private float cameraYOffset = 1.65f;
    [SerializeField]
    private float cameraZOffset = -2f;
    [SerializeField]
    private float cameraXRotation = 10f;
    public GameObject RaycastSphere;
    public bool debugDrawLine = true;

    private AudioSource source;


    void Awake()
    {
        //rSphere = Instantiate(RaycastSphere, gameObject.transform.position, gameObject.transform.rotation);
        playerCamera = Camera.main;
        anim = GetComponent<Animator>();
        //rig = GetComponent<Rig>();
        //playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y + cameraYOffset, transform.position.z);
        //playerCamera.transform.SetParent(transform);
    }
    void Start()
    {
        infoObjectTXT = GameObject.Find("textInfoObject").GetComponent<Text>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //GameObject CrossHair = GameObject.FindGameObjectWithTag("CrossHair");
        characterController = GetComponent<CharacterController>();
        source = GetComponent<AudioSource>();
        if (playerCamera != null & cameroOnPlayer)
        {
            // configure and make camera a child of player with 3rd person offset
            playerCamera.transform.position = new Vector3(transform.position.x + cameraXOffset, transform.position.y + cameraYOffset, transform.position.z + cameraZOffset);
            playerCamera.transform.SetParent(transform);
            playerCamera.transform.localEulerAngles = new Vector3(cameraXRotation, 0f, 0f);
            CrossHair.SetActive(false);
            //canvasCross = GameObject.FindGameObjectWithTag("Cross");
            //canvasCross.GetComponent<Image>().enabled = true;
        }
        //else
            //Debug.LogWarning("PlayerCamera: Could not find a camera in scene with 'MainCamera' tag.");

        // spawnobject = GetComponent<SimpleSpawn>();
        //spawnobject = GameObject.Find("World").GetComponent<SpawnObjects>();
    }


    // Start is called before the first frame update
    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
        //VerifyGravity.transform.position = spherePosition;
        // update animator if using character
        anim.SetBool("Grounded", Grounded);
        //Проверяем зрительно с помощью сферы где происходит проверка соприкосновения слоев. Player должен быть слоем другим чем Default
        //SphereGround.transform.position = spherePosition;
   }

    void PlayAudio(AudioClip clip, bool loop)
    {
        source.loop = loop;
        source.clip = clip;
        source.Play();
    }


    public void OnLand(AnimationEvent animationEvent)
    {
        //Debug.Log("Try PlaySound"+animationEvent.animatorClipInfo.weight);
        //if (animationEvent.animatorClipInfo.weight > 0.4f)
        {
            //AudioSource.PlayClipAtPoint(LandingAudioClip, transform.position, FootstepAudioVolume);
            PlayAudio(LandingAudioClip, false);
            Debug.Log("PlaySound - Ok");
        }
    }

    private bool GetCurrentAnimatorByName(string nameAnimator)
    {
        //hitme_bool = anim.GetCurrentAnimatorStateInfo(0).IsName("HitToBody");
        m_CurrentClipInfo = this.anim.GetCurrentAnimatorClipInfo(0);
        //Длина клипа в миллисекундах
        m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
        //Имя клипа текущей анимации
        m_ClipName = m_CurrentClipInfo[0].clip.name;
        return (nameAnimator == m_ClipName);
    }

    void Run(float x, float y)
    {
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1.5f, Time.deltaTime * 3);
        anim.SetFloat("x", x * animationInterpolation);
        anim.SetFloat("y", y * animationInterpolation);
    }


    void Walk(float x, float y)
    {
        // Mathf.Lerp - отвчает за то, чтобы каждый кадр число animationInterpolation(в данном случае) приближалось к числу 1 со скоростью Time.deltaTime * 3.
        // Time.deltaTime - это время между этим кадром и предыдущим кадром. Это позволяет плавно переходить с одного числа до второго НЕЗАВИСИМО ОТ КАДРОВ В СЕКУНДУ (FPS)!!!
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1f, Time.deltaTime * 3);
        anim.SetFloat("x", x * animationInterpolation);
        anim.SetFloat("y", y * animationInterpolation);
    }

    void HitMe(int maxDamage)
    {
        Debug.Log("Hitme");
        //RpcHitMe();
        //PlayAudio(hitme_a, false);
        //health = health - maxDamage;
        //if (health < 0) health = 0;
        //if (health == 0)
        {
            //Dying_Unit();
            //Если здоровье меньше нуля удаляем объект с поля
            //NetworkServer.Destroy(gameObject); 
        }
    }


    private void movePersonal()
    {
        bool isRunning = false;
        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");


        // We are grounded, so recalculate move direction based on axis
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * y : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * x : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        //Проверяем если есть попадание в тело останавливаем все скоростив ноль
        if (GetCurrentAnimatorByName("HitToBody") || GetCurrentAnimatorByName("Dying")) moveDirection = new Vector3(0, 0, 0);

        anim.SetFloat("Speed", (Math.Max(Math.Abs(curSpeedX), Math.Abs(curSpeedY))));

        if (Input.GetKey(KeyCode.W) && isRunning)
        {
            // Зажаты ли еще кнопки A S D?
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                //anim.SetFloat("Speed", 0.5f);
                // Если да, то мы идем пешком
                Walk(x, y);
            }
            // Если нет, то тогда бежим!
            else
            {
                //anim.SetFloat("Speed", 1f);
                Run(x, y);
            }
        }
        // Если W & Shift не зажаты, то мы просто идем пешком
        else
        {
            //anim.SetFloat("Speed", 0.5f);
            Walk(x, y);
        }


        //Проверяем анимацию попадания
        if (Input.GetKeyDown(KeyCode.Y)) HitMe(5);


        //анимация прыжка
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");
        }


        if (Input.GetButtonDown("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            //anim.SetTrigger("Jump");
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove && playerCamera != null)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    void InfoAboutGameObject(GameObject game_object)
    {
        if (game_object.GetComponent<Info_Object>() != null)
        {
            infoObjectTXT.text = game_object.GetComponent<Info_Object>().nameObject+" "+game_object.GetComponent<Info_Object>().healthObject+"%";
        }
        else
            infoObjectTXT.text = "";
    }

    void CheckRaycast()
    {
        RaycastHit hit;
        RaycastHit hitUI;
        Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1000, 1);
        Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitUI, 1000, 5);

        float distanceHit;
        Vector3 hitTrue;
        if (hit.transform != null)
        {
            if (debugDrawLine) Debug.DrawLine(playerCamera.transform.position, hit.point, Color.yellow);
            hitTrue = hit.point;
            distanceHit = Vector3.Distance(playerCamera.transform.position, hit.point);
            RaycastSphere.transform.position = hit.point;
            InfoAboutGameObject(hit.transform.gameObject);
        }
        else
        {
            if (debugDrawLine) Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * 1000, Color.green);
            RaycastSphere.transform.position = playerCamera.transform.position + playerCamera.transform.forward * 1000;
            infoObjectTXT.text = "";
        }
    }


    // Update is called once per frame
    void Update()
    {
        GroundedCheck();
        if (!GetCurrentAnimatorByName("Dying")) movePersonal();
        CheckRaycast();
    }
}
//Что делать при добавлении персонажа
//Создаем точку StartBullet