using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone2Controller : MonoBehaviour
{
    private int hitpoints;
    public float forwardMovementSpeed;
    private GameObject student;
    private Rigidbody2D droneRigidbody;
    private Rigidbody2D studentRigidbody;
    private Animator droneAnimator;
    private Collider2D droneCollider;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        hitpoints = 2;
        droneRigidbody = GetComponent<Rigidbody2D>();
        droneAnimator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        student = GameObject.Find("Student");
        studentRigidbody = student.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void FixedUpdate() {
        Vector2 newVelocity = droneRigidbody.velocity;
        newVelocity.x = forwardMovementSpeed;
        droneRigidbody.velocity = newVelocity;

        if(!droneAnimator.GetBool("isDead")) {
        float playerDistance = studentRigidbody.transform.position.x - transform.position.x;
            if(Mathf.Abs(playerDistance) < 0.5f) {
                Die();
                GameObject expl = (GameObject)Instantiate(explosion);
                Vector3 position = transform.position;
                expl.transform.position = position;
            }
        }
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Ammo")) {
            hitpoints--;
            if (hitpoints == 0) {
                Die();
            }
        }
        
    }

    void Die() {
        droneAnimator.SetBool("isDead", true);
        transform.GetChild(1).gameObject.GetComponent<Renderer>().enabled = false;
        tag = "Untagged";
    }
}
