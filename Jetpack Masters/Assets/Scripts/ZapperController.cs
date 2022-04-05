using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapperController : MonoBehaviour {

    StudentController studentController;
    [SerializeField] GameObject student;
    public List<GameObject> zappers;
    public List<GameObject> currentZappers;
    public float currentPosition;
    private Animator playerAnimator;

    void Awake()
    {
        studentController = student.GetComponent<StudentController>();
    }

    // Start is called before the first frame update
    void Start() {
        currentPosition = 10.0f;
        currentZappers = new List<GameObject>();

        for(int i = 0; i < 3; i++) {
            GameObject zap = GenerateZapper();

            currentZappers.Add(zap);
        }

        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void FixedUpdate() {

        GameObject zap;

        if (currentZappers.Count > 0 && transform.position.x - currentZappers[0].transform.position.x > 10.0f)
        {
            zap = currentZappers[0];

            currentZappers.RemoveAt(0);

            Destroy(zap);

            if (!studentController.bossBattleActive)
            {
                zap = GenerateZapper();

                currentZappers.Add(zap);
            }
        }
    }

    GameObject GenerateZapper() {
        GameObject zap = (GameObject)Instantiate(zappers[Random.Range(0, 3)]);

        zap.transform.position = new Vector3(currentPosition, 0.0f, 0.0f);

        int rotation = Random.Range(0, 4);
        zap.transform.eulerAngles = new Vector3(0, 0, 45.0f * rotation);

        Vector3 position = zap.transform.position;

        float colliderHight = zap.GetComponent<BoxCollider2D>().size.y;
        float colliderWidth = zap.GetComponent<BoxCollider2D>().size.x;
    
        switch(Random.Range(0, 4)) { 
            case 0:
                switch(rotation) {
                    case 0:
                        position += new Vector3(0, colliderHight/2 - 2.53f, 0);
                        zap.transform.position = position;
                    break;
                    case 2:
                        position += new Vector3(0, colliderWidth/2 - 2.53f, 0);
                        zap.transform.position = position;
                    break;
                    default:
                        position += new Vector3(0, colliderHight/2*Mathf.Cos(Mathf.PI/4) - 2.53f, 0);
                        zap.transform.position = position;
                    break;
                }
            break;
            case 1:
                switch(rotation) {
                    case 0:
                        position -= new Vector3(0, colliderHight/2 - 2.48f, 0);
                        zap.transform.position = position;
                    break;
                    case 2:
                        position -= new Vector3(0, colliderWidth/2 - 2.48f, 0);
                        zap.transform.position = position;
                    break;
                    default:
                        position -= new Vector3(0, colliderHight/2*Mathf.Cos(Mathf.PI/4) - 2.48f, 0);
                        zap.transform.position = position;
                    break;
                }
            break;
            default:
            break;
        }
         currentPosition += 10.0f;

        return zap;
    }
}
