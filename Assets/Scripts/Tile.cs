using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Tile : MonoBehaviour
{
    [SerializeField] Material originalMaterial;
    // Start is called before the first frame update    
    void Start()
    {
        RenameTile();
    }

    
    void RenameTile()
    {
        gameObject.name = $"{transform.position.x}, {transform.position.y}, {transform.position.z}";
    }

    private void OnMouseEnter()
    {
        //gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    private void OnMouseExit()
    {
        //gameObject.GetComponent<MeshRenderer>().material = originalMaterial;
    }

    private void OnMouseDown()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
