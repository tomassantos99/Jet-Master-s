using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour {   
    public GameObject ammo;
    public List<GameObject> currentAmmo;
    private Rigidbody2D playerRigidbody;

    // Start is called before the first frame update
    void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        StartCoroutine(GeneratorCheck());
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            GameObject keyboard = (GameObject)Instantiate(ammo);

            Vector3 startPosition = transform.position;

            keyboard.transform.position = new Vector3(startPosition.x + 0.5f, startPosition.y, 0);

            currentAmmo.Add(keyboard);
        }
    }

    private IEnumerator GeneratorCheck() {
        while (true) {
            List<GameObject> ammoToRemove = new List<GameObject>();
            foreach (var ammo in currentAmmo) {
                float playerX = transform.position.x;

                if (ammo != null)
                {
                    float lastAmmoX = ammo.transform.position.x;
                    if (playerX - lastAmmoX < -10)
                    {
                        ammoToRemove.Add(ammo);
                    }
                }
            }
            foreach (var ammo in ammoToRemove) {
                currentAmmo.Remove(ammo);
                Destroy(ammo);
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
}
