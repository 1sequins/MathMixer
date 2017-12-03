using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject winScreen;

    private GoalTotal[] _goalTotals;

    void Awake()
    {
        _goalTotals = GameObject.FindObjectsOfType<GoalTotal>();
    }

	// Use this for initialization
	void Start()
    {
        InitializeEvents();
	}

    private void InitializeEvents()
    {
        foreach(GoalTotal goal in _goalTotals)
        {
            goal.goalCompletedEvent.AddListener(CheckWin);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CheckWin()
    {
        bool levelComplete = true;

        foreach(GoalTotal goal in _goalTotals)
        {
            if(!goal.Complete)
            {
                levelComplete = false;
                break;
            }
        }

        if(levelComplete)
        {
            Debug.Log("Level Complete!");
            DisplayWin();
        }
        else
        {
            Debug.Log("Level incomplete");
        }
    }

    private void DisplayWin()
    {
        winScreen.SetActive(true);
    }
}
