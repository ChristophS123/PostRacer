using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    private ObstacleManager obstacleManager;
    private GameManager gameManager;

    private void Start()
    {
        obstacleManager = FindObjectOfType<ObstacleManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.collider.gameObject);
        if (gameManager != null)
        {
            gameManager.RemoveFuel();
        }
        if(collision.collider.tag == "LevelPart")
        {
            obstacleManager.SpawnObstacle();
        } else if(collision.collider.tag == "house_paket")
        {
            gameManager.FailurePaket();
        }
    }

}
