using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    private GraphicRaycaster _graphicRaycaster;
    private GoalTotal _currentGoal;
    private GameTile _currentTile;

    private EquationGenerator _equationGenerator;

    void Awake()
    {
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _equationGenerator = GameObject.FindObjectOfType<EquationGenerator>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            List<RaycastResult> results = RaycastFromMouse();

            if(results.Count > 0)
            {
                GameTile tile = GetGameTileFromRaycast(results[0]);

                if(tile != null)
                {
                    SelectGoal(tile);
                }
            }
        }
        else if(Input.GetMouseButton(0))
        {
            if(_currentGoal != null)
            {
                DragGoal();
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            ClearGoal();
        }
    }

    private List<RaycastResult> RaycastFromMouse()
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(ped, results);

        return results;
    }

    private GameTile GetGameTileFromRaycast(RaycastResult raycastResult)
    {
        GameObject hitObject = raycastResult.gameObject;
        GameTile tile = hitObject.GetComponent<GameTile>();
        if (tile == null)
        {
            tile = hitObject.transform.parent.GetComponent<GameTile>();
        }

        return tile;
    }

    private void SelectGoal(GameTile tile)
    {
        // If the tile is the end of a path
        if (tile.ActiveLink != null)
        {
            // If there is a different goal selected, or none selected, choose that one
            if (_currentGoal == null)
            {
                SetCurrentTotalGoal(tile.ActiveLink);
                _currentTile = tile;
            }
            else if(tile != _currentTile)
            {
                _currentGoal.Path.LinkTile(tile);
                _currentTile = tile;
            }
        }
        else
        {
            // If the tile is filled, remove all tiles that exist in the path after it
            if (tile.Filled && _currentGoal == null)
            {
                tile.LinkedGoal.Path.UnlinkToTile(tile);
            }
            else if (_currentGoal != null)
            {
                // If the tile is linked to the current active tile in the goal
                if (_currentGoal.ActiveTile.ContainsTileLink(tile))
                {
                    _currentGoal.Path.LinkTile(tile);
                    _currentTile = tile;
                }
            }
        }

        if (_currentGoal != null)
        {
            _equationGenerator.UpdateEquation(_currentGoal.Path);
        }
    }

    private void DragGoal()
    {
        List<RaycastResult> results = RaycastFromMouse();

        if(results.Count > 0)
        {
            GameTile tile = GetGameTileFromRaycast(results[0]);

            if (tile != null)
            {
                // If the tile is filled, remove all tiles that exist in the path after it
                if (!tile.Filled)
                {
                    // If the tile is linked to the current active tile in the goal
                    if (_currentGoal.ActiveTile.ContainsTileLink(tile))
                    {
                        _currentGoal.Path.LinkTile(tile);
                        _currentTile = tile;
                    }
                }

                if (_currentGoal != null)
                {
                    _equationGenerator.UpdateEquation(_currentGoal.Path);
                }
            }
        }
    }
    
    private void ClearGoal()
    {
        _currentGoal = null;
        _currentTile = null;
    }

    public void GameTileSelected(BaseEventData sender)
    {
        /*
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

        if(_currentGoal != null)
        {
            _equationGenerator.UpdateEquation(_currentGoal.Path);
        }
        */
    }

    private void SetCurrentTotalGoal(GoalTotal newGoal)
    {
        _currentGoal = newGoal;
    }
}
