using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
   {
    [SerializeField]
    private float runSpeed = 4f;
    private Vector2 moveVelocity;
    private Animator animator;
    private Rigidbody2D rb2D;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }
   

    // Update is called once per frame
    void FixedUpdate()
    {
       
        //The player character follows the cursor
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //"Move" should be under follow cursor, or else it won't work  
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Move(horizontal,vertical);
    }

    public void Move(float horizontal, float vertical)
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        Vector2 targetVelocity = new Vector2(horizontal, vertical);
        transform.Translate(targetVelocity * runSpeed*Time.deltaTime, Space.World);

    }


}
