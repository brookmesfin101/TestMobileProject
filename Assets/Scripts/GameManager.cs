using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TurnState { PlayerTurn, EnemyTurn}

public class GameManager : MonoBehaviour
{
    private TurnState _turnState;
    [SerializeField] CurrentTurn _currentTurnButton;

    static Dictionary<string, List<Piece>> Units = new();
    static Queue<string> TurnKey = new();
    static Queue<Piece> TurnTeam = new();

    public TurnState TurnState => _turnState;
    // Start is called before the first frame update
    void Start()
    {
        _turnState = TurnState.PlayerTurn;
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnTeam.Count == 0)
        {
            InitTeamTurnQueue();
        }
    }

    private static void InitTeamTurnQueue()
    {
        List<Piece> teamList = Units[TurnKey.Peek()];

        foreach (Piece piece in teamList)
        {
            TurnTeam.Enqueue(piece);
        }
        StartTurn();
    }

    private static void StartTurn()
    {
        if (TurnTeam.Count > 0)
        {
            TurnTeam.Peek().BeginTurn();
        }
    }

    public static void EndTurn()
    {
        //_turnState = TurnState.EnemyTurn;
        //_currentTurnButton.UpdateCurrentTurnWording();

        Piece piece = TurnTeam.Dequeue();
        piece.EndTurn();

        if (TurnTeam.Count > 0)
        {
            StartTurn();
        }
        else
        {
            string team = TurnKey.Dequeue();
            TurnKey.Enqueue(team);
            InitTeamTurnQueue();
        }
    }

    public static void AddUnit(Piece piece)
    {
        List<Piece> list;

        if (!Units.ContainsKey(piece.tag))
        {
            list = new List<Piece>();
            Units[piece.tag] = list;

            if (!TurnKey.Contains(piece.tag))
            {
                TurnKey.Enqueue(piece.tag);
            }
        }
        else
        {
            list = Units[piece.tag];
        }

        list.Add(piece);
    }
}
