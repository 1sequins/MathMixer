using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public Text totalText;

    private GoalTotal _currentGoal;

    private EquationGenerator _equationGenerator;

    void Awake()
    {
        _equationGenerator = GameObject.FindObjectOfType<EquationGenerator>();
    }

    public void GameTileSelected(BaseEventData sender)
    {
        GameTile tile = sender.selectedObject.GetComponent<GameTile>();

        // If a goal tile is selected, only change the current goal
        GoalTotal goal = tile.GetComponent<GoalTotal>();
        if(goal != null)
        {
            SetCurrentTotalGoal(goal);

            _equationGenerator.UpdateEquation(_currentGoal.Path);

            return;
        }

        // If the tile is the end of a path
        if(tile.ActiveLink != null)
        {
            // If there is a different goal selected, or none selected, choose that one
            if(_currentGoal == null || _currentGoal != tile.ActiveLink)
            {
                SetCurrentTotalGoal(tile.ActiveLink);
            }

            // Remove the tile from the path
            _currentGoal.Path.UnlinkTile();
        }
        else
        {
            if(tile.Filled) { return; } // TODO: SetCurrentTotalGoal?

            if (_currentGoal != null)
            {
                // If the tile is linked to the current active tile in the goal
                if(_currentGoal.ActiveTile.ContainsTileLink(tile))
                {
                    _currentGoal.Path.LinkTile(tile);
                }
            }
        }

        _equationGenerator.UpdateEquation(_currentGoal.Path);
    }

    public void TotalTileSelected(BaseEventData sender)
    {
        GoalTotal tTile = sender.selectedObject.GetComponent<GoalTotal>();
        SetCurrentTotalGoal(tTile);

        _equationGenerator.UpdateEquation(_currentGoal.Path);
    }

    private void SetCurrentTotalGoal(GoalTotal newGoal)
    {
        _currentGoal = newGoal;
    }
}
