using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    public float forwardMovementSpeed = 10.0f;
    public float angularVelocity = -300.0f;
    private Rigidbody2D ammoRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        ammoRigidbody = GetComponent<Rigidbody2D>();
        ammoRigidbody.angularVelocity = angularVelocity;
        int sprite = Random.Range(0, 4);
        for (int i = 0; i < 4; i++) {
            if (i != sprite)
                transform.GetChild(i).gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate() {
        Vector2 newVelocity = ammoRigidbody.velocity;
        newVelocity.x = forwardMovementSpeed;
        ammoRigidbody.velocity = newVelocity;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Drone")){
           gameObject.transform.position = new Vector3(0f,100f,0f);
        }
        Destroy(gameObject);
    }
}
