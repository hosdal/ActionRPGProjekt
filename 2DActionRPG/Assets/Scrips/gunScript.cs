﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunScript : MonoBehaviour
{
    public Rigidbody2D bulletPrefab;
    public Transform bulletEntry;
    private Animator animator;
    [SerializeField]
    private float bulletSpeed = 20f;
    

    private PlayerScript player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerScript>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Bullet action
        if(Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("gunShot", true);
            Vector2 gunDirection = bulletEntry.TransformDirection(Vector2.right);


            //create bullet
            Rigidbody2D bulletInstance = Instantiate(bulletPrefab, bulletEntry.transform.position, bulletEntry.transform.rotation);
            

            //add force to bullet
            Vector2 forceToAdd = gunDirection * bulletSpeed;
            bulletInstance.AddForce(forceToAdd, ForceMode2D.Force);

        }
        animator.SetBool("gunShot", false);
    }
}