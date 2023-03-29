using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Aaron Garza
//aaronaagarza.kaz@gmail.com
public class OverlayCheck : MonoBehaviour
{
    public GameObject killHighlight; //Tese each corespond to the highlightfor each move (piece selected, valid moves, and if a kill is possible)
    public GameObject moveHighlight;
    public GameObject selectHighlight;
    public GameObject lastHighlight;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RemoveObject("Highlight"); //if the player clicks, they have made their move and we remove the highlight
        }
    }

    public void RemoveObject(string text)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(text); //find all objects of Highlight type, delete all that exist
        foreach (GameObject GO in objects)
        {
            Destroy(GO);
        }
    }
}
