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
    public Text internalText;
    public void SetSpace()
    {
        if (gameController.playerMove == true)
        {
            string playerChar = gameController.GetPlayerSide();
            print(playerChar);
            buttonText.text = playerChar;
            if (playerChar == "X")
            {
                defaultImage.sprite = X;
            }
            if (playerChar == "O")
            {
                defaultImage.sprite = O;
            }
            button.interactable = false;
            gameController.EndTurn();
        }
    }
    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }


}
