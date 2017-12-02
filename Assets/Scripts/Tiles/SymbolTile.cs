using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SymbolTile : GameTile
{
    public enum SymbolEnum
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }
    private char[] _operatorSymbols = { '+', '-', '×', '/' };

    public SymbolEnum symbol;

    public override string Value
    {
        get
        {
            return _operatorSymbols[(int)symbol].ToString();
        }
    }

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
