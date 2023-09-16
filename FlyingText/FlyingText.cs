using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingText : MonoBehaviour
{

    public TextMesh text;

    public void SetupText(string text)
    {
        this.text.text = text;
    }

    private void Update()
    {
        transform.LookAt(transform.position - Camera.main.transform.position, Camera.main.transform.up);
    }

}
