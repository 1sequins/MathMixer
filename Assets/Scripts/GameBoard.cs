using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public int boardWidth;
    public int boardHeight;

    public GameObject tileLinkPrefab;

    private GameTile[] _tiles;

	// Use this for initialization
	void Start()
    {
        _tiles = GetComponentsInChildren<GameTile>();

        LinkBoard();
	}
	
	private void LinkBoard()
    {
        for(int i = 0; i < boardWidth; i++)
        {
            for(int j = 0; j < boardHeight; j++)
            {
                GameTile tile = _tiles[i + j * boardWidth];

                // Don't link empty tiles
                if(tile.GetType() == typeof(EmptyTile)) { continue; }

                for(int x = i - 1; x <= i + 1; x++)
                {
                    for(int y = j - 1; y <= j + 1; y++)
                    {
                        // Check for valid board space
                        if((x < 0 || x >= boardWidth) || (y < 0 || y >= boardHeight)) { continue; }

                        GameTile tileLink = _tiles[x + y * boardWidth];

                        // Don't link empty tiles
                        if (tileLink.GetType() == typeof(EmptyTile)) { continue; }

                        // Don't link same type tiles (number => number / symbol => symbol)
                        if(tile.GetType() == tileLink.GetType()) { continue; }

                        StartCoroutine(LinkTile(tile, tileLink));
                    }
                }
            }
        }
    }

    private IEnumerator LinkTile(GameTile t1, GameTile t2)
    {
        yield return new WaitForEndOfFrame();
        GameObject link = Instantiate(tileLinkPrefab, t1.transform);
        LineRenderer linkLine = link.GetComponent<LineRenderer>();
        Vector2 toPosition = t2.GetComponent<RectTransform>().anchoredPosition - t1.GetComponent<RectTransform>().anchoredPosition;
        linkLine.SetPosition(1, toPosition);
        t1.AddTileLink(t2);
    }
}
