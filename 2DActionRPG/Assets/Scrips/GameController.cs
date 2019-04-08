using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    void Update()
    {
        if (Controller.roooms < Controller.roomscleared)
            SceneManager.LoadScene("Credit");
    }
}
