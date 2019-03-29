﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    [Header("3 = Down")]
    [Header("2 = Up")]
    [Header("1 = Right")]
    [Header("0 = Left")]
    public bool[] has_doors = new bool[4];
    public Object left_room;
    public Object right_room;
    public Object up_room;
    public Object down_room;

}