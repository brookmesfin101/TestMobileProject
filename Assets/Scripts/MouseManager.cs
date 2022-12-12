using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{    
    private Camera _mainCamera;

    public static PlayerPiece SelectedPlayerPiece;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);            

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                HandlePlayerSelection(hit);

                HandlePlayerMovement(hit);

                DelegateHandler.onTileHasPiece?.Invoke();
            }
        }        
    }

    private void HandlePlayerSelection(RaycastHit hit)
    {
        if (hit.collider.gameObject.GetComponent<PlayerPiece>() != null && hit.collider.gameObject.tag == Constants.Player_Tag)
        {
            PlayerPiece playerPiece = hit.collider.gameObject.GetComponent<PlayerPiece>();
                        
            DelegateHandler.isSelectedDelegate?.Invoke(playerPiece.Id);
        }
    }

    private void HandlePlayerMovement(RaycastHit hit)
    {
        if (hit.collider.gameObject.GetComponent<Tile>() != null)
        {
            Tile tile = hit.collider.gameObject.GetComponent<Tile>();

            if (SelectedPlayerPiece != null && tile.gameObject.layer == LayerMask.NameToLayer(Constants.Walkable_Layer) && tile.PieceOnTile == null)
            {
                Vector3 newPos = new Vector3(tile.transform.position.x, SelectedPlayerPiece.transform.position.y, tile.transform.position.z);
                SelectedPlayerPiece.transform.position = newPos;                
            }
        }
    }    
}
