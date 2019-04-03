using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour, IDamageable {
    public Roomfloor room;
    [SerializeField]
    private int maxHp;
    private int currentHp;
    [SerializeField]
    private int attackPower;
    [SerializeField]
    private float speed;

    private PlayerScript player;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        currentHp = maxHp;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        if (room.isIn)
        {
            animator.SetFloat("Speed", speed);
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(player.transform.position.x, player.transform.position.y), speed * Time.deltaTime);

            Vector3 diff = (player.transform.position - transform.position);



            float angle = Mathf.Atan2(diff.y, diff.x);
            transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)  
    {
        if(collision.gameObject.tag == "Player")
        {
            player.addDamage(1);
            animator.SetTrigger("Attack");
        }
    }

    public void AddDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp == 0)
        {
            Destroy(gameObject);
            room.enemies.RemoveAt(room.enemies.Count - 1);
        }
    }
}
