using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            camera.transform.position = new Vector3(transform.position.x,transform.position.y,-10f);
        }

    }
}
