using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sm : MonoBehaviour
{
    public void Login()
    {
        SceneManager.LoadScene("Game");
    }
}
