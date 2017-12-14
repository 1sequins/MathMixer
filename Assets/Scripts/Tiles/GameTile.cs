using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameTile : MonoBehaviour
{
    public bool Filled { get { return _goalLinks.Filled; } }
    public GoalTotal LinkedGoal { get { return _goalLinks.LinkedGoal; } }
    public GoalTotal ActiveLink { get { return _goalLinks.ActiveLink; } }

    public virtual string Value { get { return ""; } }

    protected List<GameTile> _adjacentTiles;

    protected GoalLinkages _goalLinks;

    protected Text _tileText;

    private List<GameTile> _linkedTiles;

    protected virtual void Awake()
    {
        _tileText = GetComponentInChildren<Text>();
        _goalLinks = GetComponent<GoalLinkages>();

        _linkedTiles = new List<GameTile>();
    }

	// Use this for initialization
	protected virtual void Start()
    {
        _tileText.text = Value;

        InitializeEvents();
	}

    protected virtual void InitializeEvents()
    {
        PlayerInput input = GameObject.FindObjectOfType<PlayerInput>();

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { input.GameTileSelected(data); });
        trigger.triggers.Add(entry);
    }

    public virtual void FillTile(GoalTotal goal, GameTile prevTile)
    {
        _goalLinks.FillGoal(goal, prevTile);
    }

    public virtual void EmptyTile()
    {
        _goalLinks.EmptyGoal();
    }

    public virtual void EmptyTile(GoalTotal goal)
    {
        _goalLinks.EmptyGoal(goal);
    }

    public bool ContainsGoalLink(GoalTotal goal)
    {
        return _goalLinks.HasLinkedGoal(goal);
    }

    public virtual void OnClick()
    {
        
    }

    public void AddTileLink(GameTile tile)
    {
        if(!_linkedTiles.Contains(tile))
        {
            _linkedTiles.Add(tile);
        }

        if(!tile.ContainsTileLink(this))
        {
            // Link it back
            tile.AddTileLink(this);
        }
    }

    public void RemoveTileLink(GameTile tile)
    {
        _linkedTiles.Remove(tile);
    }

    public bool ContainsTileLink(GameTile tile)
    {
        return _linkedTiles.Contains(tile);
    }
}
