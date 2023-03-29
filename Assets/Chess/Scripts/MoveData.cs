using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Aaron Garza
//aaronaagarza.kaz@gmail.com
public class MoveData
{
    public TileData firstPosition = null;
    public TileData secondPosition = null;
    public ChessPiece pieceMoved = null;
    public ChessPiece pieceKilled = null;
    public int score = int.MinValue;
}
