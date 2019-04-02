using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public GameObject rooms;
    public int maxroom;
    private int counter = 0;
    private Object[] roomPrefabs;
    private List<Vector2> positions = new List<Vector2>();

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
                            script.left_room = newobj;
                            newscript.right_room = room.transform.gameObject;
                            newobj.transform.position = pos;
                            positions.Add(pos);
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
                            script.right_room = newobj;
                            newscript.left_room = room.transform.gameObject;
                            newobj.transform.position = pos;
                            positions.Add(pos);
                        }
                        break;
                    case 2:
                        pos = new Vector2(room.transform.position.x, room.transform.position.y +10f);
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
                            script.up_room = newobj;
                            newscript.down_room = room.transform.gameObject;
                            newobj.transform.position =pos;
                            positions.Add(pos);
                        }
                        break;
                    case 3:
                        pos = new Vector2(room.transform.position.x, room.transform.position.y-10f);
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
                            script.down_room = newobj;
                            newscript.up_room = room.transform.gameObject;
                            newobj.transform.position = pos;
                            positions.Add(pos);
                        }
                        break;
                }
            }

        }
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
}
