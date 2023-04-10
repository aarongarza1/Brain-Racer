using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMaxAlg : MonoBehaviour
{
    //reference to GameController
    public GameController gameController;

    public int aiPick;
    //If we want the tic tac toe board to slow down
    public bool visualizeAI = false;
    private int fieldsLeft;
    public bool useShortcuts = true;

    // Will hold the current values of the MinMax algorithm
    private int recursionScore;
    private int optimalScoreButtonIndex = -1;

    private TMPro.TMP_Text GetText(TMPro.TMP_Text button)
    {
        return button.GetComponent<TMPro.TMP_Text>();
    }

    // Use some variety and use random to determine an optimal start field. Index will be 0, 2, 6 or 8
    private int RandomCorner()
    {
        optimalScoreButtonIndex = (int)Mathf.Floor(UnityEngine.Random.Range(0, 4));
        if (optimalScoreButtonIndex == 1)
        {
            optimalScoreButtonIndex = 6;
        }
        else if (optimalScoreButtonIndex == 3)
        {
            optimalScoreButtonIndex = 8;
        }
        return optimalScoreButtonIndex;
    }

    private bool IsFieldFree(int index) => GetText(gameController.buttonList[index]).text.Length == 0;

    private IEnumerator AiTurnCoroutine()
    {

        // Call the MinMax algorithm. It will store the (for the player) worst move in optimalScoreButtonIndex.
        // What is worst for the player, is the best for the AI.
        IEnumerator minMaxEnumerator = MinMaxCoroutine(1);
        if (visualizeAI)
        {
            // Take breaks between all steps so we can see it
            yield return StartCoroutine(minMaxEnumerator);
        }
        else
        {
            // Force the coroutine to do everything in one frame
            while (minMaxEnumerator.MoveNext()) { }
        }
    }

    private bool CheckBaseCaseAndShortcuts()
    {
        if (fieldsLeft <= 0)
        {
            recursionScore = 0;
            return true;
        }

        if (!useShortcuts)
        {
            return false;
        }

        // No need to calculate anything if all fields are free - any corner is the best.
        // But let's use that chance for some variety and use random. Index will be 0, 2, 6 or 8
        if (fieldsLeft == 9)
        {
            RandomCorner();
            return true;
        }
        // Shortcut for the optimal second move after an opening
        if (fieldsLeft == 8)
        {
            // If the other player used the middle. go for any corner
            if (!GetText(gameController.buttonList[4]).text.Equals(""))
            {
                RandomCorner();
            }
            else
            { // Else the middle is always the best
                optimalScoreButtonIndex = 4;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Min Max algorithm to find the best and worse moves.
    /// This Method stores the current best and worst moves in
    /// highestCurrentScoreIndex and lowestCurrentScoreIndex as a side effect.
    /// </summary>
    /// <param name="depth">Depth - the number of recursion step for weighting the scores</param>
    /// <returns>The sum of scores of all possible steps from the current recursion level downwards (stored in recursionScore)</returns>
    private IEnumerator MinMaxCoroutine(int depth)
    {
        // Base case and shortcuts (hard coded moves) to stop recursion
        if (CheckBaseCaseAndShortcuts())
        {
            yield break;
        }

        // We want to store which field gives us the best (player) or the worst (CPU) score
        int currentBestScore = gameController.playerMove ? Int32.MinValue : Int32.MaxValue;
        int currentOptimalScoreButtonIndex = -1;

        // Find next free field
        int fieldIndex = 0;
        while (fieldIndex < gameController.buttonList.Length)
        {
            if (IsFieldFree(fieldIndex))
            {
                TMPro.TMP_Text button = gameController.buttonList[fieldIndex];
                int currentScore = 0;

                bool endRecursion = false;

                // Some delay to make it possible to see single steps
                if (visualizeAI && 1 /*algorithm step duration*/ > 0)
                {
                    yield return new WaitForSeconds(1 /*algorithmStepDuration*/);
                }

                // End iteration and recursion level when we win, because we don't need to go deeper
                if (SetMarkAndCheckForWin(button))
                {
                    // Debug.Log("Found a winner: " + GetText(button).text);
                    currentScore = (gameController.playerMove ? 1 : -1) * (10 - depth);
                    endRecursion = true;
                }
                else if (fieldsLeft > 0)
                {
                    // If there are fields left after the SetMarkAndCheckForWin we can go deeper in the recursion
                    gameController.playerMove = !gameController.playerMove; // Switch turns - in the next step we want to simulate the other player

                    IEnumerator minMaxEnumerator = MinMaxCoroutine(depth + 1);
                    if (visualizeAI)
                    {
                        // Take breaks between all steps so we can see it
                        yield return StartCoroutine(minMaxEnumerator);
                    }
                    else
                    {
                        // Force the coroutine to do everything in one frame
                        while (minMaxEnumerator.MoveNext()) { }
                    }
                    currentScore = recursionScore;
                    gameController.playerMove = !gameController.playerMove; // Switch turns back
                }

                if ((gameController.playerMove && currentScore > currentBestScore) || (!gameController.playerMove && currentScore < currentBestScore))
                {
                    currentBestScore = currentScore;
                    currentOptimalScoreButtonIndex = fieldIndex;
                }

                // Some delay to make it possible to see single steps
                if (visualizeAI && 1 > 0)
                {
                    yield return new WaitForSeconds(1);
                }

                // Undo this step and go to the next field
                GetText(button).text = "";
                fieldsLeft++;

                if (endRecursion)
                {
                    // No need to check further fields if there already is a win
                    break;
                }
            }
            fieldIndex++;
            // Stop if we checked all buttons
        }

        recursionScore = currentBestScore;
        optimalScoreButtonIndex = currentOptimalScoreButtonIndex;
        // Debug.Log("score: " + recursionScore);
    }

    private bool SetMarkAndCheckForWin(TMPro.TMP_Text button, bool colorate = false)
    {
        TMPro.TMP_Text text = GetText(button);
        if (text.text != "")
        {
            return false;
        }
        text.text = gameController.playerMove ? "X" : "O";
        fieldsLeft--;

        return CheckForWin(text.text, colorate);
    }

    private bool CheckForWin(string mark, bool colorate = false)
    {
        if (fieldsLeft > 6)
        {
            return false;
        }
        // Horizontal
        if (CompareButtons(0, 1, 2, mark, colorate)
         || CompareButtons(3, 4, 5, mark, colorate)
         || CompareButtons(6, 7, 8, mark, colorate)
        // Vertical
         || CompareButtons(0, 3, 6, mark, colorate)
         || CompareButtons(1, 4, 7, mark, colorate)
         || CompareButtons(2, 5, 8, mark, colorate)
        // Diagonal
         || CompareButtons(0, 4, 8, mark, colorate)
         || CompareButtons(6, 4, 2, mark, colorate))
        {
            return true;
        }
        return false;
    }
    private bool CompareButtons(int ind1, int ind2, int ind3, string mark, bool colorate = false)
    {
        TMPro.TMP_Text text1 = GetText(gameController.buttonList[ind1]);
        TMPro.TMP_Text text2 = GetText(gameController.buttonList[ind2]);
        TMPro.TMP_Text text3 = GetText(gameController.buttonList[ind3]);
        bool equal = text1.text == mark
                  && text2.text == mark
                  && text3.text == mark;
        if (colorate && equal)
        {
            Color color = gameController.playerMove ? Color.green : Color.red;
            text1.color = color;
            text2.color = color;
            text3.color = color;
        }
        return equal;
    }

}
