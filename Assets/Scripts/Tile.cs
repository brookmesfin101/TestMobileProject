using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Tile : MonoBehaviour
{
    [SerializeField] Material _originalMaterial;
    [SerializeField] bool walkable = false;

    private Piece _pieceOnTile;

    private bool _current = false;
    private bool _target = false;
    private bool _selectable = false;

    private List<Tile> _adjacencyList = new List<Tile>();

    //Needed BFS (breadth first search)
    private bool _visited = false;
    private Tile _parent = null;
    private int _distance = 0;

    public Piece PieceOnTile
    {
        get { return _pieceOnTile; }
        set { _pieceOnTile = value; }
    }

    public bool Current
    {
        get { return _current; }
        set { _current = value; }
    }

    public bool Visited
    {
        get { return _visited; }
        set { _visited = value; }
    }

    public bool Selectable
    {
        get { return _selectable; }
        set { _selectable = value; }
    }

    public bool Target
    {
        get { return _target; }
        set { _target = value; }
    }

    public int Distance
    {
        get { return _distance; }
        set { _distance = value; }
    }

    public Tile Parent
    {
        get { return _parent; }
        set { _parent = value; }
    }

    public List<Tile> AdjacencyList
    {
        get { return _adjacencyList; }
        set { _adjacencyList = value; }
    }

    // Start is called before the first frame update    
    void Start()
    {
        RenameTile();
        DetectPieceOnTile();
        DelegateHandler.onTileHasPiece += DetectPieceOnTile;
    }

    private void DetectPieceOnTile()
    {
        Ray ray = new Ray(transform.position, Vector3.up);
        if (Physics.Raycast(ray, out RaycastHit hit, LayerMask.NameToLayer(Constants.PlayerPiece_Layer)))
        {
            _pieceOnTile = hit.collider.GetComponent<Piece>();            
        } 
        else
        {
            _pieceOnTile = null;
        } 
    }

    void RenameTile()
    {
        gameObject.name = $"{transform.position.x}, {transform.position.y}, {transform.position.z}";
    }

    //private void OnMouseEnter()
    //{        
    //    if(gameObject.layer == LayerMask.NameToLayer(Constants.Walkable_Layer))
    //    {
    //        gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    //    } 
    //    else
    //    {
    //        gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
    //    }
    //}

    //private void OnMouseExit()
    //{
    //    gameObject.GetComponent<MeshRenderer>().material = _originalMaterial;
    //}

    // Update is called once per frame
    void Update()
    {
        if (_current)
        {
            GetComponent<MeshRenderer>().material.color = Color.magenta;
        } 
        else if (_target)
        {
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else if(_selectable)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = _originalMaterial;
        }
    }

    public void Reset()
    {
        _adjacencyList.Clear();

        _current = false;
        _target = false;
        _selectable = false;

        _visited = false;
        _parent = null;
        _distance = 0;
    }

    public void FindNeighbors(float jumpHeight)
    {
        Reset();

        CheckTile(Vector3.forward, jumpHeight);
        CheckTile(Vector3.back, jumpHeight);
        CheckTile(Vector3.left, jumpHeight);
        CheckTile(Vector3.right, jumpHeight);
    }

    public void CheckTile(Vector3 direction, float jumpHeight)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {
                if(!Physics.Raycast(tile.transform.position, Vector3.up, out RaycastHit hit, 1))
                {
                    _adjacencyList.Add(tile);
                }
            }
        }
    }
}
