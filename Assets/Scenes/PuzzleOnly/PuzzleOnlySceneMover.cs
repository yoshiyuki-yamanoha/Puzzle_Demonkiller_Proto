using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleOnlySceneMover : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Start"))
            SceneManager.LoadScene("TitleScene");
    }
}
