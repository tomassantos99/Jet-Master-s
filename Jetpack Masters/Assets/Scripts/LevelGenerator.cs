using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    public GameObject[] availableRooms;
    public List<GameObject> currentRooms;
    private float screenWidthInPoints;

    // Start is called before the first frame update
    void Start() {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
        
        StartCoroutine(GeneratorCheck());
    }

    // Update is called once per frame
    void Update() {
        
    }

    void AddRoom(float farthestRoomEndX) {
        //1
        int randomRoomIndex = Random.Range(0, availableRooms.Length);
        //2
        if(availableRooms.Length == 0) return; //

        GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);
        //3
        float roomWidth = room.transform.Find("Floor").localScale.x;
        //4
        float roomCenter = farthestRoomEndX + roomWidth * 0.5f;
        //5
        room.transform.position = new Vector3(roomCenter, 0, 0);
        //6
        currentRooms.Add(room);
    }

    private void GenerateRoomIfRequired() {
        //1
        List<GameObject> roomsToRemove = new List<GameObject>();
        //2
        bool addRooms = true;
        //3
        float playerX = transform.position.x;
        //4
        float removeRoomX = playerX - screenWidthInPoints;
        //5
        float addRoomX = playerX + screenWidthInPoints;
        //6
        float farthestRoomEndX = 0;
        foreach (var room in currentRooms) {
            //7
            float roomWidth = room.transform.Find("Floor").localScale.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;
            //8
            if (roomStartX > addRoomX) {
                addRooms = false;
            }
            //9
            if (roomEndX < removeRoomX) {
                roomsToRemove.Add(room);
            }
            //10
            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }
        //11
        foreach (var room in roomsToRemove) {
            currentRooms.Remove(room);
            Destroy(room);
        }
        //12
        if (addRooms) {
            AddRoom(farthestRoomEndX);
        }
    }

    private IEnumerator GeneratorCheck() {
        while (true) {
            GenerateRoomIfRequired();
            yield return new WaitForSeconds(0.25f);
        }
    }


}
