using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float jetpackForce = 75.0f;
    public float forwardMovementSpeed = 3.0f;
    private Rigidbody2D playerRigidbody;
    public Transform groundCheckTransform;
    private bool isGrounded;
    public LayerMask groundCheckLayerMask;
    private Animator mouseAnimator;



    // Start is called before the first frame update
    void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        mouseAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void FixedUpdate() {
        bool jetpackActive = Input.GetButton("Fire1");
        if (jetpackActive) {
            playerRigidbody.AddForce(new Vector2(0, jetpackForce));
        }

        Vector2 newVelocity = playerRigidbody.velocity;
        newVelocity.x = forwardMovementSpeed;
        playerRigidbody.velocity = newVelocity;

        UpdateGroundedStatus();
    }

    void UpdateGroundedStatus() {
        //1
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        //2
        mouseAnimator.SetBool("isGrounded", isGrounded);
    }

}
