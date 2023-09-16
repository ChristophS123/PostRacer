using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class GameManager : MonoBehaviour
{

    public static int MAX_FAILURE_PAKETS = 5;

    [SerializeField] private Text paketText;
    [SerializeField] private Text failurePaketText;
    [SerializeField] private GameObject housePaket;
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text paketsText;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private Transform gameOverTransform;
    [SerializeField] private Text coinText;

    [SerializeField] private List<GameObject> hats;
    [SerializeField] private Transform hatParent;

    [SerializeField] private List<Material> carColors;

    [SerializeField] private GameObject carColorObject;

    [SerializeField] private GameObject breakMenu;

    [SerializeField] private AudioSource coinSource;
    [SerializeField] private AudioSource paketSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource carDrivingSource;
    [SerializeField] private GameObject coinsBoard;


    public Vector3 LineOnePosition;
    public Vector3 LineTwoPosition;
    public Vector3 LineThreePosition;

    public float currentSpeed;

    private int pakets;
    private int falurePakets;
    private float _fuel;
    private float _score;
    public bool isBreak;
    private GameObject breakMenuInstance;
    public float Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            scoreText.text = value.ToString() + "m";
        }
    }
    private float _paketsScore;
    public float PaketsScore
    {
        get
        {
            return _paketsScore;
        }
        set
        {
            paketSource.Play();
            _paketsScore = value;
            paketsText.text = value.ToString();
        }
    }
    private float Fuel
    {
        get
        {
            return _fuel;
        }
        set
        {
            _fuel = value;
            fuelSlider.value = value;
            if(_fuel <= 0)
            {
                GameOver();
            }
        }
    }

    private PostRacerAPI postRacerAPI;

    private float _coins;
    public float Coins
    {
        get
        {
            return _coins;
        }
        set
        {
            coinSource.Play();
            _coins = value;
            coinText.text = value.ToString();
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
            if(value == -1)
            {
                return;
            }
            Instantiate(hats[value], hatParent);
        }
    }

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
            if(selectedColor == -1)
                renderer.material = carColors[0];
            renderer.material = carColors[selectedColor];
        }
    }

    private float currentInterval;
    private Tutorial tutorial;

    private void Start()
    {
        tutorial = FindObjectOfType<Tutorial>();
        isBreak = true;
        SetUpSounds();
        postRacerAPI = new PostRacerAPI();
        Coins = 0;
        CurrentHat = PlayerPrefs.GetInt("selectedhat", -1);
        pakets = 0;
        falurePakets = 0;
        Fuel = 1;
        Score = 0;
        currentInterval = 2.5F;
        if(PlayerPrefs.GetString("gametype") == "offline")
        {
            Destroy(coinsBoard);
        }
        CurrentCarColor = PlayerPrefs.GetInt("selectedcolor", 0);
        paketText.text = pakets.ToString();
        failurePaketText.text = falurePakets + " / " + MAX_FAILURE_PAKETS;
        currentSpeed = 3F;
        StartCoroutine(SpeedRoutine());
        StartCoroutine(AddScore());
        StartCoroutine(IntervalRoutine());
    }

    public void ToggleBreak()
    {
        isBreak = !isBreak;
        if(!isBreak)
        {
            musicSource.Pause();
            carDrivingSource.Pause();
            breakMenuInstance = Instantiate(breakMenu);
            Time.timeScale = 0;
        } else
        {
            carDrivingSource.UnPause();
            musicSource.UnPause();
            Destroy(breakMenuInstance);
            Time.timeScale = 1;
        }
    }

    private void SetUpSounds()
    {
        musicSource.volume = PlayerPrefs.GetInt("music", 50) / 100F;
        coinSource.volume = PlayerPrefs.GetInt("effects", 50) / 100F;
        paketSource.volume = PlayerPrefs.GetInt("effects", 50) / 100F;
        carDrivingSource.volume = PlayerPrefs.GetInt("effects", 50) / 100F;
    }

    private IEnumerator AddScore()
    {
        while(true)
        {
            Score++;
            yield return new WaitForSeconds(currentInterval);
        }
    }

    public void PickUpPaket()
    {
        if (tutorial.nextStep == Tutorial.TutorialStep.PICKUP_PAKET)
            tutorial.CurrentStep = Tutorial.TutorialStep.PICKUP_PAKET;
        pakets++;
        paketSource.Play();
        paketText.text = pakets.ToString();
    }

    public void PlacePaket(GameObject house)
    {
        if (tutorial.isInTutorial == true && tutorial.nextStep != Tutorial.TutorialStep.PLACE_PAKET)
            return;
        if (tutorial.nextStep == Tutorial.TutorialStep.PLACE_PAKET)
            tutorial.CurrentStep = Tutorial.TutorialStep.PLACE_PAKET;
        if (pakets == 0)
            return;
        pakets--;
        PaketsScore++;
        PaketSpawner paketSpawner = house.GetComponentInParent<PaketSpawner>();
        Vector3 position = paketSpawner.getLocation();
        BoxCollider boxCollider = house.GetComponent<BoxCollider>();
        Destroy(boxCollider);
        Destroy(house);
        Instantiate(housePaket, position, Quaternion.identity);
        paketText.text = pakets.ToString();
        FlyingTextManager flyingTextManager = FindObjectOfType<FlyingTextManager>();
        flyingTextManager.SpawnText(new Vector3(position.x, position.y + 1, position.z), "+1");
    }

    public void GameOver()
    {
        PlayerPrefs.SetInt("adrounds", PlayerPrefs.GetInt("adrounds", 0) + 1);
        PlayerPrefs.SetInt("possiblead", 1);
        if (PlayerPrefs.GetString("gametype") == "offline")
        {
            SaveOfflineData();
            SceneManager.LoadScene("OfflineMenu");
        } else
        {
            SaveOnlineData();
        }
    }

    private async void SaveOnlineData()
    {
        Instantiate(gameOver, gameOverTransform);
        Time.timeScale = 0;
        string username = PlayerPrefs.GetString("username");
        float lastHighscore = await postRacerAPI.GetHighscore(username);
        float lastPaketHighscore = await postRacerAPI.GetHighscorePakets(username);
        float lastCoins = await postRacerAPI.GetCoins(username);
        if (Score > lastHighscore)
        {
            await postRacerAPI.SetHighscore(username, Score);
        }
        if (PaketsScore > lastPaketHighscore)
        {
            await postRacerAPI.setHighscorePakets(username, Score);
        }
        await postRacerAPI.SetCoins(username, lastCoins + Coins);
        await postRacerAPI.SetLastScore(username, Score);
        SceneManager.LoadScene("OnlineMenu");
    }

    private void SaveOfflineData()
    {
        PlayerPrefs.SetFloat(StatsField.LASTSCORE, Score);
        float lastHighScore = PlayerPrefs.GetFloat(StatsField.HIGHSCORE, 0);
        if (Score > lastHighScore)
        {
            PlayerPrefs.SetFloat(StatsField.HIGHSCORE, Score);
        }
        float lastPaketScore = PlayerPrefs.GetFloat(StatsField.PAKETHIGHSCORE, 0);
        if (PaketsScore > lastPaketScore)
        {
            PlayerPrefs.SetFloat(StatsField.PAKETHIGHSCORE, PaketsScore);
        }
        PlayerPrefs.Save();
    }

    public void RemoveFuel()
    {
        Fuel -= 0.02F;
    }

    public void AddFuel()
    {
        if (tutorial.nextStep == Tutorial.TutorialStep.PICKUP_FUEL)
            tutorial.CurrentStep = Tutorial.TutorialStep.PICKUP_FUEL;
        float newFuel = Fuel + 0.3F;
        if(newFuel >= 1)
        {
            newFuel = 1;
        }
        paketSource.Play();
        Fuel = newFuel;
    }

    public void FailurePaket()
    {
        falurePakets++;
        failurePaketText.text = falurePakets + " / " + MAX_FAILURE_PAKETS;
        if(falurePakets >= MAX_FAILURE_PAKETS)
        {
            GameOver();
        }
    }

    private IEnumerator SpeedRoutine()
    {
        while(true)
        {
            PlusSpeed();
            yield return new WaitForSeconds(0.5F);
        }
    }

    private IEnumerator IntervalRoutine()
    {
        while(true)
        {
            MinusInterval();
            yield return new WaitForSeconds(3.5F);
        }
    }

    private void MinusInterval()
    {
        if(currentInterval > 0.5F)
        {
            currentInterval -= 0.1F;
        }
    }

    public void PlusSpeed()
    {
        currentSpeed += 0.1F;
    }

}
