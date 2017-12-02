using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NumberTile))]
[RequireComponent(typeof(LinkedPath))]
public class GoalTotal : MonoBehaviour
{
    public int total;
    public Color goalColor;

    public int CurrentTotal { get { return _linkedPath.PathTotal; } }
    public GameTile ActiveTile { get { return _linkedPath.ActiveTile; } }
    public LinkedPath Path { get { return _linkedPath; } }

    private NumberTile _tile;
    private LinkedPath _linkedPath;

    private Image _spriteImage;

    void Awake()
    {
        _tile = GetComponent<NumberTile>();
        _linkedPath = GetComponent<LinkedPath>();

        _spriteImage = GetComponent<Image>();
    }

    // Use this for initialization
    void Start()
    {
        _linkedPath.LinkTile(_tile);
	}

    public void CheckComplete()
    {
        if (_linkedPath.PathTotal == total)
        {
            _spriteImage.color = Color.yellow;
        }
        else
        {
            _spriteImage.color = goalColor;
        }
    }
}
