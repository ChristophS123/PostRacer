using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTextManager : MonoBehaviour
{

    [SerializeField] private GameObject flyingTextPrefab;
    [SerializeField] private float flyDistance = 4F;
    [SerializeField] private float speed = 1F;

    public void SpawnText(Vector3 spawnPoint, string text)
    {
        GameObject spawnedText = Instantiate(flyingTextPrefab);
        spawnedText.transform.position = spawnPoint;
        spawnedText.GetComponent<FlyingText>().SetupText(text);
        StartCoroutine(Move(spawnedText));
    }

    public IEnumerator Move(GameObject textObject)
    {
        float targetY = textObject.transform.position.y + flyDistance;
        while(textObject.transform.position.y < targetY)
        {
            textObject.transform.position += Vector3.up * speed * Time.deltaTime;
            yield return null;
        }
        Destroy(textObject);
    }

}
