using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomfloor: MonoBehaviour {
    private bool firstenter = false;
    public Rigidbody2D enemyPrefab;
    public List<GameObject> enemeyspawns;
    private List<Rigidbody2D> enemies = new List<Rigidbody2D>();
    public bool isIn = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !firstenter)
        {
            isIn = true;
            firstenter = true;
            foreach (var spawn in enemeyspawns)
            {
                var enemy = Instantiate(enemyPrefab, spawn.transform);
                enemy.GetComponent<enemyScript>().room = this;
                enemies.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            isIn = false;
    }

    private void Update()
    {
        
    }
}
