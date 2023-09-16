using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarOptik : MonoBehaviour
{

    [SerializeField] private GameObject carColorObject;
    [SerializeField] private List<Material> carColors;

    [SerializeField] private List<GameObject> hats;
    [SerializeField] private Transform hatParent;

    private int _currentCarColor;
    private int CurrentCarColor
    {
        get
        {
            return _currentCarColor;
        }
        set
        {
            _currentCarColor = value;
            Renderer renderer = carColorObject.GetComponent<Renderer>();
            int selectedColor = value;
            if (selectedColor == -1)
                renderer.material = carColors[0];
            renderer.material = carColors[selectedColor];
        }
    }

    private int _currentHat;
    private int CurrentHat
    {
        get
        {
            return _currentHat;
        }
        set
        {
            _currentHat = value;
            if (value == -1)
            {
                return;
            }
            Instantiate(hats[value], hatParent);
        }
    }

    void Start()
    {
        CurrentCarColor = PlayerPrefs.GetInt("selectedcolor", 0);
        CurrentHat = PlayerPrefs.GetInt("selectedhat", -1);
    }

    
}
