using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
     if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("hit");
        } else if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Door")
        {
            Destroy(gameObject);
        }
    }
}
