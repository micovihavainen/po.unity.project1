using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{

    [SerializeField] private AudioSource beep;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            beep.Play();
            SceneManager.LoadScene("Level1");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            beep.Play();
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Music volume
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            // fx volume
        }
    }
}
