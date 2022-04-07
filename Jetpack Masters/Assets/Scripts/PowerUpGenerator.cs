using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpGenerator : MonoBehaviour
{

    private float screenWidthInPoints;

    public float objectsMinDistance = 5.0f;
    public float objectsMaxDistance = 10.0f;

    public float objectsMinY = -1.4f;
    public float objectsMaxY = 1.4f;

    public GameObject[] powerUps;
    public List<GameObject> currentPowerUps;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
        currentPowerUps = new List<GameObject>();
        StartCoroutine(GeneratorCheck());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GeneratorCheck()
    {
        while (true)
        {
            GeneratePowerUp();
            yield return new WaitForSeconds(2f);
        }
    }

    void GeneratePowerUp()
        
    {
        float playerX = transform.position.x;
        float removeObjectsX = playerX - screenWidthInPoints;

        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (var obj in currentPowerUps)
        {
            if (obj != null)
            {
                float objX = obj.transform.position.x;

                if (objX < removeObjectsX)
                {
                    objectsToRemove.Add(obj);
                }
            }

        }

        foreach (var obj in objectsToRemove)
        {
            currentPowerUps.Remove(obj);
            if (obj != null)
            {
                Destroy(obj);
            }

        }

        int rnd = Random.Range(0, 100);

        if (rnd < 10)
        {
            AddPowerUp();
        }
    }

    void AddPowerUp()
    {

        int randomIndex;
        GameObject obj;

        randomIndex = Random.Range(0, powerUps.Length);

        obj = Instantiate(powerUps[randomIndex]);

        float objectPositionX = gameObject.transform.position.x + screenWidthInPoints + 20;
        float randomY = Random.Range(objectsMinY, objectsMaxY);

        obj.transform.position = new Vector3(objectPositionX, randomY, 0);

        currentPowerUps.Add(obj);

    }
}
