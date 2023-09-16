using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarController : MonoBehaviour
{

    [SerializeField] private GameManager gameManager;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Camera camera;

    private Tutorial tutorial;

    private Animator animator;

    private short _streetLine;
    private short StreetLine
    {
        get
        {
            return _streetLine;
        }
        set
        {
            if(value > 2 || value < 0)
            {
                return;
            }
            /*switch (value)
            {
                case 0:
                    transform.position = gameManager.LineOnePosition;
                    break;
                case 1:
                    transform.position = gameManager.LineTwoPosition;
                    break;
                case 2:
                    transform.position = gameManager.LineThreePosition;
                    break;
            }*/
            _streetLine = value;
            Debug.Log(_streetLine);
        }
    }

    private Vector2 startPosition;
    private Vector2 endPosition;

    private int sensitivity;

    private void Start()
    {
        tutorial = FindObjectOfType<Tutorial>();
        animator = GetComponent<Animator>();
        _streetLine = 1;
        sensitivity = PlayerPrefs.GetInt("sensitivity", 50);
    }

    private void Update()
    {
        MobileTouch();
        ComputerControll();
        ComputerPaketControll();
    }

    private void ComputerPaketControll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "house_paket")
                {
                    GameObject house = hit.collider.gameObject;
                    gameManager.PlacePaket(house);
                }
            }
        }
    }

    private void ComputerControll()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        } else if(Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
    }


    bool alreadyTouch = false;
    private void MobileTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(alreadyTouch)
            {
                return;
            }
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPosition = touch.position;
                    break;
                case TouchPhase.Moved:
                    endPosition = touch.position;
                    if (endPosition.x < startPosition.x - sensitivity)
                    {
                        MoveLeft();
                        alreadyTouch = true;
                    }  
                    if (endPosition.x > startPosition.x + sensitivity)
                    {
                        MoveRight();
                        alreadyTouch = true;
                    } 
                    break;
                case TouchPhase.Ended:
                    break;
                default:
                    break;
            }
        } else
        {
            alreadyTouch = false;
        }
    }

    private void MoveRight()
    {
        if (tutorial.isInTutorial == true && tutorial.nextStep != Tutorial.TutorialStep.SWIPE_RIGHT)
            return;
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0));
        //animator.SetTrigger("middle_to_right");
        if (StreetLine == 1)
        {
            Debug.Log("middle_to_right");
            animator.SetTrigger("middle_to_right");
        }
        if(StreetLine == 0)
        {
            Debug.Log("left_to_middle");
            animator.SetTrigger("left_to_middle");
        }
        StreetLine++;
        if (tutorial.nextStep == Tutorial.TutorialStep.SWIPE_RIGHT) 
            tutorial.CurrentStep = Tutorial.TutorialStep.SWIPE_RIGHT;
    }

    private void MoveLeft()
    {
        if (tutorial.isInTutorial == true && tutorial.nextStep != Tutorial.TutorialStep.SWIPE_LEFT)
            return;
        if (StreetLine == 1)
        {
            animator.SetTrigger("middle_to_left");
        }
        if(StreetLine == 2)
        {
            animator.SetTrigger("right_to_middle");
        }
        StreetLine--;
        if(tutorial.nextStep == Tutorial.TutorialStep.SWIPE_LEFT)
            tutorial.CurrentStep = Tutorial.TutorialStep.SWIPE_LEFT;
    }
}
