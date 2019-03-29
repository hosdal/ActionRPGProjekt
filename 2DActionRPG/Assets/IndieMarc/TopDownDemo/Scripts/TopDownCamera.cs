﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Top-down camera script
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// Company: Falling Flames Games
/// </summary>

namespace IndieMarc.TopDown
{

    public class TopDownCamera : MonoBehaviour
    {
        [Header("Camera Target")]
        public GameObject target;
        public Vector3 target_offset;
        public float camera_speed = 5f;
        
        private TopDownCharacter target_character;
        private Camera cam;
        private Vector3 cur_pos;

        private static TopDownCamera _instance;

        void Awake()
        {
            _instance = this;
            cam = GetComponent<Camera>();
        }

        void LateUpdate()
        {
            if (target != null)
            {
                //Find target
                Vector3 target_pos = target.transform.position + target_offset;
                
                //Check if need to move
                Vector3 diff = target_pos - transform.position;
                if (diff.magnitude > 0.1f)
                {
                    //Move camera
                    transform.position = Vector3.SmoothDamp(transform.position, target_pos, ref cur_pos, 1f / camera_speed, Mathf.Infinity, Time.deltaTime);
                }
            }
        }

        public float GetFrustrumHeight()
        {
            if (cam.orthographic)
                return 2f * cam.orthographicSize;
            else
                return 2.0f * Mathf.Abs(transform.position.z) * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        }

        public float GetFrustrumWidth()
        {
            return GetFrustrumHeight() * cam.aspect;
        }

        public static TopDownCamera Get()
        {
            return _instance;
        }

        public static Camera GetCamera()
        {
            return _instance.cam;
        }
    }

}