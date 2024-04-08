using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpToOnline : MonoBehaviour
{
    public void Jump()
    {
        SceneManager.LoadScene("Start");
    }
}
