using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{

    [SerializeField] private float speed;

    private GameManager gameManager;
    private ObstacleManager obstacleManager;

    void Update()
    {
        gameManager = FindObjectOfType<GameManager>();
        obstacleManager = FindObjectOfType<ObstacleManager>();
        transform.Translate(0, 0, - (1 * (speed + gameManager.currentSpeed) * Time.deltaTime));
    }

    public void Init()
    {
        gameManager = FindObjectOfType<GameManager>();
        obstacleManager = FindObjectOfType<ObstacleManager>();
    }

}
