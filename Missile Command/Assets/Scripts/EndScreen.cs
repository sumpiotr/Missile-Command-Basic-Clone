using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(LoadMenu), 5);
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            LoadMenu();
        }
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    
}
