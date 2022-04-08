using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Generator : MonoBehaviour
{
    StudentController studentController;

    //Needs access to this object so it can check variables from its scripts
    [SerializeField] GameObject student;

    public GameObject[] availableRooms;
    public GameObject firstRoom;
    private List<GameObject> currentRooms;
    private float screenWidthInPoints;

    public GameObject[] coinsObjects;
    public List<GameObject> objects;

    public GameObject[] nonBossObjects;

    public float objectsMinDistance = 5.0f;
    public float objectsMaxDistance = 10.0f;

    public float objectsMinY = -1.4f;
    public float objectsMaxY = 1.4f;

    public float objectsMinRotation = -45.0f;
    public float objectsMaxRotation = 45.0f;

    void Awake()
    {
        studentController = student.GetComponent<StudentController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentRooms = new List<GameObject>();
        currentRooms.Add(Instantiate(firstRoom));
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
        StartCoroutine(GeneratorCheck());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddRoom(float farthestRoomEndX)
    {
        int randomRoomIndex = Random.Range(0, availableRooms.Length);
        GameObject room = Instantiate(availableRooms[randomRoomIndex]);
        float roomWidth = room.transform.Find("Floor").localScale.x;
        float roomCenter = farthestRoomEndX + roomWidth * 0.5f;
        room.transform.position = new Vector3(roomCenter, 0, 0);
        currentRooms.Add(room);
    }

    private void GenerateRoomIfRequired()
    {
        List<GameObject> roomsToRemove = new List<GameObject>();

        bool addRooms = true;

        float playerX = transform.position.x;

        float removeRoomX = playerX - screenWidthInPoints;

        float addRoomX = playerX + screenWidthInPoints;

        float farthestRoomEndX = 0;
        foreach (var room in currentRooms)
        {
            float roomWidth = room.transform.Find("Floor").localScale.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;

            if (roomStartX > addRoomX)
            {
                addRooms = false;
            }

            if (roomEndX < removeRoomX)
            {
                roomsToRemove.Add(room);
            }

            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }

        foreach (var room in roomsToRemove)
        {
            currentRooms.Remove(room);
            Destroy(room);
        }

        if (addRooms)
        {
            AddRoom(farthestRoomEndX);
        }
    }

    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            GenerateRoomIfRequired();

            // If the player is the middle of a battle with a boss there is no need to generate new objects
            if (!studentController.bossBattleActive)
            {
                GenerateObjectsIfRequired();
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    void AddObject(float lastObjectX)
    {
        int rnd = Random.Range(0, 100);

        int randomIndex;
        GameObject obj;

        if (rnd < 20)
        {
            randomIndex = Random.Range(0, nonBossObjects.Length);

            obj = Instantiate(nonBossObjects[randomIndex]);
        }
        else
        {
            randomIndex = Random.Range(0, coinsObjects.Length);

            obj = Instantiate(coinsObjects[randomIndex]);
        }

        float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
        float randomY = Random.Range(objectsMinY, objectsMaxY);
        obj.transform.position = new Vector3(objectPositionX, randomY, 0);

        float rotation = Random.Range(objectsMinRotation, objectsMaxRotation);
        obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);

        objects.Add(obj);
    }

    void GenerateObjectsIfRequired()
    {
        float playerX = transform.position.x;
        float removeObjectsX = playerX - screenWidthInPoints;
        float addObjectX = playerX + screenWidthInPoints;
        float farthestObjectX = 0;

        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (var obj in objects)
        {
            if (obj != null)
            {
                float objX = obj.transform.position.x;

                farthestObjectX = Mathf.Max(farthestObjectX, objX);

                if (objX < removeObjectsX)
                {
                    objectsToRemove.Add(obj);
                }
            }

        }

        // Remove objects that are to the left of the player and out of the game scene
        foreach (var obj in objectsToRemove)
        {
            objects.Remove(obj);
            if (obj != null)
            {
                Destroy(obj);
            }

        }

        if (farthestObjectX < addObjectX)
        {

            AddObject(addObjectX);
        }
    }
}
