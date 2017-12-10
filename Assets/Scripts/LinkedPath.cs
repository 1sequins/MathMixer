using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GoalTotal))]
[RequireComponent(typeof(NumberTile))]
public class LinkedPath : MonoBehaviour
{
    public UnityEvent pathUpdatedEvent;
    public int PathTotal { get; private set; }
    public string PathEquation { get { return _equationGenerator.GetEquationString(this); } }

    // Current active tile in the path
    public GameTile ActiveTile { get { return _tileStack.Peek(); } }
    public Stack<GameTile> TileStack { get { return _tileStack; } }

    private GoalTotal _goalTotal;
    private Stack<GameTile> _tileStack;
    private EquationGenerator _equationGenerator;

    private StringToFormula _stf;

    void Awake()
    {
        _goalTotal = GetComponent<GoalTotal>();
        _tileStack = new Stack<GameTile>();
        _equationGenerator = GameObject.FindObjectOfType<EquationGenerator>();

        _stf = new StringToFormula();
    }

    public void LinkTile(GameTile nextTile)
    {
        GameTile previousTile = (_tileStack.Count > 0) ? _tileStack.Peek() : null;

        _tileStack.Push(nextTile);
        nextTile.FillTile(_goalTotal, previousTile);

        if(nextTile.GetType() == typeof(NumberTile))
        {
            PathTotal = (int)_stf.Eval(_equationGenerator.GetEquationString(this, true));
            _goalTotal.CheckComplete();
        }

        pathUpdatedEvent.Invoke();
    }

    public void UnlinkTile()
    {
        GameTile removedTile = _tileStack.Pop();

        if(removedTile.GetType() != typeof(NumberTile))
        {
            PathTotal = (int)_stf.Eval(_equationGenerator.GetEquationString(this, true));
        }

        removedTile.EmptyTile(_goalTotal);

        _goalTotal.CheckComplete();

        pathUpdatedEvent.Invoke();
    }
}
