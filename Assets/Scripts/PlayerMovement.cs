using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    


    [Header("Move Variables")]
    public float speed = 12.0f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3.0f;

    [Header("Gravity")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isJumping = false;
    bool isFalling = false;
    bool isCrouching = false;
    bool isSprinting = false;
    bool isWalking = false;
    bool isAiming = false;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    [Header("Animations")]
    private Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        anim = GetComponentInChildren<Animator>();

        AimDownSide();
        AimingActions();

        // Kiểm tra xem player có đang đứng trên mặt đất không
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // Nếu đang đứng trên mặt đất và vận tốc y < 0
        if (isGrounded && velocity.y < 0)
        {
            // Đặt vận tốc y về 0
            velocity.y = -2f;
        }

        // Nhận giá trị trục X và Z của bàn phím
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Tạo vector di chuyển
        Vector3 move = transform.right * x + transform.forward * z;

        // Di chuyển player
        controller.Move(move * speed * Time.deltaTime);

        // Nếu player đang đứng trên mặt đất
        if (isGrounded)
        {
            isFalling = false;
            // Nếu player nhấn phím Space
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                // Tính vận tốc nhảy
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // Tính vận tốc rơi 
        isFalling = true;
        isJumping = false;

        velocity.y += gravity * Time.deltaTime;

        // Di chuyển player theo vận tốc
        controller.Move(velocity * Time.deltaTime);

        // Nếu player di chuyển
        if (lastPosition != gameObject.transform.position && isAiming == false)
        {
            isWalking = true;
            Debug.Log("Walking");

            // Nếu player nhấn phím LeftShift
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // animator.SetBool("isSprinting", true);
                isSprinting = true;
                Debug.Log("Sprinting");

                // Tăng tốc độ di chuyển
                speed = 10.0f;

                anim.SetFloat("Speed", 1.0f);
            }
            else
            {
                // animator.SetBool("isSprinting", false);
                isSprinting = false;

                // Đặt tốc độ di chuyển về mặc định
                speed = 5.0f;

                anim.SetFloat("Speed", 0.0f);
            }

            lastPosition = gameObject.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && isAiming == false)
        {
            Debug.Log("Shooting");
            anim.SetTrigger("Shoot");
        }

        if (Input.GetKeyDown(KeyCode.R) && isAiming == false)
        {
            Debug.Log("Reloading");
            anim.SetTrigger("Reload");
        }
    }

    private void AimDownSide()
    {
        
        if (Input.GetMouseButton(1))
        {
            isAiming = true;  
            Debug.Log("isAiming");
            anim.SetBool("isAiming", true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            Debug.Log("Not Aiming");
            anim.SetBool("isAiming", false);
        }
    }

    private void AimingActions()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isAiming == true)
        {
            Debug.Log("Shooting");
            anim.SetTrigger("Shoot");
        }

        if (Input.GetKeyDown(KeyCode.R) && isAiming == true)
        {
            Debug.Log("Reloading");
            anim.SetTrigger("Reload");
        }

        if (lastPosition != gameObject.transform.position && isAiming == true)
        {
            isWalking = true;
            Debug.Log("Walking While Aiming");

            anim.SetFloat("Aiming Speed", 1.0f);

            lastPosition = gameObject.transform.position;
        }

        else 
        {
            isWalking = false;
            anim.SetFloat("Aiming Speed", 0.0f);
        }
    }
}
