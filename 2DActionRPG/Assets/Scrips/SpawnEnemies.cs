using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {
    private bool firstenter = false;
    public Rigidbody2D enemyPrefab;
    public List<GameObject> enemeyspawns;
    private List<Rigidbody2D> enemies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !firstenter)
        {
            firstenter = true;
            foreach (var spawn in enemeyspawns)
            {
                Instantiate(enemyPrefab, spawn.transform);
            }
        }
    }

    private void Update()
    {
        if (enemies.Count == 0)
        {
            var script = GetComponentInParent<Room>();
            foreach (var d in script.doors)
            {
                d.SetActive(false);
            }
        }
    }
}
