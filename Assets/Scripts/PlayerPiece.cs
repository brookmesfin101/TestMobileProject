using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerPiece : Piece
{    
    private bool _isSelected = false;    

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        DelegateHandler.isSelectedDelegate += ToggleSelected;        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)
        {
            return;
        }

        _animator.SetBool("isSelected", _isSelected);
        if (!Moving)
        {
            FindSelectableTiles();
            CheckMouse();
        }
        else
        {
            Move();
        }
    }    

    public void ToggleSelected(Guid id)
    {
        _isSelected = id == _id ? !_isSelected : false;

        if (_isSelected)
        {
            MouseManager.SelectedPlayerPiece = this;
        }
    }

    private void CheckMouse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.tag == Constants.Tile_Tag)
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.Selectable)
                    {
                        // todo: move target
                        MoveToTile(t);
                    }
                }
            }
        }
    }
}
