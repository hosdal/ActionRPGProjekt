﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Platformer character movement
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// Company: Falling Flames Games
/// </summary>

namespace IndieMarc.Platformer
{

    public class PlatformerCharacter : MonoBehaviour
    {

        public int player_id;

        [Header("Movement")]
        public float move_accel = 1f;
        public float move_deccel = 1f;
        public float move_max = 1f;

        [Header("Jump")]
        public bool can_jump = true;
        public float jump_strength = 1f;
        public float jump_time_min = 1f;
        public float jump_time_max = 1f;
        public float jump_gravity = 1f;
        public float jump_fall_gravity = 1f;
        public float jump_move_percent = 0.75f;
        public LayerMask ground_layer;
        public float ground_raycast_dist = 0.1f;

        [Header("Crouch")]
        public bool can_crouch = true;
        public float crouch_coll_percent = 0.5f;

        [Header("Death")]
        public float level_bottom;

        [Header("Parts")]
        public GameObject hold_hand;
        
        private Rigidbody2D rigid;
        private Animator animator;
        private CapsuleCollider2D capsule_coll;
        private ContactFilter2D contact_filter;
        private Vector2 coll_start_h;
        private Vector2 coll_start_off;
        private Vector3 start_scale;

        private Vector2 move;
        private Vector2 move_input;
        private bool jump_press;
        private bool jump_hold;

        private bool is_grounded = false;
        private bool is_ceiled = false;
        private bool is_crouch = false;
        private bool is_jumping = false;
        private float jump_timer = 0f;
        private Vector3 last_ground_pos;

        private CarryItem carry_item;
        private float take_item_timer = 0f;

        private static Dictionary<int, PlatformerCharacter> character_list = new Dictionary<int, PlatformerCharacter>();

        void Awake()
        {
            character_list[player_id] = this;
            rigid = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            capsule_coll = GetComponent<CapsuleCollider2D>();
            coll_start_h = capsule_coll.size;
            coll_start_off = capsule_coll.offset;
            start_scale = transform.localScale;

            contact_filter = new ContactFilter2D();
            contact_filter.layerMask = ground_layer;
            contact_filter.useLayerMask = true;
            contact_filter.useTriggers = false;

            last_ground_pos = transform.position;
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
            float desiredSpeed = Mathf.Abs(move_input.x) > 0.1f ? move_input.x * move_max : 0f;
            float acceleration = Mathf.Abs(move_input.x) > 0.1f ? move_accel : move_deccel;
            acceleration = !is_grounded ? jump_move_percent * acceleration : acceleration;
            move.x = Mathf.MoveTowards(move.x, desiredSpeed, acceleration * Time.fixedDeltaTime);

            //Side facing
            if (Mathf.Abs(move.x) > 0.01f)
            {
                float side = (move.x < 0f) ? -1f : 1f;
                transform.localScale = new Vector3(start_scale.x * side, start_scale.y, start_scale.z);
            }

            UpdateJump();
            UpdateCrouch();

            //Move
            rigid.velocity = move;
        }

        //Handle render and controls
        void Update()
        {

            //Controls
            PlatformerControls controls = PlatformerControls.Get(player_id);
            move_input = controls.GetMove();
            jump_press = controls.GetJumpDown();
            jump_hold = controls.GetJumpHold();

            if (jump_press)
                TryJump();

            //Reset when fall
            if (transform.position.y < level_bottom - GetSize().y)
            {
                Teleport(GetLastGround());
            }

            //Items
            take_item_timer += Time.deltaTime;
            if (carry_item && controls.GetActionDown())
                carry_item.UseItem();

            //Anims
            animator.SetBool("Jump", IsJumping());
            animator.SetBool("InAir", !IsGrounded());
            animator.SetBool("Crouch", IsCrouching());
            animator.SetFloat("Speed", move.magnitude);
            animator.SetBool("Hold", GetHoldingItem() != null);
        }

        private void LateUpdate()
        {
            if (carry_item != null)
                carry_item.UpdateCarryItem();
        }

