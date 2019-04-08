using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomfloor : MonoBehaviour
{
    private bool firstenter = false;
    public Rigidbody2D enemyPrefab;
    public List<GameObject> enemeyspawns;
    public List<Rigidbody2D> enemies = new List<Rigidbody2D>();
    public bool isIn = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !firstenter)
        {
            firstenter = true;
            foreach (var spawn in enemeyspawns)
            {
                var enemy = Instantiate(enemyPrefab, spawn.transform);
                enemy.GetComponent<enemyScript>().room = this;
                enemies.Add(enemy);
            }
        }
        isIn = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            isIn = false;
    }

    private void Update()
    {
        if (enemies.Count == 0 && firstenter && isIn)
        {
            Room script = GetComponentInParent<Room>();
            foreach (var d in script.doors)
            {
                if ((d.transform.localPosition.x < 0f && script.left_room != null) || (d.transform.localPosition.x > 0f && script.right_room != null) || (d.transform.localPosition.y > 0f && script.up_room != null) || (d.transform.localPosition.y < 0f && script.down_room != null))
                {
                    Debug.Log(d.transform.position);
                    Debug.Log(script.left_room);
                    Debug.Log(script.right_room);
                    Debug.Log(script.up_room);
                    Debug.Log(script.down_room);
                    d.SetActive(false);
                }
            }
            if (script.left_room != null)
            {
                var gameobject = (GameObject)script.left_room;
                var newscript = gameobject.GetComponent<Room>();
                foreach (var d in newscript.doors)
                {
                    if (d.transform.position.x > gameobject.transform.position.x)
                        d.SetActive(false);
                }

            }
            if (script.right_room != null)
            {
                var gameobject = (GameObject)script.right_room;
                var newscript = gameobject.GetComponent<Room>();
                foreach (var d in newscript.doors)
                {
                    if (d.transform.position.x < gameobject.transform.position.x)
                        d.SetActive(false);
                }
            }
            if (script.up_room != null)
            {
                var gameobject = (GameObject)script.up_room;
                var newscript = gameobject.GetComponent<Room>();
                foreach (var d in newscript.doors)
                {
                    if (d.transform.position.y < gameobject.transform.position.y)
                        d.SetActive(false);
                }

            }
            if (script.down_room != null)
            {
                var gameobject = (GameObject)script.down_room;
                var newscript = gameobject.GetComponent<Room>();
                foreach (var d in newscript.doors)
                {
                    if (d.transform.position.y > gameobject.transform.position.y)
                        d.SetActive(false);
                }

            }
        }
    }
}
