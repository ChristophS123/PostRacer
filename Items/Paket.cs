using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paket : MonoBehaviour
{

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Car")
        {
            Destroy(gameObject);
            gameManager.PickUpPaket();
        }
    }

}
