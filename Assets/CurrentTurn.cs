using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentTurn : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] TMP_Text _text;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCurrentTurnWording();
    }

    public void UpdateCurrentTurnWording()
    {
        _text.text = _gameManager.TurnState switch
        {
            TurnState.PlayerTurn => "Player Turn",
            TurnState.EnemyTurn => "Enemy Turn",
            _ => "Error",
        };
    }
}
