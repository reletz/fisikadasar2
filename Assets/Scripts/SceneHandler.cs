using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[Serializable]
public class MessageEvent : UnityEvent<string> {}

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;
    public SceneMessage messagePrefab;
    public MessageEvent onMessageReceived;
    public UnityEvent onLoadWithoutMessage;

    private string message;

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    public void LoadScene(string sceneName)
    {
        if (instance != this) {instance.LoadScene(sceneName); return;}

        if (message != null)
        {
            SceneMessage sceneMessage = Instantiate(messagePrefab);
            sceneMessage.message = message;
            sceneMessage.name = "SceneMessage";
            DontDestroyOnLoad(sceneMessage);
        }

        SceneManager.LoadScene(sceneName);
    }

    public void AddMessage(string newMessage)
    {
        if (instance != this) {instance.AddMessage(newMessage); return;}

        message = newMessage;
    }

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        GameObject messageObject = GameObject.Find("SceneMessage");

        if (messageObject != null)
        {
            SceneMessage sceneMessage = messageObject.GetComponent<SceneMessage>();
            onMessageReceived.Invoke(sceneMessage.message);
            Destroy(messageObject);
        }
        else
        {
            onLoadWithoutMessage.Invoke();
        }
    }
}
