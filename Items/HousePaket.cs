using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousePaket : MonoBehaviour
{

    [SerializeField] private float speed;

    private GameManager gameManager;

    void Update()
    {
        gameManager = FindObjectOfType<GameManager>();
        transform.Translate(0, 0, -(1 * (speed + gameManager.currentSpeed) * Time.deltaTime));
    }

}
