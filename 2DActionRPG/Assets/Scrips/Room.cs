using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    [Header("3 = Down")]
    [Header("2 = Up")]
    [Header("1 = Right")]
    [Header("0 = Left")]
    public bool[] has_doors = new bool[4];
    public Object left_room;
    public Object right_room;
    public Object up_room;
    public Object down_room;
    private Camera camera;
    private List<GameObject> doors = new List<GameObject>();

    private void Start()
    {
        camera = Camera.main;
        var children = GetComponents<Transform>();
        foreach (var c in children)
        {
            if (transform.gameObject.tag == "Door")
            {
                doors.Add(c.gameObject);
            }
        }

    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            camera.transform.position = transform.position;
        /*if (collision.gameObject.tag == "Enemy")
        {
            foreach (var d in doors)
            {
                d.SetActive(true);
            }
        } else
        {
            foreach (var d in doors)
            {
                d.SetActive(false);
            }
        }*/
    }

}
