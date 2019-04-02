using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    private bool firstenter = false;
    public Rigidbody2D enemyPrefab;
    public List<GameObject> enemeyspawns;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            firstenter = true;
        }
    }
}
