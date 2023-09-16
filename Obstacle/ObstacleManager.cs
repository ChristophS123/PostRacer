using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> obstacles;
    private int lastObstacleID;

    public short lastLine;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        lastObstacleID = 1;
    }

    public void SpawnObstacle()
    {
        int randomNumber = 0;
        do
        {
            randomNumber = new System.Random().Next(0, obstacles.Count - 1);
        } while (randomNumber == lastObstacleID);
        Debug.Log(lastObstacleID + " " + randomNumber);
        GameObject gameObject = obstacles[randomNumber];
        GameObject obstacleObjectHolder = Instantiate(gameObject);
        LevelPart levelPart = obstacleObjectHolder.GetComponent<LevelPart>();
        levelPart.Init();
    }

}
