using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {
    private bool firstenter = false;
    public Rigidbody2D enemyPrefab;
    public List<GameObject> enemeyspawns;
    private List<Rigidbody2D> enemies = new List<Rigidbody2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !firstenter)
        {
            firstenter = true;
            foreach (var spawn in enemeyspawns)
            {
                enemies.Add(Instantiate(enemyPrefab, spawn.transform));
            }
        }
    }

    private void Update()
    {
        
    }
}
