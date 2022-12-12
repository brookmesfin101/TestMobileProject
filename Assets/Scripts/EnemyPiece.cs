using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPiece : Piece
{
    GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)
        {
            return;
        }
       
        if (!Moving)
        {
            FindNearestTarget();
            CalculatePath();
            FindSelectableTiles();
        }
        else
        {
            Move();
        }
    }

    private void CalculatePath()
    {
        Tile targetTile = GetTargetTile(target);
        FindPath(targetTile);
    }

    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(Constants.Player_Tag);

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position);

            if (d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }

        target = nearest;
    }
}
