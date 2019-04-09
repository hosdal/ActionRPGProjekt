using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour
{
    public void easy()
    {
        Controller.roooms = 5;
        SceneManager.LoadScene("Maps");
    }

    public void normal()
    {
        Controller.roooms = 20;
        SceneManager.LoadScene("Maps");
    }

    public void hard()
    {
        Controller.roooms = 30;
        SceneManager.LoadScene("Maps");
    }
}
