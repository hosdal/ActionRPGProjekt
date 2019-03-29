using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject rooms;
    public int maxroom;
    private int counter = 0;
    private Object[] roomPrefabs;

    void Awake()
    {
        while (counter < maxroom)
        {
            roomPrefabs = Resources.LoadAll("Rooms");
            Transform[] roomslist = rooms.GetComponentsInChildren<Transform>();
            GameObject room = roomslist[Random.Range(1, roomslist.Length)].gameObject;
            Room script = room.GetComponentInChildren<Room>();
            int index = Random.Range(0, script.has_doors.Length);
            if (script.has_doors[index])
            {
                switch (index)
                {
                    case 0:
                        if (script.left_room == null)
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
                            script.left_room = newobj;
                            newscript.right_room = room.transform.gameObject;
                            newobj.transform.position = new Vector2(room.transform.position.x - 5, room.transform.position.y);
                        }
                        break;
                    case 1:
                        if (script.right_room == null)
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
                            script.right_room = newobj;
                            newscript.left_room = room.transform.gameObject;
                            newobj.transform.position = new Vector2(room.transform.position.x + 5, room.transform.position.y);
                        }
                        break;
                    case 2:
                        if (script.up_room == null)
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
                            script.up_room = newobj;
                            newscript.down_room = room.transform.gameObject;
                            newobj.transform.position = new Vector2(room.transform.position.x, room.transform.position.y + 5);
                        }
                        break;
                    case 3:
                        if (script.down_room == null)
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
                            script.down_room = newobj;
                            newscript.up_room = room.transform.gameObject;
                            newobj.transform.position = new Vector2(room.transform.position.x, room.transform.position.y - 5);
                        }
                        break;
                }
            }

        }
    }

    void Update()
    {

    }
}
