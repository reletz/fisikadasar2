using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MinigameTic : MonoBehaviour
{
    public GameObject X1;
    public GameObject X2;
    public GameObject X3;
    public GameObject X4;
    public GameObject X5;
    public GameObject X6;
    public GameObject X7;
    public GameObject X8;
    public GameObject X9;
    public GameObject O1;
    public GameObject O2;
    public GameObject O3;
    public GameObject O4;
    public GameObject O5;
    public GameObject O6;
    public GameObject O7;
    public GameObject O8;
    public GameObject O9;
    
    private bool onChange = false;
    private bool unPressable=false;

    private bool win=false;
    private bool lose=false;
    private bool on1 = false;
    private bool on2 = false;
    private bool on3 = false;
    private bool on4 = false;
    private bool on5 = false;
    private bool on6 = false;
    private bool on7 = false;
    private bool on8 = false;
    private bool on9 = false;

    private bool Xon1 = false;
    private bool Xon2 = false;
    private bool Xon3 = false;
    private bool Xon4 = false;
    private bool Xon5 = false;
    private bool Xon6 = false;
    private bool Xon7 = false;
    private bool Xon8 = false;
    private bool Xon9 = false;

    private bool Oon1 = false;
    private bool Oon2 = false;
    private bool Oon3 = false;
    private bool Oon4 = false;
    private bool Oon5 = false;
    private bool Oon6 = false;
    private bool Oon7 = false;
    private bool Oon8 = false;
    private bool Oon9 = false;
    
    public Image bg;
    public float fadeSpeed;
    // Start is called before the first frame update
    void Start()
    {
        onChange=true;
        //Adds flipping page sfx
        AudioManager.Instance.PlaySFX("Paper");
    }

    // Update is called once per frame
    void Update()
    {
        //Fades In
        if(onChange)
        {
            float tempAlpha = bg.color.a;
            tempAlpha -= Time.deltaTime * fadeSpeed;
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, tempAlpha);
        }
        if(onChange && bg.color.a <= 0)
        {
            onChange=false;
            bg.enabled=false;
            Debug.Log("a");
        }


        //Win or Lose
        if(Xon1 && Xon2 && Xon3 || Xon4 && Xon5 && Xon6 || Xon7 && Xon8 && Xon9 || Xon1 && Xon4 && Xon7 || Xon2 && Xon5 && Xon8 || Xon3 && Xon6 && Xon9 || Xon1 && Xon5 && Xon9 || Xon3 && Xon5 && Xon7)
        {
            win=true;
            Debug.Log("You Win!");
        }
        if(Oon1 && Oon2 && Oon3 || Oon4 && Oon5 && Oon6 || Oon7 && Oon8 && Oon9 || Oon1 && Oon4 && Oon7 || Oon2 && Oon5 && Oon8 || Oon3 && Oon6 && Oon9 || Oon1 && Oon5 && Oon9 || Oon3 && Oon5 && Oon7)
        {
            lose=true;
            Debug.Log("You Lose!");
        }
        // Win or Lose

        // Fades Out
        if(win)
        {
            bg.enabled=true;
            float tempAlpha = bg.color.a;
            tempAlpha += Time.deltaTime * fadeSpeed;
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, tempAlpha);
        }
        if(lose)
        {
            bg.enabled=true;
            float tempAlpha = bg.color.a;
            tempAlpha += Time.deltaTime * fadeSpeed;
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, tempAlpha);
        }

        // CHANGE SCENE
        if(win && bg.color.a >= 1)
        {
            //Put GetInt on the dialog scene & make it fastforward to Mom's response you winning
            PlayerPrefs.SetInt("Win",1);
            SceneHandler.instance.AddMessage("TicTacToeWin");
            SceneHandler.instance.LoadScene("1-0");
        }
        if(lose && bg.color.a >= 1)
        {
            //Put GetInt on the dialog scene & make it fastforward to Mom's response you losing
            PlayerPrefs.SetInt("Lose",1);
            SceneHandler.instance.AddMessage("TicTacToeLose");
            SceneHandler.instance.LoadScene("1-0");
        }
    }

    //Restart
    public void ReloadScene()
    {
        Time.timeScale=1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Mom's Turn
    private IEnumerator momTurn()
    {
        yield return new WaitForSeconds(2f);
        Change();
    }
    
    private void Change()
    {
        StartCoroutine(momWrite());
    }

    private IEnumerator momWrite()
    {
        int randomPick = Random.Range(1, 10);
        yield return new WaitForSeconds(0.01f);
        if(randomPick==1)
        {
            if(!on1)
            {
                momGet1();
            }
            else
            {
                StartCoroutine(momWrite());
            }
        }
        if(randomPick==2)
        {
            if(!on2)
            {
                momGet2();
            }
            else
            {
                StartCoroutine(momWrite());
            }
        }
        if(randomPick==3)
        {
            if(!on3)
            {
                momGet3();
            }
            else
            {
                StartCoroutine(momWrite());
            }
        }
        if(randomPick==4)
        {
            if(!on4)
            {
                momGet4();
            }
            else
            {
                StartCoroutine(momWrite());
            }
        }
        if(randomPick==5)
        {
            if(!on5)
            {
                momGet5();
            }
            else
            {
                StartCoroutine(momWrite());
            }
        }
        if(randomPick==6)
        {
            if(!on6)
            {
                momGet6();
            }
            else
            {
                StartCoroutine(momWrite());
            }
        }
        if(randomPick==7)
        {
            if(!on7)
            {
                momGet7();
            }
            else
            {
                StartCoroutine(momWrite());
            }
        }
        if(randomPick==8)
        {
            if(!on8)
            {
                momGet8();
            }
            else
            {
                StartCoroutine(momWrite());
            }
        }
        if(randomPick==9)
        {
            if(!on9)
            {
                momGet9();
            }
            else
            {
                StartCoroutine(momWrite());
            }
        }
    }

    
    //Write Player
    public void Get1()
    {
        if(!on1 && !unPressable)
        {
            on1=true;
            Xon1=true;
            X1.SetActive(true);
            unPressable=true;
            AudioManager.Instance.PlaySFX("Write");
            StartCoroutine(momTurn());
        }
    }
    public void Get2()
    {
        if(!on2 && !unPressable)
        {
            on2=true;
            Xon2=true;
            X2.SetActive(true);
            unPressable=true;
            AudioManager.Instance.PlaySFX("Write");
            StartCoroutine(momTurn());
        }
    }
    public void Get3()
    {
        if(!on3 && !unPressable)
        {
            on3=true;
            Xon3=true;
            X3.SetActive(true);
            unPressable=true;
            AudioManager.Instance.PlaySFX("Write");
            StartCoroutine(momTurn());
        }
    }
    public void Get4()
    {
        if(!on4 && !unPressable)
        {
            on4=true;
            Xon4=true;
            X4.SetActive(true);
            unPressable=true;
            AudioManager.Instance.PlaySFX("Write");
            StartCoroutine(momTurn());
        }
    }
    public void Get5()
    {
        if(!on5 && !unPressable)
        {
            on5=true;
            Xon5=true;
            X5.SetActive(true);
            unPressable=true;
            AudioManager.Instance.PlaySFX("Write");
            StartCoroutine(momTurn());
        }
    }
    public void Get6()
    {
        if(!on6 && !unPressable)
        {
            on6=true;
            Xon6=true;
            X6.SetActive(true);
            unPressable=true;
            AudioManager.Instance.PlaySFX("Write");
            StartCoroutine(momTurn());
        }
    }
    public void Get7()
    {
        if(!on7 && !unPressable)
        {
            on7=true;
            Xon7=true;
            X7.SetActive(true);
            unPressable=true;
            AudioManager.Instance.PlaySFX("Write");
            StartCoroutine(momTurn());
        }
    }
    public void Get8()
    {
        if(!on8 && !unPressable)
        {
            on8=true;
            Xon8=true;
            X8.SetActive(true);
            unPressable=true;
            AudioManager.Instance.PlaySFX("Write");
            StartCoroutine(momTurn());
        }
    }
    public void Get9()
    {
        if(!on9 && !unPressable)
        {
            on9=true;
            Xon9=true;
            X9.SetActive(true);
            unPressable=true;
            AudioManager.Instance.PlaySFX("Write");
            StartCoroutine(momTurn());
        }
    }

    //Write Mom
    public void momGet1()
    {
        on1=true;
        Oon1=true;
        O1.SetActive(true);
        unPressable=false;
        AudioManager.Instance.PlaySFX("Write");
    }
    public void momGet2()
    {
        on2=true;
        Oon2=true;
        O2.SetActive(true);
        unPressable=false;
        AudioManager.Instance.PlaySFX("Write");
    }
    public void momGet3()
    {
        on3=true;
        Oon3=true;
        O3.SetActive(true);
        unPressable=false;
        AudioManager.Instance.PlaySFX("Write");
    }
    public void momGet4()
    {
        on4=true;
        Oon4=true;
        O4.SetActive(true);
        unPressable=false;
        AudioManager.Instance.PlaySFX("Write");
    }
    public void momGet5()
    {
        on5=true;
        Oon5=true;
        O5.SetActive(true);
        unPressable=false;
        AudioManager.Instance.PlaySFX("Write");
    }
    public void momGet6()
    {
        on6=true;
        Oon6=true;
        O6.SetActive(true);
        unPressable=false;
        AudioManager.Instance.PlaySFX("Write");
    }
    public void momGet7()
    {
        on7=true;
        Oon7=true;
        O7.SetActive(true);
        unPressable=false;
        AudioManager.Instance.PlaySFX("Write");
    }
    public void momGet8()
    {
        on8=true;
        Oon8=true;
        O8.SetActive(true);
        unPressable=false;
        AudioManager.Instance.PlaySFX("Write");
    }
    public void momGet9()
    {
        on9=true;
        Oon9=true;
        O9.SetActive(true);
        unPressable=false;
        AudioManager.Instance.PlaySFX("Write");
    }

}

