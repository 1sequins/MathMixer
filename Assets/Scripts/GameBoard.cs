﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public int boardWidth;
    public int boardHeight;

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
                GameTile tile = _tiles[i + j * boardHeight];

                for(int x = i - 1; x <= i + 1; x++)
                {
                    for(int y = j - 1; y <= j + 1; y++)
                    {
                        // Check for valid board space
                        if((x < 0 || x >= boardWidth) || (y < 0 || y >= boardHeight)) { continue; }

                        GameTile tileLink = _tiles[x + y * boardHeight];

                        tile.AddTileLink(tileLink);
                    }
                }
            }
        }
    }
}