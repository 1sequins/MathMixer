using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public float touchHoldBuffer = 0.2f;

    private enum TouchState
    {
        None,
        Tapped,
        Hold,
        Drag,
        Release
    }
    private TouchState _touchState;

    private Vector2 _touchStart;

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
        _touchState = TouchState.None;

        UpdateTouchState();

        switch(_touchState)
        {
            case TouchState.Tapped:
                List<RaycastResult> results = RaycastFromMouse();

                if (results.Count > 0)
                {
                    GameTile tile = GetGameTileFromRaycast(results[0]);

                    if (tile != null)
                    {
                        SelectGoal(tile);
                    }
                }
                break;
            case TouchState.Drag:
                if (_currentGoal != null)
                {
                    DragGoal();
                }
                break;
            case TouchState.Release:
                ClearGoal();
                break;
        }
    }

    private void UpdateTouchState()
    {
        // Mouse input
        if(Input.GetMouseButtonDown(0))
        {
            _touchState = TouchState.Tapped;
            _touchStart = Input.mousePosition;
        }
        else if(Input.GetMouseButton(0))
        {
            if(Vector2.Distance(_touchStart, Input.mousePosition) > touchHoldBuffer)
            {
                _touchState = TouchState.Drag;
            }
            else
            {
                _touchState = TouchState.Hold;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _touchState = TouchState.Release;
        }

        // Touch input
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                _touchState = TouchState.Tapped;
                _touchStart = touch.position;
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                if(Vector2.Distance(_touchStart, Input.mousePosition) > touchHoldBuffer)
                {
                    _touchState = TouchState.Drag;
                }
                else
                {
                    _touchState = TouchState.Hold;
                }
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                _touchState = TouchState.Release;
            }
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
                SetCurrentTotalGoal(tile.ActiveLink);
                _currentTile = tile;
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
