using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Aaron Garza
//aaronaagarza.kaz@gmail.com
public class BoardManager
{
    //Declare our class var
    private static BoardManager instance = null;
    //singleton function
    public static BoardManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BoardManager();
            }
            return instance;
        }
    }

    private TileData[,] board = new TileData[8, 8];
    //Sets up our TileData objects for each space
    public void SetupBoard()
    {
        for (int y = 0; y < 8; ++y)
        {
            for (int x = 0; x < 8; ++x)
            {
                board[x, y] = new TileData(x, y);
            }
        }
    }

    public TileData GetTileFromBoard(Vector2 tile)
    {
        return board[(int)tile.x, (int)tile.y];
    }
}
