using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    private const string PLAYER_PIECE = "PlayerPiece";

    public static string PlayerPiece_Layer
    {
        get { return PLAYER_PIECE; }
    }

    private const string WALKABLE = "Walkable";

    public static string Walkable_Layer
    {
        get { return WALKABLE; }
    }

    private const string UNWALKABLE = "UnWalkable";

    public static string UnWalkable_Layer
    {
        get { return UNWALKABLE; }
    }

    private const string PLAYER_TAG = "Player";

    public static string Player_Tag
    {
        get { return PLAYER_TAG; }
    }

    private const string ENEMY_TAG = "Enemy";

    public static string Enemy_Tag
    {
        get { return ENEMY_TAG; }
    }

    private const string TILE_TAG = "Tile";

    public static string Tile_Tag
    {
        get { return TILE_TAG; }
    }
}
