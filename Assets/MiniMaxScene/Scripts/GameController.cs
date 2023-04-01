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
    MiniMax miniMaxGamer = new MiniMax();
    public TMPro.TMP_Text[] buttonList;
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
    public int winLoss;

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
                cpuChoice = Random.Range(0, 15); //call to minimax rather than random. buttonList keeps track of when the text of a button is set to X or O and is ""empty otherwise for determining score. Player is X, CPU is O
                if (buttonList[cpuChoice].GetComponentInParent<Button> ().interactable ==true)
                {
                    buttonList[cpuChoice].text = GetComputerSide();
                    buttonList[cpuChoice].GetComponentInParent<Button>().interactable = false;
                    EndTurn();
                }
            }
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
        if (CheckWin())
        {
            GameOver(playerSide);
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
            winLoss = 0;
        }
        else { 
            if (playerSide == "X")
            {
                SetGameOverText("You win!");
                winLoss = 1;
            }
            if (playerSide == "O")
            {
                SetGameOverText("You lost :(");
                winLoss = 0;
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

    public bool CheckWin() //low amount of win states so it can be brute forced
    {
        //horiz 1
        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide && buttonList[3].text == playerSide)
        {
            return true;
        }
        //horiz 2
        else if (buttonList[4].text == playerSide && buttonList[5].text == playerSide && buttonList[6].text == playerSide && buttonList[7].text == playerSide)
        {
            return true;
        }
        //horiz 3
        else if (buttonList[8].text == playerSide && buttonList[9].text == playerSide && buttonList[10].text == playerSide && buttonList[11].text == playerSide)
        {
            return true;
        }
        //horiz 4
        else if (buttonList[12].text == playerSide && buttonList[13].text == playerSide && buttonList[14].text == playerSide && buttonList[15].text == playerSide)
        {
            return true;
        }
        //vert 1
        else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide && buttonList[12].text == playerSide)
        {
            return true;
        }
        //vert 2
        else if (buttonList[1].text == playerSide && buttonList[5].text == playerSide && buttonList[9].text == playerSide && buttonList[13].text == playerSide)
        {
            return true;
        }
        //vert 3
        else if (buttonList[2].text == playerSide && buttonList[6].text == playerSide && buttonList[10].text == playerSide && buttonList[14].text == playerSide)
        {
            return true;
        }
        //vert 4
        else if (buttonList[3].text == playerSide && buttonList[7].text == playerSide && buttonList[11].text == playerSide && buttonList[15].text == playerSide)
        {
            return true;
        }
        //diag 1
        else if (buttonList[0].text == playerSide && buttonList[5].text == playerSide && buttonList[10].text == playerSide && buttonList[15].text == playerSide)
        {
            return true;
        }
        //diag 2
        else if (buttonList[3].text == playerSide && buttonList[6].text == playerSide && buttonList[9].text == playerSide && buttonList[12].text == playerSide)
        {
            return true;
        }
        else
        {

            return false;

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
