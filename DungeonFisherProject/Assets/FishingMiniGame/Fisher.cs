using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fisher : MonoBehaviour
{
    public FishingMiniGameScript Script;
    // Start is called before the first frame update
    public void EndZakidivaniia()
    {
        Script.Fishing = true;
        Script.Catch.GetComponent<SpriteRenderer>().sprite = null;
        Script.NumberCatchPosition = 0;
        Script.Catch.GetComponent<SpriteRenderer>().color = new Color(Script.Catch.GetComponent<SpriteRenderer>().color.r, Script.Catch.GetComponent<SpriteRenderer>().color.g, Script.Catch.GetComponent<SpriteRenderer>().color.b, 0);
    }
    public void NextCatchPosition()
    {
        Script.Catch.GetComponent<SpriteRenderer>().color = new Color(Script.Catch.GetComponent<SpriteRenderer>().color.r, Script.Catch.GetComponent<SpriteRenderer>().color.g, Script.Catch.GetComponent<SpriteRenderer>().color.b, 1);
        Script.Catch.transform.localPosition = Script.CatchPositions[Script.NumberCatchPosition];
        Script.Catch.transform.localRotation = Quaternion.Euler(0, 0, Script.CatchRotations[Script.NumberCatchPosition]);
        Script.NumberCatchPosition += 1;
    }
}
