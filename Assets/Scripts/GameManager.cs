using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    menu,
    inGame,
    gameOver
}

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.menu;

    public static GameManager sharedInstance;

    private PlayerController controller;

    public int collectedObject = 0;

    


    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && currentGameState != GameState.inGame)
        {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }
    }

    public void StartGame()
    {
        SetGameState(GameState.inGame);

    }
    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }
    public void BackToMenu()
    {
        SetGameState(GameState.menu);
    }

    private void SetGameState(GameState newGameState)
    {
        if(newGameState == GameState.menu)
        {
            MenuManager.sharedInstance.HideGameCanvas();
            MenuManager.sharedInstance.ShowMainMenu();
            MenuManager.sharedInstance.HideGameOverCanvas();
            
        }
        else if(newGameState == GameState.inGame)
        {
            LevelManager.sharedInstance.RemoveAllLevelBlocks();
            LevelManager.sharedInstance.GenerateInitialBlocks();
            
            controller.StartGame();
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.ShowGameCanvas();
            MenuManager.sharedInstance.HideGameOverCanvas();
            GetComponent<AudioSource>().Play();
            //coinsText.text = "0";

        }
        else if(newGameState == GameState.gameOver)
        {
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.HideGameCanvas();
            MenuManager.sharedInstance.ShowGameOverCanvas();
        }

        this.currentGameState = newGameState;
    }

    public void CollectObject(Collectable collectable)
    {
        collectedObject += collectable.value;
    }
}
