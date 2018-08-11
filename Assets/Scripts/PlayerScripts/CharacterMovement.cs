 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    private MovementMotor motor;

    public float move_Magnitude = 0.5f;
    public float speed = 0.7f;
    public float speed_Move_WhileAttack = 0.1f;
    public float speed_Attack = 1.5f;
    public float turnSpeed = 10f;
    public float speed_Jump = 20f;

    private float speed_Move_Multiplier = 1f;

    private Vector3 direction;

    private Animator animator;
    private Camera mainCamera;

    private string PARAMETER_STATE = "State";

    void Awake()
    {
        motor = GetComponent<MovementMotor>();
        //animator = GetComponent<Animator>();
    }

    void Start () {
        mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        MovementAndJumping();
	}

    private Vector3 MoveDirection {
        get { return direction; }

        set {
            direction = value * speed_Move_Multiplier;

            if (direction.magnitude > 0.1f) {
                var newRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
            }

            direction *= speed * (Vector3.Dot(transform.forward, direction) + 1f) * 5f;   
            motor.Move(direction);
        }
    }

    void Moving(Vector3 dir, float mult) {
        speed_Move_Multiplier = 1 * mult;
        MoveDirection = dir;
    }

    void Jump() {
        motor.Jump(speed_Jump);
    }

    void MovementAndJumping() {
        Vector3 moveInput = Vector3.zero;
        Vector3 forward = Quaternion.AngleAxis(-90, Vector3.up) * mainCamera.transform.right;

        moveInput += forward * Input.GetAxis("Vertical");
        moveInput += mainCamera.transform.right * Input.GetAxis("Horizontal");

        moveInput.Normalize();
        Moving(moveInput.normalized, 1f);

        if (Input.GetKey (KeyCode.Space)) {
            Jump();
        }
    }
}
