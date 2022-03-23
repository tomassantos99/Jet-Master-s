using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentController : MonoBehaviour
{

    public float jetpackForce = 75.0f;
    public float forwardMovementSpeed = 3.0f;
    public ParticleSystem mainJetpack;
    public ParticleSystem miniJetpack;

    private Rigidbody2D playerRigidbody;

    public Transform groundCheckTransform;
    private bool isGrounded;
    public LayerMask groundCheckLayerMask;
    private Animator mouseAnimator;

    // Start is called before the first frame update
    void Start()
    {
        mouseAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        bool jetpackActive = Input.GetButton("Fire1");
        if (jetpackActive)
        {
            playerRigidbody.AddForce(new Vector2(0, jetpackForce));
        }
        Vector2 newVelocity = playerRigidbody.velocity;
        newVelocity.x = forwardMovementSpeed;
        playerRigidbody.velocity = newVelocity;

        UpdateGroundedStatus();
        AdjustJetpack(jetpackActive);
    }

    void UpdateGroundedStatus()
    {
        //1
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        //2
        mouseAnimator.SetBool("isGrounded", isGrounded);
    }

    void AdjustJetpack(bool jetpackActive)
    {
        var mainJetpackEmission = mainJetpack.emission;
        var miniJetpackEmission = miniJetpack.emission;
        mainJetpackEmission.enabled = !isGrounded;
        miniJetpackEmission.enabled = !isGrounded;
        if (jetpackActive)
        {
            mainJetpackEmission.rateOverTime = 300.0f;
            miniJetpackEmission.rateOverTime = 300.0f;
        }
        else
        {
            mainJetpackEmission.rateOverTime = 75.0f;
            miniJetpackEmission.rateOverTime = 300.0f;
        }
    }
}
