using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TrappeComponent : MonoBehaviour
{
    public TextMeshPro answerText; 

    //Update the trappe answer text, located in the level
    public void UpdateTrappeText (string _answer)
    {
        answerText.text = _answer; 
    }
}
