using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDisplayPanel : MonoBehaviour
{
    public GameObject goalDisplayPrefab;

	// Use this for initialization
	void Start ()
    {
        ClearGoalDisplay();

        GoalTotal[] goals = GameObject.FindObjectsOfType<GoalTotal>();

        for(int i = 0; i < goals.Length; i++)
        {
            GameObject goal = Instantiate(goalDisplayPrefab, transform);

            goal.GetComponent<GoalDisplay>().SetGoal(goals[i]);
        }
	}

    private void ClearGoalDisplay()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
