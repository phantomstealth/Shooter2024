using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// необходимо чтобы название скрипта и название класса совпадали
public class CustomCharacterController : MonoBehaviour{ 	
	public Animator anim;
    CharacterController controller;
    public float jumpForce = 3.5f; 
    public float walkingSpeed = 2f;
    public float runningSpeed = 6f;
    public float currentSpeed;
    public float gravity;
    private float animationInterpolation = 1f;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        // Прекрепляем курсор к середине экрана
        Cursor.lockState = CursorLockMode.Locked;
        // и делаем его невидимым
        Cursor.visible = false;
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
    void Run(float x, float y)
    {
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1.5f, Time.deltaTime * 3);
        anim.SetFloat("x", x * animationInterpolation);
        anim.SetFloat("y", y * animationInterpolation);

        currentSpeed = Mathf.Lerp(currentSpeed, runningSpeed, Time.deltaTime * 3);
    }
    void Walk(float x,float y)
    {
        // Mathf.Lerp - отвчает за то, чтобы каждый кадр число animationInterpolation(в данном случае) приближалось к числу 1 со скоростью Time.deltaTime * 3.
        // Time.deltaTime - это время между этим кадром и предыдущим кадром. Это позволяет плавно переходить с одного числа до второго НЕЗАВИСИМО ОТ КАДРОВ В СЕКУНДУ (FPS)!!!
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1f, Time.deltaTime * 3);
        anim.SetFloat("x", x * animationInterpolation);
        anim.SetFloat("y", y * animationInterpolation);

        currentSpeed = Mathf.Lerp(currentSpeed, walkingSpeed, Time.deltaTime * 3);
    }

    void CharacterMove(float x,float y)
    {
        direction = new Vector3(x, 0f, y);
        direction = transform.TransformDirection(direction) * currentSpeed;
        controller.Move(direction * Time.deltaTime);
    }

    private void GamingGravity()
    {
        if (!controller.isGrounded) gravity -= 5f * Time.deltaTime;
        else gravity = 10f;
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        // Устанавливаем поворот персонажа когда камера поворачивается 

        // Зажаты ли кнопки W и Shift?
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            // Зажаты ли еще кнопки A S D?
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                // Если да, то мы идем пешком
                Walk(x,y);
            }
            // Если нет, то тогда бежим!
            else
            {
                Run(x,y);
            }
        }
        // Если W & Shift не зажаты, то мы просто идем пешком
        else
        {
            Walk(x,y);
        }
        //Если зажат пробел, то в аниматоре отправляем сообщение тригеру, который активирует анимацию прыжка
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");
        }
        CharacterMove(x,y);
        GamingGravity();
    }
    // Update is called once per frame
    public void Jump()
    {
        // Выполняем прыжок по команде анимации.
        //rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}