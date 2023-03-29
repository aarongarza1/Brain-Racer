using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Aaron Garza
//aaronaagarza.kaz@gmail.com
public class GameManager : MonoBehaviour
{
    private MiniMax minMax = new MiniMax();
    UIManager uiManager;
    BoardManager board;
    OverlayCheck overlay;
    //the functions below are for our game values and how we get/set them
    public bool playerTurn = true;
    bool playerMoved = false;
    AudioSource gameMusic;
    public bool PlayerMoved
    {
        get
        {
            return playerMoved;
        }
        set
        {
            playerMoved = value;
        }
    }
    bool kingDead = false;
    public bool KingDead
    {
        get
        {
            return kingDead;
        }
        set
        {
            kingDead = value;
        }
    }
    bool isWhiteWin = false;
    public bool IsWhiteWin
    {
        get
        {
            return isWhiteWin;
        }
        set
        {
            isWhiteWin = value;
        }
    }
    float timer = 0.0f;
    int turnCount = 0;
    public int TurnCount
    {
        get
        {
            return turnCount;
        }
        set
        {
            turnCount = value;
        }
    }

    MoveData tempMove = null;
    public MoveData TempMove
    {
        get
        {
            return tempMove;
        }
        set
        {
            tempMove = value;
        }
    }

    public MiniMax MinMax
    {
        get
        {
            return minMax;
        }

        set
        {
            minMax = value;
        }
    }
    //Here we begin setting up our Tex Sprites
    [Header("===Queen Sprites===")]
    public Sprite queen_White;
    public Sprite queen_Black;
    [Header("===Pawn Sprites===")]
    public Sprite pawn_White;
    public Sprite pawn_Black;

