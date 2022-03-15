using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour {
    public float forwardMovementSpeed = 10.0f;
    public float angularVelocity = -300.0f;
    private Rigidbody2D keyboardRigidbody;
    // Start is called before the first frame update
    void Start() {
        keyboardRigidbody = GetComponent<Rigidbody2D>();
        keyboardRigidbody.angularVelocity = angularVelocity;
    }

    // Update is called once per frame
    void Update() {
        
    }

    void FixedUpdate() {
        Vector2 newVelocity = keyboardRigidbody.velocity;
        newVelocity.x = forwardMovementSpeed;
        keyboardRigidbody.velocity = newVelocity;
    }
}
