using System.Collections.Generic;
using UnityEngine;

public class TicTacMiniMax : MonoBehaviour
{
    public int maxDepth = 10; // Maximum depth for the minimax algorithm
    public string aiPlayer = "O"; // AI player is "O"
    public string humanPlayer = "X"; // Human player is "X"

   
    private int EvaluateBoard(List<string> boardState)
    {
        // Check for horizontal wins
        for (int i = 0; i < 4; i++)
        {
            int rowIndex = i * 4;
            string row = boardState[rowIndex] + boardState[rowIndex + 1] + boardState[rowIndex + 2] + boardState[rowIndex + 3];
            if (row == "OOOO")
            {
                return 100;
            }
            else if (row == "XXXX")
            {
                return -100;
            }
        }

        // Check for vertical wins
        for (int i = 0; i < 4; i++)
        {
            string column = boardState[i] + boardState[i + 4] + boardState[i + 8] + boardState[i + 12];
            if (column == "OOOO")
            {
                return 100;
            }
            else if (column == "XXXX")
            {
                return -100;
            }
        }

        // Check for diagonal wins
        string diagonal1 = boardState[0] + boardState[5] + boardState[10] + boardState[15];
        string diagonal2 = boardState[3] + boardState[6] + boardState[9] + boardState[12];
        if (diagonal1 == "OOOO" || diagonal2 == "OOOO")
        {
            return 100;
        }
        else if (diagonal1 == "XXXX" || diagonal2 == "XXXX")
        {
            return -100;
        }

        // No winner yet, return 0
        return 0;
    }
    public int Minimax(List<string> boardState, int depth, int alpha, int beta, bool isMaximizingPlayer)
    {
        int score = EvaluateBoard(boardState);
        if (score == 100 || score == -100 || depth == 0)
        {
            return score;
        }

        int bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;

        for (int i = 0; i < 16; i++)
        {
            if (IsValidMove(boardState, i))
            {
                boardState[i] = isMaximizingPlayer ? "O" : "X";

                int currentScore = Minimax(boardState, depth - 1, alpha, beta, !isMaximizingPlayer);

                if (isMaximizingPlayer)
                {
                    bestScore = Mathf.Max(bestScore, currentScore);
                    alpha = Mathf.Max(alpha, currentScore);
                }
                else
                {
                    bestScore = Mathf.Min(bestScore, currentScore);
                    beta = Mathf.Min(beta, currentScore);
                }

                boardState[i] = "";
                if (beta <= alpha)
                {
                    break;
                }
            }
        }

        return bestScore;
    }
    public int GetBestMove(List<string> boardState)
    {
        int bestScore = int.MinValue;
        int bestMove = -1;

        for (int i = 0; i < 16; i++)
        {
            if (IsValidMove(boardState, i))
            {
                boardState[i] = "O";
                int currentScore = Minimax(boardState, 3, int.MinValue, int.MaxValue, false);
                boardState[i] = "";

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestMove = i;
                }
            }
        }

        return bestMove;
    }
    private bool IsValidMove(List<string> boardState, int moveIndex)
    {
        // Check if the move is within the bounds of the board
        if (moveIndex < 0 || moveIndex >= boardState.Count)
        {
            return false;
        }

        // Check if the move is on an empty space
        if (boardState[moveIndex] != "")
        {
            return false;
        }

        // The move is valid
        return true;
    }
}