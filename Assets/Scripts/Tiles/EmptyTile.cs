using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : GameTile
{
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
        // No function on click
    }
}
