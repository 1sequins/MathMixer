using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameTile))]
public class GoalLinkages : MonoBehaviour
{
    public int maxFill = 1;
    public GameObject dotPrefab;
    public GameObject tileLinkPrefab;
    public Color fillColor;

    public int Fill
    {
        get { return _fill; }
        set
        {
            _fill = value;
            _fill = Mathf.Clamp(_fill, 0, maxFill);
        }
    }
    private int _fill;

    public bool Filled { get { return _fill >= maxFill; } }

    public GoalTotal LinkedGoal { get { return GetLinkedGoal(); } }
    public GoalTotal ActiveLink { get { return ActiveLinkedGoal(); } }

    private Image[] _fillDots;
    private GoalTotal[] _goalLinks;
    private GameObject[] _goalLines;
    private Color _originalDotColor;

    private Image _spriteImage;
    private Color _originalSpriteColor;

    private GameTile _gameTile;

    void Awake()
    {
        _fillDots = new Image[maxFill];
        _goalLinks = new GoalTotal[maxFill];
        _goalLines = new GameObject[maxFill];
        _originalDotColor = dotPrefab.GetComponent<Image>().color;

        _spriteImage = GetComponent<Image>();
        _originalSpriteColor = _spriteImage.color;

        _gameTile = GetComponent<GameTile>();
    }

    // Use this for initialization
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        // No dots for single filling
        if (maxFill == 1)
        {
            return;
        }

        float step = (360f / maxFill);// * Mathf.Deg2Rad;

        float rotation = 0f;
        for (int i = 0; i < maxFill; i++)
        {
            GameObject dot = Instantiate(dotPrefab, transform);
            dot.transform.RotateAround(transform.position, Vector3.forward, rotation);

            _fillDots[i] = dot.GetComponent<Image>();

            rotation += step;
        }
    }

    public bool FillGoal(GoalTotal goal, GameTile prevTile)
    {
        if (Filled) { return false; }

        int fillIndex = _fill;

        for (int i = 0; i < _goalLinks.Length; i++)
        {
            if (_goalLinks[i] == null)
            {
                fillIndex = i;
                break;
            }
        }

        if(maxFill > 1)
        {
            _fillDots[fillIndex].color = goal.goalColor;
        }

        _goalLinks[fillIndex] = goal;
        LinkTiles(prevTile, fillIndex);

        Fill++;

        if(Filled)
        {
            _spriteImage.color = (maxFill == 1) ? goal.goalColor : fillColor;
        }

        return true;
    }

    public GoalTotal EmptyGoal()
    {
        Debug.Log(Fill);
        return EmptyGoal(_goalLinks[_fill - 1]);
    }

    public GoalTotal EmptyGoal(GoalTotal goal)
    {
        int fillIndex = -1;

        for (int i = _goalLinks.Length - 1; i >= 0; i--)
        {
            if (_goalLinks[i] == goal)
            {
                fillIndex = i;
                break;
            }
        }

        if (fillIndex < 0)
        {
            Debug.LogWarning(goal.name + " not connected to tile");
            return null;
        }

        GoalTotal removedGoal = null;

        if(maxFill > 1)
        {
            _fillDots[fillIndex].color = _originalDotColor;
        }

        removedGoal = _goalLinks[fillIndex];
        _goalLinks[fillIndex] = null;
        Destroy(_goalLines[fillIndex]);

        Fill--;
        if (!Filled)
        {
            _spriteImage.color = _originalSpriteColor;
        }

        return removedGoal;
    }

    public GoalTotal ActiveLinkedGoal()
    {
        for(int i = _goalLinks.Length - 1; i >= 0; i--)
        {
            if(_goalLinks[i] != null && _goalLinks[i].ActiveTile == _gameTile)
            {
                return _goalLinks[i];
            }
        }

        return null;
    }

    public bool HasLinkedGoal(GoalTotal goal)
    {
        bool hasGoal = false;

        foreach(GoalTotal gt in _goalLinks)
        {
            if(gt == goal)
            {
                hasGoal = true;
                break;
            }
        }

        return hasGoal;
    }

    private void LinkTiles(GameTile prevTile, int linkIndex)
    {
        if (prevTile != null)
        {
            _goalLines[linkIndex] = Instantiate(tileLinkPrefab, prevTile.transform);
            LineRenderer linkLine = _goalLines[linkIndex].GetComponent<LineRenderer>();
            linkLine.startColor = linkLine.endColor = _goalLinks[linkIndex].goalColor;
            Vector2 toPosition = this.GetComponent<RectTransform>().anchoredPosition - prevTile.GetComponent<RectTransform>().anchoredPosition;
            linkLine.SetPosition(1, toPosition);
        }
    }

    private GoalTotal GetLinkedGoal()
    {
        GoalTotal linkedGoal = null;

        for(int i = 0; i < _goalLinks.Length; i++)
        {
            if(_goalLinks[i] != null)
            {
                linkedGoal = _goalLinks[i];
            }
        }

        return linkedGoal;
    }
}
