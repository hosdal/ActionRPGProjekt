using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
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
    }

    private void FixedUpdate()
    {
        if (room.isIn)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), speed * Time.deltaTime);

            //transform.position += transform.forward * speed * Time.deltaTime;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