    public static GameManager Instance;
    //singleton operation
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }
    //initialize our objects
    void Start()
    {
        gameMusic = GetComponent<AudioSource>();
        gameMusic.Play();
        overlay = GameObject.FindGameObjectWithTag("ChessBoard").GetComponent<OverlayCheck>();
        board = BoardManager.Instance;
        uiManager = UIManager.Instance;

        board.SetupBoard();

        //uiManager.CheckMoved(playerMoved);
        uiManager.TurnCount(turnCount);
        uiManager.PlayerTurnText(playerTurn);
    }

    private void Update()
    {
        if (kingDead)
        {
            //Make a restart button on win.
            uiManager.GameRestart(kingDead, isWhiteWin);
        }
        else if (!kingDead)
        {
            if (!playerTurn && timer >= 1.0f)
            {
                timer = 0.0f;
                MoveData move = MinMax.GetMove();
                DoAIMove(move);
                UpdateTurn();
                //playerTurn = !playerTurn;
                //uiManager.PlayerTurnText(playerTurn);
            }
            else if (!playerTurn)
            {
                timer += Time.deltaTime;
            }
        }
    }

    void DoAIMove(MoveData move)
    {
        TileData firstPosition = move.firstPosition;
        TileData secondPosition = move.secondPosition;
        
        SwapPieces(move);

        //if (!kingDead)
        //{
        //    playerTurn = !playerTurn;
        //    turnCount++;

        //    uiManager.TurnCount(turnCount);
        //    uiManager.PlayerTurnText(playerTurn);
        //}
    }

    public void SwapPieces(MoveData move)
    {
        overlay.RemoveObject("Highlight");
        overlay.RemoveObject("LastTag");

        TileData firstTile = move.firstPosition;
        TileData secondTile = move.secondPosition;

        LastMoveTag(move);

        firstTile.CurrentPiece.MovePiece(new Vector2(secondTile.Position.x, secondTile.Position.y));

        ConvertPawn(firstTile, move);

        CheckDeath(secondTile);

        secondTile.CurrentPiece = move.pieceMoved;
        firstTile.CurrentPiece = null;
        secondTile.CurrentPiece.chessPosition = secondTile.Position;
        secondTile.CurrentPiece.HasMoved = true;

        //UpdateTurn();

        if (playerTurn)
        {
            playerMoved = true;
        }
        uiManager.CheckMoved(playerMoved, kingDead);
    }

    public void UndoMove()
    {
        overlay.RemoveObject("LastTag");

        TileData firstTile = tempMove.firstPosition;
        TileData secondTile = tempMove.secondPosition;

        secondTile.CurrentPiece.MovePiece(new Vector2(firstTile.Position.x, firstTile.Position.y));

        //ReturnPawn(firstTile, tempMove);

        //SpriteRenderer sRend = secondTile.CurrentPiece.GetComponent<SpriteRenderer>();
        //sRend.enabled = true;
        //secondTile.CurrentPiece.gameObject.SetActive(true);

        firstTile.CurrentPiece = tempMove.pieceMoved;
        secondTile.CurrentPiece = null;
        firstTile.CurrentPiece.chessPosition = firstTile.Position;
        firstTile.CurrentPiece.HasMoved = false;

        playerMoved = false;

        uiManager.CheckMoved(playerMoved, kingDead);
    }

    private void UpdateTurn()
    {
        if (!kingDead)
        {
            playerTurn = !playerTurn;
            turnCount++;

            uiManager.TurnCount(turnCount);
            uiManager.PlayerTurnText(playerTurn);
        }
    }

    void CheckDeath(TileData _secondTile)
    {
        if (_secondTile.CurrentPiece != null)
        {
            if (_secondTile.CurrentPiece.Type == ChessPiece.PieceType.KING)
            {
                kingDead = true;
                if (_secondTile.CurrentPiece.Team == ChessPiece.PlayerTeam.BLACK)
                {
                    isWhiteWin = true;
                }
                else if (_secondTile.CurrentPiece.Team == ChessPiece.PlayerTeam.WHITE)
                {
                    isWhiteWin = false;
                }
            }
            //SpriteRenderer sRend = _secondTile.CurrentPiece.GetComponent<SpriteRenderer>();
            //sRend.enabled = false;
            //_secondTile.CurrentPiece.gameObject.SetActive(false);
            Destroy(_secondTile.CurrentPiece.gameObject);
            SceneManager.LoadScene("SampleScene");
        }
    }

    //Special rule, pawn becomes queen.
    void ConvertPawn(TileData _firstTile, MoveData _move)
    {
        if (_firstTile.CurrentPiece.Type == ChessPiece.PieceType.PAWN)
        {
            if (_firstTile.CurrentPiece.Team == ChessPiece.PlayerTeam.WHITE)
            {
                if (_move.secondPosition.Position.y == 7)
                {
                    _firstTile.CurrentPiece.SetType((int)ChessPiece.PieceType.QUEEN, queen_White);
                }
            }
            else if (_firstTile.CurrentPiece.Team == ChessPiece.PlayerTeam.BLACK)
            {
                if (_move.secondPosition.Position.y == 0)
                {
                    _firstTile.CurrentPiece.SetType((int)ChessPiece.PieceType.QUEEN, queen_Black);
                }
            }
        }
    }
    //Inverse special rule for undo method. (requires reference of past tile)
    void ReturnPawn(TileData _firstTile, MoveData _move)
    {
        if (_firstTile.CurrentPiece.Type == ChessPiece.PieceType.PAWN)
        {
            if (_firstTile.CurrentPiece.Team == ChessPiece.PlayerTeam.WHITE)
            {
                if (_move.secondPosition.Position.y == 7)
                {
                    _firstTile.CurrentPiece.SetType((int)ChessPiece.PieceType.PAWN, pawn_White);
                }
            }
            else if (_firstTile.CurrentPiece.Team == ChessPiece.PlayerTeam.BLACK)
            {
                if (_move.secondPosition.Position.y == 7)
                {
                    _firstTile.CurrentPiece.SetType((int)ChessPiece.PieceType.PAWN, pawn_Black);
                }
            }
        }
    }

    void LastMoveTag(MoveData move)
    {
        GameObject GOfrom = Instantiate(overlay.lastHighlight);
        GOfrom.transform.position = new Vector2(move.firstPosition.Position.x, move.firstPosition.Position.y);
        GOfrom.transform.parent = transform;

        GameObject GOto = Instantiate(overlay.lastHighlight);
        GOto.transform.position = new Vector2(move.secondPosition.Position.x, move.secondPosition.Position.y);
        GOto.transform.parent = transform;
    }

    public void PlayerEndTurn()
    {
        //playerMoved = false;
        playerTurn = !playerTurn;
        turnCount++;
        playerMoved = false;
        uiManager.CheckMoved(playerMoved, kingDead);
        //uiManager.CheckMoved(playerMoved, kingDead);
        uiManager.TurnCount(turnCount);
        uiManager.PlayerTurnText(playerTurn);
    }
}
