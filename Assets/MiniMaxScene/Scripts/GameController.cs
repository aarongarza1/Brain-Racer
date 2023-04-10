using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class Player
{
    public Image panel;
    public TMPro.TMP_Text text;
}
[System.Serializable]
public class PlayerColor
{
    public Color textColor;
    public Color panelColor;
}

public class GameController : MonoBehaviour
{
    [SerializeField] public TicTacMiniMax miniMaxGamer;
    public TMPro.TMP_Text[] buttonList;
    public static List<string> boardState = new List<string>();
    private string playerSide;
    public GameObject gameOverPanel;
    public TMPro.TMP_Text gameOverText;
    private int moveCount;
    public GameObject restartButton;

    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    private string computerSide;
    public bool playerMove;
    public float delay;
    private int cpuChoice;
    public Sprite O;
    public GridSpace[] gridSpace;

    private void Awake(){
        gameOverPanel.SetActive(false);
        SetGameControllerReferenceOnButtons();
        playerSide = "X";
        computerSide = "O";
        moveCount = 0;
        restartButton.SetActive(false);
        SetPlayerColors(playerX, playerO);
    }

    private void Update()
    {
        if(playerMove == false)
        {
            
            delay += delay * Time.deltaTime;
            if (delay >= 30)
            {
                for (int i = 0; i < buttonList.Length; i++)
                {
                    boardState.Add(buttonList[i].text);
                }
                cpuChoice = miniMaxGamer.GetBestMove(boardState); ; //call to minimax rather than random. buttonList keeps track of when the text of a button is set to X or O and is ""empty otherwise for determining score. Player is X, CPU is O
                print(cpuChoice);
                if(cpuChoice == -1)
                {
                    
                }
                else if (buttonList[cpuChoice].GetComponentInParent<Button> ().interactable ==true)
                {
                    buttonList[cpuChoice].text = GetComputerSide();
                    buttonList[cpuChoice].GetComponentInParent<GridSpace>().defaultImage.sprite = O;
                    buttonList[cpuChoice].GetComponentInParent<Button>().interactable = false;
                    EndTurn();
                }
            }
            boardState.Clear();
        }
    }   

