using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NumberTile : GameTile
{
    public int number;

    public override string Value { get { return number.ToString(); } }

    protected override void Awake()
    {
        base.Awake();
    }

	// Use this for initialization
	protected override void Start()
    {
        base.Start();
	}

    public override void OnClick()
    {
        base.OnClick();
    }
}
