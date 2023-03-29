using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public TMPro.TMP_Text[] buttonList;
    private string playerSide;
    public GameObject gameOverPanel;
    public TMPro.TMP_Text gameOverText;
    private int moveCount;
    public GameObject restartButton;

    private void Awake(){
        gameOverPanel.SetActive(false);
        SetGameControllerReferenceOnButtons();
        playerSide = "X";
        moveCount= 0;
        restartButton.SetActive(false);
    }

    void SetGameControllerReferenceOnButtons(){
        for (int i = 0; i < buttonList.Length; i++)        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public string GetPlayerSide(){
        return playerSide;
    }


    public void EndTurn()
    {
        moveCount++;
        if (CheckWin())
        {
            GameOver();
        }
        if(moveCount >= 16)
        {
            SetGameOverText("It's a draw!");
        }
        ChangeSides();
    }
    void GameOver()
    {
        SetBoardInteractable(false);


        if (playerSide == "X")
        {
            SetGameOverText("You win!");
        }
        if (playerSide == "O")
        {
            SetGameOverText("You lost :(");
        }

        restartButton.SetActive(true);
    }
    void ChangeSides()
    {
        playerSide = (playerSide == "X") ? "O" : "X";
    }

    public bool CheckWin() //low amount of win states so it can be brute forced
    {
        //horiz 1
        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide && buttonList[3].text == playerSide){
            return true;
        }
        //horiz 2
        if (buttonList[4].text == playerSide && buttonList[5].text == playerSide && buttonList[6].text == playerSide && buttonList[7].text == playerSide){
            return true;
        }
        //horiz 3
        if (buttonList[8].text == playerSide && buttonList[9].text == playerSide && buttonList[10].text == playerSide && buttonList[11].text == playerSide)
        {
            return true;
        }
        //horiz 4
        if (buttonList[12].text == playerSide && buttonList[13].text == playerSide && buttonList[14].text == playerSide && buttonList[15].text == playerSide)
        {
            return true;
        }
        //vert 1
        if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide && buttonList[12].text == playerSide)
        {
            return true;
        }
        //vert 2
        if (buttonList[1].text == playerSide && buttonList[5].text == playerSide && buttonList[9].text == playerSide && buttonList[13].text == playerSide)
        {
            return true;
        }
        //vert 3
        if (buttonList[2].text == playerSide && buttonList[6].text == playerSide && buttonList[10].text == playerSide && buttonList[14].text == playerSide)
        {
            return true;
        }
        //vert 4
        if (buttonList[3].text == playerSide && buttonList[7].text == playerSide && buttonList[11].text == playerSide && buttonList[15].text == playerSide)
        {
            return true;
        }
        //diag 1
        if (buttonList[0].text == playerSide && buttonList[5].text == playerSide && buttonList[10].text == playerSide && buttonList[15].text == playerSide)
        {
            return true;
        }
        //diag 2
        if (buttonList[3].text == playerSide && buttonList[6].text == playerSide && buttonList[9].text == playerSide && buttonList[12].text == playerSide)
        {
            return true;
        }

        return false;
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

        restartButton.SetActive(false);
    }
    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }
}
