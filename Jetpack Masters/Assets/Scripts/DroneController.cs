using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    private int hitpoints;
    public float forwardMovementSpeed;
    public float verticalForce;
    public float maxYSpeed;
    private GameObject student;
    private Rigidbody2D droneRigidbody;
    private Rigidbody2D studentRigidbody;
    private Animator droneAnimator;
    // Start is called before the first frame update
    void Start()
    {
        hitpoints = 2;
        student = GameObject.Find("Student");
        droneRigidbody = GetComponent<Rigidbody2D>();
        studentRigidbody = student.GetComponent<Rigidbody2D>();
        droneAnimator = transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        if (droneAnimator.GetBool("isDead")) {
            return;
        }

        FollowPlayer();

        Vector2 newVelocity = droneRigidbody.velocity;
        if (Mathf.Abs(newVelocity.y) > maxYSpeed) {
            newVelocity.y = maxYSpeed * (newVelocity.y/Mathf.Abs(newVelocity.y));
        }
        newVelocity.x = forwardMovementSpeed;
        droneRigidbody.velocity = newVelocity;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Ammo")) {
            hitpoints--;
            if (hitpoints == 0) {
                droneAnimator.SetBool("isDead", true);
                droneRigidbody.gravityScale = 1;
                tag = "Untagged";
            }
        }
        
    }

    void FollowPlayer() {
        float dif = studentRigidbody.transform.position.y - transform.position.y;
        Vector2 velocity = droneRigidbody.velocity;
        if(Mathf.Abs(dif) < 0.1f) {
            velocity.y = 0f;
            droneRigidbody.velocity = velocity;
        }
        else if(dif > 0f) {
            if(dif < 0.5f) 
                droneRigidbody.AddForce(new Vector2(0, Mathf.Abs(verticalForce / dif)));
            else
                droneRigidbody.AddForce(new Vector2(0, Mathf.Abs(verticalForce * dif)));
        }
        else if(dif < 0f) {
            
            if(dif > -0.5f) 
                droneRigidbody.AddForce(new Vector2(0, -Mathf.Abs(verticalForce / dif)));
            else
                droneRigidbody.AddForce(new Vector2(0, -Mathf.Abs(verticalForce * dif)));
        }
    }
}
