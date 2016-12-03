using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public Sprite[] splashScreens = new Sprite[9];
    public Image imageToChange;
    private int i = 0;

    void Start()
    {
        imageToChange.sprite = splashScreens[i];
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            i++;
            imageToChange.sprite = splashScreens[i];
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            i--;
            imageToChange.sprite = splashScreens[i];
        }
    }
}