    void SetGameControllerReferenceOnButtons(){
        for (int i = 0; i < buttonList.Length; i++)        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public string GetPlayerSide(){
        return playerSide;
    }
    public string GetComputerSide()
    {
        return computerSide;
    }

    public void EndTurn()
    {
        moveCount++;
        if (CheckWin() == 1)
        {
            GameOver(playerSide);
        }
        else if (CheckWin() == 0)
        {
            GameOver(computerSide);
        }
        else if (moveCount >= 16)
        {
            GameOver("draw");
        }
        else
        {
            ChangeSides();
            delay = 10;
        }
    }

    void SetPlayerColors(Player newPlayer, Player oldPlayer) {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }
    void GameOver(string winningPlayer)
    {
        SetBoardInteractable(false);

        if (winningPlayer == "draw")
        {
            SetGameOverText("It's a draw!");
            ScoreVariables.score = 0;
        }
        else { 
            if (winningPlayer == "X")
            {
                SetGameOverText("You win!");
                ScoreVariables.score = 1;
            }
            if (winningPlayer == "O")
            {
                SetGameOverText("You lost :(");
                ScoreVariables.score = 0;
            }
        }

        restartButton.SetActive(true);
    }
    void ChangeSides()
    {
        playerSide = (playerSide == "X") ? "X" : "O";
        playerMove = (playerMove == true) ? false : true;
        if (playerMove== true)
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
    }

    public int CheckWin() //low amount of win states so it can be brute forced
    {
        //horiz 1
        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide && buttonList[3].text == playerSide)
        {
            return 1;
        }
        //horiz 2
        else if (buttonList[4].text == playerSide && buttonList[5].text == playerSide && buttonList[6].text == playerSide && buttonList[7].text == playerSide)
        {
            return 1;
        }
        //horiz 3
        else if (buttonList[8].text == playerSide && buttonList[9].text == playerSide && buttonList[10].text == playerSide && buttonList[11].text == playerSide)
        {
            return 1;
        }
        //horiz 4
        else if (buttonList[12].text == playerSide && buttonList[13].text == playerSide && buttonList[14].text == playerSide && buttonList[15].text == playerSide)
        {
            return 1;
        }
        //vert 1
        else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide && buttonList[12].text == playerSide)
        {
            return 1;
        }
        //vert 2
        else if (buttonList[1].text == playerSide && buttonList[5].text == playerSide && buttonList[9].text == playerSide && buttonList[13].text == playerSide)
        {
            return 1;
        }
        //vert 3
        else if (buttonList[2].text == playerSide && buttonList[6].text == playerSide && buttonList[10].text == playerSide && buttonList[14].text == playerSide)
        {
            return 1;
        }
        //vert 4
        else if (buttonList[3].text == playerSide && buttonList[7].text == playerSide && buttonList[11].text == playerSide && buttonList[15].text == playerSide)
        {
            return 1;
        }
        //diag 1
        else if (buttonList[0].text == playerSide && buttonList[5].text == playerSide && buttonList[10].text == playerSide && buttonList[15].text == playerSide)
        {
            return 1;
        }
        //diag 2
        else if (buttonList[3].text == playerSide && buttonList[6].text == playerSide && buttonList[9].text == playerSide && buttonList[12].text == playerSide)
        {
            return 1;
        }
        else if (buttonList[0].text == computerSide && buttonList[1].text == computerSide && buttonList[2].text == computerSide && buttonList[3].text == computerSide)
        {
            return 0;
        }
        //horiz 2
        else if (buttonList[4].text == computerSide && buttonList[5].text == computerSide && buttonList[6].text == computerSide && buttonList[7].text == computerSide)
        {
            return 0;
        }
        //horiz 3
        else if (buttonList[8].text == computerSide && buttonList[9].text == computerSide && buttonList[10].text == computerSide && buttonList[11].text == computerSide)
        {
            return 0;
        }
        //horiz 4
        else if (buttonList[12].text == computerSide && buttonList[13].text == computerSide && buttonList[14].text == computerSide && buttonList[15].text == computerSide)
        {
            return 0;
        }
        //vert 1
        else if (buttonList[0].text == computerSide && buttonList[4].text == computerSide && buttonList[8].text == computerSide && buttonList[12].text == computerSide)
        {
            return 0;
        }
        //vert 2
        else if (buttonList[1].text == computerSide && buttonList[5].text == computerSide && buttonList[9].text == computerSide && buttonList[13].text == computerSide)
        {
            return 0;
        }
        //vert 3
        else if (buttonList[2].text == computerSide && buttonList[6].text == computerSide && buttonList[10].text == computerSide && buttonList[14].text == computerSide)
        {
            return 0;
        }
        //vert 4
        else if (buttonList[3].text == computerSide && buttonList[7].text == computerSide && buttonList[11].text == computerSide && buttonList[15].text == computerSide)
        {
            return 0;
        }
        //diag 1
        else if (buttonList[0].text == computerSide && buttonList[5].text == computerSide && buttonList[10].text == computerSide && buttonList[15].text == computerSide)
        {
            return 0;
        }
        //diag 2
        else if (buttonList[3].text == computerSide && buttonList[6].text == computerSide && buttonList[9].text == computerSide && buttonList[12].text == computerSide)
        {
            return 0;
        }
        else
        {

            return -1;

        }
    }
    void SetGameOverText (string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    public void RestartGame()
    {
        playerSide = "X";
        moveCount= 0;
        gameOverPanel.SetActive(false);
        SetBoardInteractable(true);
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
            buttonList[i].spriteAsset= null;
        }
        for (int i = 0; i < 15; i++)
        {
            gridSpace[i].defaultImage.sprite = null;
        }
        SetPlayerColors(playerX, playerO);
        restartButton.SetActive(false);
        SceneManager.LoadScene("SampleScene");
    }
    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }
}
