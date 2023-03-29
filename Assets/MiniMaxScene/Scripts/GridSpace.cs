using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridSpace : MonoBehaviour{
    public Button button;
    public Image defaultImage;
    public Sprite X;
    public Sprite O;
    public TMPro.TMP_Text buttonText;
    private GameController gameController;
    public void SetSpace()
    {
        string playerChar = gameController.GetPlayerSide();
        buttonText.text = playerChar;
        if(playerChar == "X")
        {
            defaultImage.sprite = X;
        }
        if (playerChar == "O")
        {
            defaultImage.sprite = O;
        }
        button.interactable= false;
        gameController.EndTurn();
    }
    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }

}
