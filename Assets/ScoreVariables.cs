using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreVariables
{
    static ScoreVariables variables;
    public static int score;

    void Start() {
        variables = new ScoreVariables();
    }
}
