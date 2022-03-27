using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapperController : MonoBehaviour {
    public List<GameObject> zappers;
    public List<GameObject> currentZappers;
    public float currentPosition;
    private Animator playerAnimator;
    // Start is called before the first frame update
    void Start() {
        currentPosition = 10.0f;
        currentZappers = new List<GameObject>();

        for(int i = 0; i < 3; i++) {
            GameObject zap = (GameObject)Instantiate(zappers[Random.Range(0, 3)]);

            zap.transform.position = new Vector3(currentPosition, 0.0f, 0.0f);

            currentPosition += 10.0f;

            currentZappers.Add(zap);
        }

        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void FixedUpdate() {
        if(transform.position.x - currentZappers[0].transform.position.x > 10.0f) { 
            GameObject zap = currentZappers[0];

            currentZappers.RemoveAt(0);

            Destroy(zap);

            zap = (GameObject)Instantiate(zappers[Random.Range(0, 3)]);

            zap.transform.position = new Vector3(currentPosition, 0.0f, 0.0f);

            currentPosition += 10.0f;

            currentZappers.Add(zap);
        }

        if(GetComponent<Collider2D>().bounds.Intersects(currentZappers[0].GetComponent<Collider2D>().bounds)) { 
            playerAnimator.SetBool("isDead", true);
        }
    }
}
