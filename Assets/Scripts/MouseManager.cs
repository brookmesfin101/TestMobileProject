using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    
    private Camera _mainCamera;
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
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.Log($"name = {hit.collider.name}");
                if(hit.collider.gameObject.GetComponent<PlayerPiece>() != null)
                {
                    PlayerPiece playerPiece = hit.collider.gameObject.GetComponent<PlayerPiece>();

                    playerPiece.ToggleSelected();
                }
            }
        }        
    }
}
