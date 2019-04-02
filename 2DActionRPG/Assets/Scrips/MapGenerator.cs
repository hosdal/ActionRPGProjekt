using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public GameObject rooms;
    public int maxroom;
    private int counter = 0;
    private Object[] roomPrefabs;
    private List<Vector2> positions = new List<Vector2>();
    private List<GameObject> createdRooms = new List<GameObject>();

    void Awake()
    {
        positions.Add(new Vector2(0, 0));
        while (counter < maxroom)
        {
            roomPrefabs = Resources.LoadAll("Rooms");
            var roomslist = rooms.GetComponentsInChildren<Room>();
            GameObject room = roomslist[Random.Range(0, roomslist.Length)].gameObject;
            Room script = room.GetComponentInChildren<Room>();
            int index = Random.Range(0, script.has_doors.Length);
            Vector2 pos = new Vector2();
            if (script.has_doors[index])
            {
                switch (index)
                {
                    case 0:
                        pos = new Vector2(room.transform.position.x - 13.5f, room.transform.position.y);
                        if (script.left_room == null && !isPosInArray(pos, positions))
                        {
                            GameObject obj = null;
                            bool found = false;
                            Room newscript = null;
                            while (!found)
                            {
                                obj = (GameObject)roomPrefabs[Random.Range(0, roomPrefabs.Length)];
                                newscript = obj.GetComponent<Room>();
                                if (newscript.has_doors[1])
                                    found = true;
                            }
                            counter++;
                            var newobj = Instantiate(obj);
                            newscript = newobj.GetComponent<Room>();
                            newobj.transform.parent = rooms.transform;
                            newobj.transform.position = pos;
                            positions.Add(pos);
                            createdRooms.Add(newobj);
                        }
                        break;
                    case 1:
                        pos = new Vector2(room.transform.position.x + 13.5f, room.transform.position.y);
                        if (script.right_room == null && !isPosInArray(pos, positions))
                        {
                            GameObject obj = null;
                            bool found = false;
                            Room newscript = null;
                            while (!found)
                            {
                                obj = (GameObject)roomPrefabs[Random.Range(0, roomPrefabs.Length)];
                                newscript = obj.GetComponent<Room>();
                                if (newscript.has_doors[0])
                                    found = true;
                            }
                            counter++;
                            var newobj = Instantiate(obj);
                            newscript = newobj.GetComponent<Room>();
                            newobj.transform.parent = rooms.transform;
                            newobj.transform.position = pos;
                            positions.Add(pos);
                            createdRooms.Add(newobj);
                        }
                        break;
                    case 2:
                        pos = new Vector2(room.transform.position.x, room.transform.position.y + 10f);
                        if (script.up_room == null && !isPosInArray(pos, positions))
                        {
                            GameObject obj = null;
                            bool found = false;
                            Room newscript = null;
                            while (!found)
                            {
                                obj = (GameObject)roomPrefabs[Random.Range(0, roomPrefabs.Length)];
                                newscript = obj.GetComponent<Room>();
                                if (newscript.has_doors[3])
                                    found = true;
                            }
                            counter++;
                            var newobj = Instantiate(obj);
                            newscript = newobj.GetComponent<Room>();
                            newobj.transform.parent = rooms.transform;
                            newobj.transform.position = pos;
                            positions.Add(pos);
                            createdRooms.Add(newobj);

                        }
                        break;
                    case 3:
                        pos = new Vector2(room.transform.position.x, room.transform.position.y - 10f);
                        if (script.down_room == null && !isPosInArray(pos, positions))
                        {
                            GameObject obj = null;
                            bool found = false;
                            Room newscript = null;
                            while (!found)
                            {
                                obj = (GameObject)roomPrefabs[Random.Range(0, roomPrefabs.Length)];
                                newscript = obj.GetComponent<Room>();
                                if (newscript.has_doors[2])
                                    found = true;
                            }
                            counter++;
                            var newobj = Instantiate(obj);
                            newscript = newobj.GetComponent<Room>();
                            newobj.transform.parent = rooms.transform;
                            newobj.transform.position = pos;
                            positions.Add(pos);
                            createdRooms.Add(newobj);
                            foreach (var g in createdRooms)
                            {
                                checkNeigbour(newobj, g);
                            }
                        }
                        break;
                }
            }

        }

        foreach (var g in createdRooms)
        {
            foreach (var g2 in createdRooms)
            {
                checkNeigbour(g, g2);
            }
        }
    }

    private void createRoom()
    {

    }

    private bool isPosInArray(Vector2 val, List<Vector2> arr)
    {
        for (int i = 0; i < arr.Count; i++)
        {
            if (arr[i] == val)
                return true;
        }
        return false;
    }

    private void checkNeigbour(GameObject room1, GameObject room2)
    {
        Room script1 = room1.GetComponentInChildren<Room>();
        Room script2 = room2.GetComponentInChildren<Room>();
        if (room1.transform.position.x - 13.5f == room2.transform.position.x && room1.transform.position.y == room2.transform.position.y && script1.has_doors[0] && script2.has_doors[1])
        {
            script1.left_room = room2;
        }
        else if (room1.transform.position.x + 13.5f == room2.transform.position.x && room1.transform.position.y == room2.transform.position.y && script1.has_doors[1] && script2.has_doors[0])
        {
            script1.right_room = room2;
        }
        else if (room1.transform.position.x == room2.transform.position.x && room1.transform.position.y + 10f == room2.transform.position.y && script1.has_doors[2] && script2.has_doors[3])
        {
            script1.up_room = room2;
        }
        else if (room1.transform.position.x == room2.transform.position.x && room1.transform.position.y - 10f == room2.transform.position.y && script1.has_doors[3] && script2.has_doors[2])
        {
            script1.down_room = room2;
        }
    }
}
