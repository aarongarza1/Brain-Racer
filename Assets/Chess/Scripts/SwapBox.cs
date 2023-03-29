using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Aaron Garza
//aaronaagarza.kaz@gmail.com
public class SwapBox : MonoBehaviour
{
    public MoveData move;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && move != null)
        {
            gameManager.SwapPieces(move);
            gameManager.TempMove = move; //store for the undo function
        }
    }
}