        private void UpdateJump()
        {
            //Jump
            is_grounded = DetectGrounded(false);
            is_ceiled = DetectGrounded(true);
            jump_timer += Time.fixedDeltaTime;

            //Jump end timer
            if (is_jumping && !jump_hold && jump_timer > jump_time_min)
                is_jumping = false;
            if (is_jumping && jump_timer > jump_time_max)
                is_jumping = false;

            //Jump hit ceil
            if (is_ceiled)
            {
                is_jumping = false;
                move.y = Mathf.Min(move.y, 0f);
            }

            //Add jump velocity
            if (!is_grounded)
            {
                //Falling
                float gravity = !is_jumping ? jump_fall_gravity : jump_gravity; //Gravity increased when going down
                move.y = Mathf.MoveTowards(move.y, -move_max * 2f, gravity * Time.fixedDeltaTime);
            }
            else if (!is_jumping)
            {
                //Grounded
                move.y = 0f;
            }

            //Save last landed position
            if (is_grounded)
                last_ground_pos = transform.position;
        }

        private void UpdateCrouch()
        {
            if (!can_crouch)
                return;

            //Crouch
            if (move_input.y < -0.1f && is_grounded)
            {
                if(!is_crouch)
                    animator.SetTrigger("Action");

                is_crouch = true;
                move = Vector2.zero;
                capsule_coll.size = new Vector2(coll_start_h.x, coll_start_h.y * crouch_coll_percent);
                capsule_coll.offset = new Vector2(coll_start_off.x, coll_start_off.y - coll_start_h.y * (1f - crouch_coll_percent) / 2f);

            }
            else
            {
                is_crouch = false;
                capsule_coll.size = coll_start_h;
                capsule_coll.offset = coll_start_off;
            }
        }

        private void TryJump()
        {
            if (can_jump && is_grounded && !is_crouch)
            {
                move.y = jump_strength;
                jump_timer = 0f;
                is_jumping = true;
                animator.SetTrigger("Action");
            }
        }

        private bool DetectGrounded(bool detect_ceiled)
        {
            bool grounded = false;
            Vector2[] raycastPositions = new Vector2[3];

            Vector2 raycast_start = rigid.position;
            Vector2 orientation = detect_ceiled ? Vector2.up : Vector2.down;
            float radius = GetSize().x * 0.5f * transform.localScale.y; ;

            if (capsule_coll != null)
            {
                //Adapt raycast to collider
                Vector2 raycast_offset = capsule_coll.offset + orientation * Mathf.Abs(capsule_coll.size.y * 0.5f - capsule_coll.size.x * 0.5f);
				raycast_start = rigid.position + raycast_offset * transform.localScale.y;
            }

            float ray_size = radius + ground_raycast_dist;
            raycastPositions[0] = raycast_start + Vector2.left * radius / 2f;
            raycastPositions[1] = raycast_start;
            raycastPositions[2] = raycast_start + Vector2.right * radius / 2f;

            RaycastHit2D[] hitBuffer = new RaycastHit2D[5];
            for (int i = 0; i < raycastPositions.Length; i++)
            {
                Physics2D.Raycast(raycastPositions[i], orientation, contact_filter, hitBuffer, ray_size);
                for (int j = 0; j < hitBuffer.Length; j++)
                {
                    if (hitBuffer[j].collider != null)
                    {
                        grounded = true;
                    }
                }
            }
            return grounded;
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
            is_jumping = false;
        }

        public Vector3 GetMove()
        {
            return move;
        }

        public float GetSide()
        {
            return Mathf.Sign(transform.localScale.x);
        }

        public Vector3 GetLastGround()
        {
            return last_ground_pos;
        }

        public bool IsJumping()
        {
            return is_jumping;
        }

        public bool IsGrounded()
        {
            return is_grounded;
        }

        public bool IsCrouching()
        {
            return is_crouch;
        }

        public Vector2 GetSize()
        {
            if (capsule_coll != null)
                return capsule_coll.size;
            return new Vector2(1f, 1f);
        }

        public Vector3 GetHandPos()
        {
            return hold_hand.transform.position;
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.gameObject.GetComponent<CarryItem>())
            {
                TakeItem(coll.gameObject.GetComponent<CarryItem>());
            }
        }

        public static PlatformerCharacter Get(int player_id)
        {
            foreach (PlatformerCharacter control in GetAll())
            {
                if (control.player_id == player_id)
                {
                    return control;
                }
            }
            return null;
        }

        public static PlatformerCharacter[] GetAll()
        {
            PlatformerCharacter[] list = new PlatformerCharacter[character_list.Count];
            character_list.Values.CopyTo(list, 0);
            return list;
        }
    }

}