using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsText : MonoBehaviour
{

    [SerializeField] private Text text1;

    public void SetText(string text)
    {
        Debug.Log(text);
        text1.text = text;
    }

}
