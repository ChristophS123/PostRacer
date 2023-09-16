using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class Shop : MonoBehaviour
{

    [SerializeField] private List<GameObject> hats;
    [SerializeField] private List<GameObject> cars;
    [SerializeField] private Text unlockButtonText;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Transform loadingParent;

    [SerializeField] private GameObject console;
    [SerializeField] private Text coinsText;

    [SerializeField] private Button hatsButton;
    [SerializeField] private Button carsButton;

    private ShopType currentShopType;
    private PostRacerAPI postRacerAPI;

    private GameObject currentConsole;

    private GameObject looked;

    private int _currentHat;
    private int CurrentHat
    {
        get
        {
            return _currentHat;
        }
        set
        {
            if (currentShopType != ShopType.Hat)
                return;
            if (value >= hats.Count)
            {
                carsButton.GetComponent<Image>().color = Color.red;
                hatsButton.GetComponent<Image>().color = Color.white;
                currentShopType = ShopType.Car;
                CurrentCar = 0;
                _currentHat = -1;
                return;
            }
            if (value < 0)
                return;
            _currentHat = value;
            Debug.Log(value);
            GameObject hatObject = Instantiate(hats[value]);
            if(looked != null)
            {
                Destroy(looked);
            }
            looked = hatObject;
            Hat hat = hatObject.GetComponent<Hat>();
            SetButton(hat);
        }
    }

    private int _currentCar;
    private int CurrentCar
    {
        get
        {
            return _currentCar;
        }
        set
        {
            if (currentShopType != ShopType.Car)
                return;
            if (value < 0)
            {
                hatsButton.GetComponent<Image>().color = Color.red;
                carsButton.GetComponent<Image>().color = Color.white;
                currentShopType = ShopType.Hat;
                CurrentHat = hats.Count-1;
                _currentCar = 0;
                return;
            }
            if (value >= cars.Count)
                return;
            _currentCar = value;
            GameObject carObject = Instantiate(cars[value]);
            if (looked != null)
            {
                Destroy(looked);
            }
            looked = carObject;
            Car car = carObject.GetComponent<Car>();
            SetButton(car);
        }
    }

    private void Start()
    {
        postRacerAPI = new PostRacerAPI();
        currentShopType = ShopType.Hat;
        CurrentHat = 0;
        LoadCoins();
    }

    private async void LoadCoins()
    {
        coinsText.text = "Loading...";
        float coins = await postRacerAPI.GetCoins(PlayerPrefs.GetString("username"));
        coinsText.text = coins.ToString();
    }

    private async void SetButton(ShopItem shopItem)
    {
        unlockButtonText.text = "Loading...";
        if(await shopItem.isUnlocked())
        {
            if(shopItem.isSelected())
            {
                if(currentShopType == ShopType.Car)
                {
                    unlockButtonText.text = "Selected";
                } else
                {
                    unlockButtonText.text = "Deselect";
                }
                if (currentConsole != null)
                    Destroy(currentConsole);
            } else
            {
                unlockButtonText.text = "Select";
                if (currentConsole != null)
                    Destroy(currentConsole);
            }
        } else
        {
            unlockButtonText.text = $"Unlock ({shopItem.getPrice()} Coins)";
        }
    }

    public void LoadHats()
    {
        if(currentShopType == ShopType.Car)
        {
            CurrentCar = 0;
            CurrentCar--;
            CurrentHat -= hats.Count - 1;
        }
    }

    public void LoadCars()
    {
        if(currentShopType == ShopType.Hat)
        {
            CurrentHat = hats.Count;
        }
    }

    public void LoadNextHat()
    {
        if(currentShopType == ShopType.Hat)
        {
            CurrentHat++;
        } else
        {
            CurrentCar++;
        }
    }

    public void LoadBackHat()
    {
        if (currentShopType == ShopType.Hat)
        {
            CurrentHat--;
        }
        else
        {
            CurrentCar--;
        }
    }

    public async void OnUnlockButtonClicked()
    {
        if(currentShopType == ShopType.Hat)
        {
            Hat hat = looked.GetComponent<Hat>();
            if (unlockButtonText.text.Contains("Unlock"))
            {
                GameObject loading = Instantiate(loadingScreen, loadingParent);
                string msg = await hat.Buy();
                if (msg != "")
                {
                    if (currentConsole == null)
                        currentConsole = Instantiate(console, loadingParent);
                }
                Destroy(loading);
                LoadCoins();
                CurrentHat = hat.id;
            }
            else if (unlockButtonText.text.Contains("Select"))
            {
                hat.Select();
                unlockButtonText.text = "Deselect";
            }
            else if (unlockButtonText.text.Contains("Deselect"))
            {
                hat.Deselect();
                unlockButtonText.text = "Select";
            }
        } else
        {
            Car car = looked.GetComponent<Car>();
            if (unlockButtonText.text.Contains("Unlock"))
            {
                GameObject loading = Instantiate(loadingScreen, loadingParent);
                string msg = await car.Buy();
                if (msg != "")
                {
                    if (currentConsole == null)
                        currentConsole = Instantiate(console, loadingParent);
                }
                Destroy(loading);
                LoadCoins();
                CurrentCar = car.id;
            }
            else if (unlockButtonText.text.Contains("Select"))
            {
                if(unlockButtonText.text != "Selected")
                {
                    car.Select();
                    if(currentShopType == ShopType.Hat)
                    {
                        unlockButtonText.text = "Deselect";
                    } else
                    {
                        unlockButtonText.text = "Selected";
                    }
                }
            }
            else if (unlockButtonText.text.Contains("Deselect"))
            {
                car.Deselect();
                unlockButtonText.text = "Select";
            }
        }
        
    }

    public void BackToOnlineMenu()
    {
        SceneManager.LoadScene("OnlineMenu");
    }

    private enum ShopType
    {
        Hat, Car
    }

}
