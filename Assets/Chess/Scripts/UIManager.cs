using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Aaron Garza
//aaronaagarza.kaz@gmail.com
public class UIManager : MonoBehaviour
{
    GameManager gameManager;
    MiniMax minMax; //hold our algorithm as object

    [Header("===Texts===")]
    public Text TurnText;
    public Text WinnerText;
    public Text TurnCountText;
    public Text DifficultyText;

    [Header("===Buttons===")]
    public Button UndoButton;
    public Button EndTurnButton;
    public Button RestartButton;

    [Header("===Sliders===")]
    public Slider DifficultySlider;

    public static UIManager Instance = null;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; //Singleton of our UI Manager
        }
        else if (Instance != this)
        {
            Destroy(this); //don't allow more than one instance
        }

        gameManager = GameManager.Instance; //set our gamemanager and minmax variables
        minMax = gameManager.MinMax;
    }

    public void Start()
    {
        ChangeDepth(); //Adjust the depth of our search on start.
    }

    //simple function that informs the user whose turn it is
    public void PlayerTurnText(bool _playerTurn)
    {
        if (_playerTurn)
        {
            TurnText.text = "White Turn"; 
        }
        else if (!_playerTurn)
        {
            TurnText.text = "Black Turn";
        }
    }

    public void GameRestart(bool _kingDead, bool _isWhiteWin)
    {
        if (_kingDead)
        {
            RestartButton.gameObject.SetActive(true); //restart only active on game end condition, otherwise user may reset

            if (_isWhiteWin)
            {
                WinnerText.text = "White Wins!";
            }
            else if (!_isWhiteWin)
            {
                WinnerText.text = "Black Wins!";
            }
        }
        else
        {
            return;
        }
    }

    public void TurnCount(int _turnCount)
    {
        TurnCountText.text = "Turns Taken: " + _turnCount.ToString();
    }

    public void ResetLevel(int _level)
    {
        SceneManager.LoadScene(_level);
    }

    public void CheckMoved(bool _playerMoved, bool _kingDead)
    {
        if (_playerMoved && !_kingDead)
        {
            UndoButton.gameObject.SetActive(true);
            EndTurnButton.gameObject.SetActive(true);
        }
        else if (!_playerMoved)
        {
            UndoButton.gameObject.SetActive(false);
            EndTurnButton.gameObject.SetActive(false);
        }
    }

    public void ChangeDepth()
    {
        minMax.MaxDepth = (int)DifficultySlider.value; //translate our slider into our depth difficulty

        DepthNumber();

        Debug.Log("MaxDepth : " + minMax.MaxDepth);
    }

    public void DepthNumber()
    {
        DifficultyText.text = "Difficulty: " + minMax.MaxDepth.ToString();
    }

    public void ExitGame()
    {
        Application.Quit(); //ends game
    }
}
