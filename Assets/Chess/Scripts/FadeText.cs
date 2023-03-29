using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Aaron Garza
//aaronaagarza.kax@gmail.com
//simulates the flashing text indicating player turn
public class FadeText : MonoBehaviour
{
    Text text;

    float alphaVal = 0.0f;

    bool isAlphaUp = false;

    void Start()
    {
        text = GetComponent<Text>(); //retrieve text component
    }
    
    void Update()
    {
        if(text.text == "White Turn") //if white turn, change color to white
            text.color = new Color(255.0f, 255.0f, 255.0f, alphaVal);
        if (text.text == "Black Turn") //if black turn change color to black
            text.color = new Color(0.0f, 0.0f, 0.0f, alphaVal);
        //here we use time to adjust whether tha transparency is increased or decreased

        if (isAlphaUp)
        {
            alphaVal += Time.deltaTime; //increase alpha of text
        }
        else if (!isAlphaUp)
        {
            alphaVal -= Time.deltaTime; //decrease text alpha
        }

        CheckThreshold();
    }

    void CheckThreshold()
    {
        if (alphaVal <= 0.4f)
        {
            isAlphaUp = true;
        }
        else if (alphaVal >= 1.0f)
        {
            isAlphaUp = false;
        }
    }
}






