using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    public ScreenFader screenFader;
    public Board board;

    // Start is called before the first frame update

    public void OutOfMove()
    {
        screenFader.FadeOn(true);
    }

    public void ResetGame()
    {
        screenFader.FadeOff();
        board.ClearBoard();
        board.FillBoard(10, 0.2f);
    }
    public void BackToGame()
    {
        screenFader.FadeOff();
    }
}
