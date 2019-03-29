using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Top down character movement
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// Company: Falling Flames Games
/// </summary>

namespace IndieMarc.TopDown
{
    public class TopDownCharacter : MonoBehaviour
    {
        public int player_id;

        [Header("Movement")]
        public float move_accel = 1f;
        public float move_deccel = 1f;
        public float move_max = 1f;

        [Header("Parts")]
        public GameObject hold_hand;

        private Rigidbody2D rigid;
        private Animator animator;
        private AutoOrderLayer auto_order;
        private ContactFilter2D contact_filter;

        private CarryItem carry_item;
        private Vector2 move;
        private Vector2 move_input;
        private Vector2 lookat = Vector2.zero;
        private float side = 1f;
        private float take_item_timer = 0f;

        private static Dictionary<int, TopDownCharacter> character_list = new Dictionary<int, TopDownCharacter>();

        void Awake()
        {
            character_list[player_id] = this;
            rigid = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            auto_order = GetComponent<AutoOrderLayer>();
        }

        void OnDestroy()
        {
            character_list.Remove(player_id);
        }

        void Start()
        {

        }

        //Handle physics
        void FixedUpdate()
        {
            //Movement velocity
            float desiredSpeedX = Mathf.Abs(move_input.x) > 0.1f ? move_input.x * move_max : 0f;
            float accelerationX = Mathf.Abs(move_input.x) > 0.1f ? move_accel : move_deccel;
            move.x = Mathf.MoveTowards(move.x, desiredSpeedX, accelerationX * Time.fixedDeltaTime);
            float desiredSpeedY = Mathf.Abs(move_input.y) > 0.1f ? move_input.y * move_max : 0f;
            float accelerationY = Mathf.Abs(move_input.y) > 0.1f ? move_accel : move_deccel;
            move.y = Mathf.MoveTowards(move.y, desiredSpeedY, accelerationY * Time.fixedDeltaTime);

            //Move
            rigid.velocity = move;
            
        }

        //Handle render and controls
        void Update()
        {
            //Controls
            TopDownControls controls = TopDownControls.Get(player_id);
            move_input = controls.GetMove();

            //Update lookat side
            if (move.magnitude > 0.1f)
                lookat = move.normalized;
            if (Mathf.Abs(lookat.x) > 0.02)
                side = Mathf.Sign(lookat.x);

            //Items
            take_item_timer += Time.deltaTime;
            if (carry_item && controls.GetActionDown())
                carry_item.UseItem();

            //Anims
            animator.SetFloat("Speed", move.magnitude);
            animator.SetInteger("Side", GetSideAnim());
            animator.SetBool("Hold", GetHoldingItem() != null);
        }

        private void LateUpdate()
        {
            if (carry_item != null)
                carry_item.UpdateCarryItem();
        }

        public void TakeItem(CarryItem item)
        {
            if (take_item_timer < 0f)
                return;

            if (item.CanTake(gameObject))
            {
                if (!item.HasBearer())
                {
                    //Drop current and take new item
                    DropItem();
                    carry_item = item;
                    item.Take(this);
                    take_item_timer = -0.2f;
                }
            }
        }

        public void DropItem()
        {
            if (carry_item != null)
                carry_item.Drop();
            carry_item = null;
            take_item_timer = -0.2f;
        }

        public CarryItem GetHoldingItem()
        {
            return carry_item;
        }

        public void Teleport(Vector3 pos)
        {
            transform.position = pos;
            move = Vector2.zero;
        }

        public Vector3 GetMove()
        {
            return move;
        }

        public int GetSortOrder()
        {
            return auto_order.GetSortOrder();
        }

        //Get Character side
        public float GetSide()
        {
            return side; //Return 1 frame before to let anim do transitions
        }

        public int GetSideAnim()
        {
            return (side >= 0) ? 1 : 3;
        }

        public Vector3 GetHandPos()
        {
            return hold_hand.transform.position;
        }

        void OnCollisionStay2D(Collision2D coll)
        {
            if (coll.gameObject.GetComponent<Door>() && carry_item && carry_item.GetComponent<Key>())
            {
                carry_item.GetComponent<Key>().TryOpenDoor(coll.gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.gameObject.GetComponent<CarryItem>())
            {
                TakeItem(coll.gameObject.GetComponent<CarryItem>());
            }
        }
    }
}
