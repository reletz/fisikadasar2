using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneZeroEvents : MonoBehaviour
{
    public static OneZeroEvents instance;

    [SerializeField] private BaseDialogue ticTacToeWinEntryPoint;
    [SerializeField] private BaseDialogue ticTacToeLoseEntryPoint;
    [SerializeField] private Sprite airportBackground;
    [SerializeField] private Sprite mainCharacterSprite;
    [SerializeField] private Sprite momSprite;
    [SerializeField] private Sprite emptySprite;

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    public void OnLoadWithMessage(string message)
    {
        if (message == "TicTacToeWin")
        {
            LoadSceneOnAirport();
            ticTacToeWinEntryPoint.Enter();
        }
        else if (message == "TicTacToeLose")
        {
            LoadSceneOnAirport();
            ticTacToeLoseEntryPoint.Enter();
        }
    }

    public void OnLoadWithoutMessage()
    {
        LoadStartingScene();
    }

    public void LoadSceneOnAirport()
    {
        if (instance != this) {instance.LoadSceneOnAirport(); return;}

        BackgroundManager.instance.SwitchBackground(airportBackground);

        ActorManager.instance.NewCharacter("Mom");
        ActorManager.instance.SelectCharacter("Mom");
        ActorManager.instance.ChangeCharacterSprite(momSprite);
        ActorManager.instance.ChangeCharacterMouth(emptySprite);
        ActorManager.instance.ChangeCharacterEyebrow(emptySprite);
        ActorManager.instance.ChangeCharacterEye(emptySprite);
        ActorManager.instance.ChangeCharacterExpression(emptySprite);

        ActorManager.instance.NewCharacter("MC");
        ActorManager.instance.SelectCharacter("MC");
        ActorManager.instance.ChangeCharacterSprite(mainCharacterSprite);
        ActorManager.instance.ChangeCharacterMouth(emptySprite);
        ActorManager.instance.ChangeCharacterEyebrow(emptySprite);
        ActorManager.instance.ChangeCharacterEye(emptySprite);
        ActorManager.instance.ChangeCharacterExpression(emptySprite);
        ActorManager.instance.ScaleCharacterX(-5);
        ActorManager.instance.PositionCharacterX(5);
    }

    public void LoadStartingScene()
    {
        if (instance != this) {instance.LoadStartingScene(); return;}
        DialogueReader.instance.currentDialogue.Enter();
    }

    public void LoadEndScene()
    {
        if (instance != this) {instance.LoadEndScene(); return;}
        SceneHandler.instance.LoadScene("1-1");
    }

    void Awake()
    {
        Initialize();
    }
}
