using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GoalDisplay : MonoBehaviour
{
    private GoalTotal _goalTotal;

    private Text _goalText;

    private string _goalColor;

    void Awake()
    {
        _goalText = GetComponent<Text>();
    }

    void Start()
    {
        SetGoal(_goalTotal);
    }

    public void SetGoal(GoalTotal newGoal)
    {
        //_goalTotal.Path.pathUpdatedEvent.RemoveListener(UpdateText);

        _goalTotal = newGoal;

        _goalTotal.Path.pathUpdatedEvent.AddListener(UpdateText);

        _goalColor = ColorUtility.ToHtmlStringRGBA(_goalTotal.goalColor);

        UpdateText();
    }

    public void UpdateText()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(_goalTotal.CurrentTotal.ToString());
        sb.Append("<color=#");
        sb.Append(_goalColor);
        sb.Append("><size=26>");
        sb.Append(_goalTotal.total);
        sb.Append("</size></color>");

        _goalText.text = sb.ToString();
    }
}
