using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour
{

    [SerializeField] private Text tutorialText;

    public bool isInTutorial;

    public TutorialStep nextStep;

    private TutorialStep _currentStep;
    public TutorialStep CurrentStep
    {
        get
        {
            return _currentStep;
        }
        set
        {
            Debug.Log(_currentStep);
            if (_currentStep == TutorialStep.FINISHED)
            {
                return;
            }
            switch(value)
            {
                case TutorialStep.SWIPE_LEFT:
                    if(nextStep == TutorialStep.SWIPE_LEFT)
                    {
                        StartCoroutine(OnLeftSwiped());
                    }
                    break;
                case TutorialStep.SWIPE_RIGHT:
                    if (nextStep == TutorialStep.SWIPE_RIGHT)
                    {
                        StartCoroutine(OnRightSwiped());
                    }
                    break;
                case TutorialStep.PICKUP_PAKET:
                    if (nextStep == TutorialStep.PICKUP_PAKET)
                    {
                        StartCoroutine(OnPickUpPaket());
                    }
                    break;
                case TutorialStep.PLACE_PAKET:
                    if (nextStep == TutorialStep.PLACE_PAKET)
                    {
                        StartCoroutine(OnPaketPlaced());
                    }
                    break;
                case TutorialStep.PICKUP_FUEL:
                    if (nextStep == TutorialStep.PICKUP_FUEL)
                    {
                        OnFuelPickUp();
                    }
                    break;
            }
            _currentStep = value;
        }
    }

    private void Start()
    {
        nextStep = TutorialStep.FINISHED;
        if(PlayerPrefs.GetInt("tutorial", 0) == 0)
        {
            isInTutorial = true;
            _currentStep = TutorialStep.SWIPE_LEFT;
            StartCoroutine(StartTutorial());
        } else
        {
            isInTutorial = false;
            _currentStep = TutorialStep.FINISHED;
        }
    }

    private IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(1.8F);
        nextStep = TutorialStep.SWIPE_LEFT;
        Time.timeScale = 0.5F;
        tutorialText.text = "Swipe Left";
    }

    public IEnumerator OnLeftSwiped()
    {
        nextStep = TutorialStep.FINISHED;
        tutorialText.text = "";
        Time.timeScale = 1F;
        yield return new WaitForSeconds(4.25F);
        nextStep = TutorialStep.SWIPE_RIGHT;
        Time.timeScale = 0.5F;
        tutorialText.text = "Swipe Right";
    }

    public IEnumerator OnRightSwiped()
    {
        nextStep = TutorialStep.FINISHED;
        tutorialText.text = "";
        Time.timeScale = 1F;
        yield return new WaitForSeconds(1.8F);
        nextStep = TutorialStep.PICKUP_PAKET;
        Time.timeScale = 0.5F;
        tutorialText.text = "Pick Paket";
    }

    public IEnumerator OnPickUpPaket()
    {
        nextStep = TutorialStep.FINISHED;
        tutorialText.text = "";
        Time.timeScale = 1F;
        yield return new WaitForSeconds(1.8F);
        nextStep = TutorialStep.PLACE_PAKET;
        Time.timeScale = 0.5F;
        tutorialText.text = "Place Paket";
    }

    public IEnumerator OnPaketPlaced()
    {
        nextStep = TutorialStep.FINISHED;
        tutorialText.text = "";
        Time.timeScale = 1F;
        yield return new WaitForSeconds(6);
        nextStep = TutorialStep.PICKUP_FUEL;
        Time.timeScale = 0.5F;
        tutorialText.text = "Pick Fuel";
    }

    public void OnFuelPickUp()
    {
        tutorialText.text = "";
        Time.timeScale = 1F;
        FinishTutorial();
    }

    public void FinishTutorial()
    {
        PlayerPrefs.SetInt("tutorial", 1);
        isInTutorial = false;
    }

    public enum TutorialStep
    {
        SWIPE_LEFT, SWIPE_RIGHT, PICKUP_PAKET, PLACE_PAKET, PICKUP_FUEL, FINISHED
    }

}
