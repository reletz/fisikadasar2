using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tombol : MonoBehaviour
{   
    [SerializeField] string sceneload;
    public void RestartGame()
    {
        SceneManager.LoadScene(sceneload);
    }
}
