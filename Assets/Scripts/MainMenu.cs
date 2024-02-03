using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image bg;
    public GameObject BG;
    private bool on=false;
    public float fadeSpeed;
    

    void Update()
    {
        if(on)
        {
            float tempAlpha = bg.color.a;
            tempAlpha += Time.deltaTime * fadeSpeed;
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, tempAlpha);
        }
        if(on && bg.color.a >= 1)
        {
            on=false;
            BG.SetActive(false);
            SceneHandler.instance.LoadScene("1-0");
            Debug.Log("a");
        }
    }
    public void Play()
    {
        on=true;
        BG.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
