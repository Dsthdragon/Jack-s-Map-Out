using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class playerMovement : MonoBehaviour {

    public float walkSpeed = 2;
    public float runSpeed = 6;
    float turnSmoothVelocity;
    public float gravity = -12;
    float speedSmoothVelocity;

    public float turnSmoothTime = 0.2f;
    public float speedSmoothTime = 0.1f;

    public Button actionButton;

    float velocityY;
    float currentSpeed;

    Animator animator;
    Transform cameraT;
    CharacterController controller;
    public Collider actionHandler;

	// Use this for initialization
	void Start ()
    {
        animator = transform.Find("player_object").GetComponent<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent <CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        bool running = Input.GetKey(KeyCode.LeftShift);
        

        Move(inputDir, running);
        moveAnimation(inputDir, running);

    }

    public void Move(Vector2 inputDir, bool running)
    {
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded)
        {

            velocityY = 0;
        }

    }

    public void moveAnimation(Vector2 inputDir, bool running)
    {
        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f) * inputDir.magnitude;

        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
        actionButton.onClick.AddListener(takeAction);
        if (Input.GetKeyDown(KeyCode.E))
            takeAction();
    }

    void takeAction()
    {
        animator.SetTrigger("takeAction");
    }


    float GetModifiedSmoothTime(float smoothTime)
    {
        return smoothTime;
    }
}
