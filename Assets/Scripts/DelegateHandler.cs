using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateHandler : MonoBehaviour
{
    public delegate void IsSelectedDelegate(Guid id);
    public static IsSelectedDelegate isSelectedDelegate;

    public delegate void OnTileDelegate();
    public static OnTileDelegate onTileDelegate;

    public delegate void TileHasPiece();
    public static TileHasPiece onTileHasPiece;
}
