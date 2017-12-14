using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TotalTile : GameTile
{
    public int total;
    public Color fillColor;

    public GameTile ActiveTile { get { return _linkedPath.ActiveTile; } }

    public override string Value { get { return total.ToString(); } }

    private LinkedPath _linkedPath;

    private Image _spriteImage;

    protected override void Awake()
    {
        base.Awake();

        _linkedPath = GetComponent<LinkedPath>();
        _spriteImage = GetComponent<Image>();
    }

	// Use this for initialization
	protected override void Start()
    {
        base.Start();

        _spriteImage.color = fillColor;
	}

    protected override void InitializeEvents()
    {
        PlayerInput input = GameObject.FindObjectOfType<PlayerInput>();

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { input.GameTileSelected(data); });
        trigger.triggers.Add(entry);
    }

    public void CheckComplete()
    {
        if(_linkedPath.PathTotal == total)
        {
            _spriteImage.color = Color.yellow;
        }
        else
        {
            _spriteImage.color = fillColor;
        }
    }

    public void OnClick(PointerEventData data)
    {
        Debug.Log("Total: " + total);
    }
}
