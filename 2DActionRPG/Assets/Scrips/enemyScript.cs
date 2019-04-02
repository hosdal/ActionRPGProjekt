using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour, IDamageable
{
    public Roomfloor room;
    [SerializeField]
    private int maxHp;
    private int currentHp;
    [SerializeField]
    private int attackPower;
    [SerializeField]
    private float speed;

    private PlayerScript player;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        currentHp = maxHp;
    }

    private void FixedUpdate()
    { 
       
        if (room.isIn)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(player.transform.position.x, player.transform.position.y), speed*Time.deltaTime);

            Vector3 diff = (player.transform.position - transform.position);

   

            float angle = Mathf.Atan2(diff.y, diff.x);
            transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
          
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), speed * Time.deltaTime);

            //transform.position += transform.forward * speed * Time.deltaTime;

        }
    }

    public void AddDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp == 0)
        {
            Destroy(gameObject);
        }
    }
}
