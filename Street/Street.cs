using UnityEngine;

public class Street : MonoBehaviour
{

    [SerializeField] private float scrollSpeed = 0.5F;

    private GameManager gameManager;

    private float offset;
    private Material material;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        offset += - (Time.deltaTime * ((gameManager.currentSpeed + 2.3F) / 3)) / 10F;
        material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }

}
