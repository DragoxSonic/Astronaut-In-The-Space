using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager sharedInstance;
    public Canvas menuCanvas;
    public Canvas gameCanvas;
    public Canvas gameOverCanvas;

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    public void ShowMainMenu()
    {
        menuCanvas.enabled = true;
    }
    public void HideMainMenu()
    {
        menuCanvas.enabled = false;
    }
    public void ShowGameCanvas()
    {
        gameCanvas.enabled = true;
    }
    public void HideGameCanvas()
    {
        gameCanvas.enabled = false;
    }
    public void ShowGameOverCanvas()
    {
        gameOverCanvas.enabled = true;
    }
    public void HideGameOverCanvas()
    {
        gameOverCanvas.enabled = false;
    }
    //sintaxis espacial de metodos que dependen de una plataforma
    //if else etc se usan diferente
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Aplication.Quit();

#endif
    }
}
