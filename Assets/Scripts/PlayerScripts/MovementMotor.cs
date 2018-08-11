using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMotor : MonoBehaviour {

    public float gravityMultiplier = 1f;
    public float lerpTime = 10f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 targetDirection = Vector3.zero;

    private float fallVelocity = 0f;

    [HideInInspector]
    public CharacterController characterController;

    public float distanceToGround = 0.1f;

    private bool isGrounded;

    private Collider myCollider;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        myCollider = GetComponent<Collider>();
    }


    // Use this for initialization
    void Start () {
        distanceToGround = myCollider.bounds.extents.y;
	}
	
	// Update is called once per frame
	void Update () {
        isGrounded = OnGroundCheck();

        moveDirection = Vector3.Lerp(moveDirection, targetDirection, Time.deltaTime * lerpTime);
        moveDirection.y = fallVelocity;

        characterController.Move(moveDirection * Time.deltaTime);

        if (!isGrounded) {
            fallVelocity -= 90f;
        }
	}

    public bool OnGroundCheck()
    {
        RaycastHit hit;

        if (characterController.isGrounded)
        {
            return true;
        }
        if (Physics.Raycast(myCollider.bounds.center, -Vector3.up, out hit, distanceToGround + -.1f))
        {
            return true;
        }

        return false;
    }

    public void Move(Vector3 dir) {
        targetDirection = dir;
    }

    public void Stop() {
        moveDirection = Vector3.zero;
        targetDirection = Vector3.zero;
    }

    public void Jump(float jumpSpeed) {
        if (isGrounded) {
            fallVelocity = jumpSpeed;
        }
    }
}
