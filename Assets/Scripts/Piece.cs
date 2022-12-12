using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool turn = false;

    [SerializeField] protected Animator _animator;

    [SerializeField] int _move = 5;
    [SerializeField] int _jumpHeight = 2;
    [SerializeField] float _moveSpeed = 2;
    [SerializeField] float jumpVelocity = 4.5f;

    List<Tile> _selectableTiles = new();
    GameObject[] _tiles;
    bool _moving = false;

    float _halfHeight = 0;

    bool _fallingDown = false;
    bool _jumpingUp = false;
    bool _moveToEdge = false;
    Vector3 _jumpTarget;

    Stack<Tile> _path = new();
    Tile _currentTile;

    Vector3 _velocity = new Vector3();
    Vector3 _heading = new Vector3();

    protected Guid _id;

    public Guid Id
    {
        get { return _id; }
    }

    public bool Moving
    {
        get { return _moving; }
        set { _moving = value; }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _id = Guid.NewGuid();
        Init();
    }
   
    protected void Init()
    {
        _tiles = GameObject.FindGameObjectsWithTag(Constants.Tile_Tag);

        _halfHeight = GetComponent<Collider>().bounds.extents.y;

        GameManager.AddUnit(this);
    }

    public void GetCurrentTile()
    {
        _currentTile = GetTargetTile(gameObject);
        _currentTile.Current = true;
    }

    public void EndTurn()
    {
        turn = false;
    }

    public void BeginTurn()
    {
        turn = true;
    }

    public Tile GetTargetTile(GameObject target)
    {
        Tile tile = null;
        if(Physics.Raycast(target.transform.position, Vector3.down, out RaycastHit hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }
        return tile;
    }

    public void ComputeAdjacencyLists()
    {
        foreach (GameObject tile in _tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(_jumpHeight);
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists();
        GetCurrentTile();

        Queue<Tile> process = new();

        process.Enqueue(_currentTile);
        _currentTile.Visited = true;
        //currentTile.Parent = ?? leave as null

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            _selectableTiles.Add(t);
            t.Selectable = true;

            if(t.Distance < _move)
            {
                foreach (Tile tile in t.AdjacencyList)
                {
                    if (!tile.Visited)
                    {
                        tile.Parent = t;
                        tile.Visited = true;
                        tile.Distance = 1 + t.Distance;
                        process.Enqueue(tile);
                    }
                }
            }            
        }
    }

    public void MoveToTile(Tile tile)
    {
        _path.Clear();

        tile.Target = true;
        Moving = true;

        Tile next = tile;
        while(next != null)
        {
            _path.Push(next);
            next = next.Parent;
        }
    }

    public void Move()
    {
        if (_path.Count > 0)
        {
            Tile t = _path.Peek();
            Vector3 target = t.transform.position;

            // Calculate the unit's position
            target.y += _halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                bool jump = transform.position.y != target.y;

                if (jump)
                {
                    Jump(target);
                }
                else
                {
                    CalculateHeading(target);
                    SetHorizontalVelocity();
                }
                
                // Locomotion
                transform.forward = _heading;
                transform.position += _velocity * Time.deltaTime;
            } 
            else
            {
                // Tile center reached
                transform.position = target;
                _path.Pop();
            }
        }
        else
        {
            RemoveSelectableTiles();
            _moving = false;

            GameManager.EndTurn();
        }
    }

    private void SetHorizontalVelocity()
    {
        _velocity = _heading * _moveSpeed;
    }

    private void CalculateHeading(Vector3 target)
    {
        _heading = target - transform.position;
        _heading.Normalize();
    }

    protected void RemoveSelectableTiles()
    {
        if (_currentTile != null)
        {
            _currentTile.Current = false;
            _currentTile = null;
        }

        foreach (Tile tile in _selectableTiles)
        {
            tile.Reset();
        }
        _selectableTiles.Clear();
    }

    void Jump(Vector3 target)
    {
        if (_fallingDown)
        {
            FallDownward(target);
        }
        else if (_jumpingUp)
        {
            JumpUpward(target);
        }
        else if (_moveToEdge)
        {
            MoveToEdge();
        } 
        else
        {
            PrepareJump(target);
        }
    }

    private void MoveToEdge()
    {
        if (Vector3.Distance(transform.position, _jumpTarget) >= 0.05f)
        {
            SetHorizontalVelocity();
        } 
        else
        {
            _moveToEdge = false;
            _fallingDown = true;

            _velocity /= 3.5f;
            _velocity.y = 1.5f;
        }
    }

    private void JumpUpward(Vector3 target)
    {
        _velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y)
        {
            _jumpingUp = false;
            _fallingDown = true;
        }
    }

    private void FallDownward(Vector3 target)
    {
        _velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y <= target.y)
        {
            _fallingDown = false;
            _jumpingUp = false;
            _moveToEdge = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            _velocity = new Vector3();
        }
    }

    private void PrepareJump(Vector3 target)
    {
        float targetY = target.y;
        target.y = transform.position.y;

        CalculateHeading(target);

        if (transform.position.y > targetY)
        {
            _fallingDown = false;
            _jumpingUp = false;
            _moveToEdge = true;

            _jumpTarget = transform.position + (target - transform.position) / 2.0f;
        }
        else
        {
            _fallingDown = false;
            _jumpingUp = true;
            _moveToEdge = false;

            _velocity = _heading * _moveSpeed / 3.0f;

            float difference = targetY - transform.position.y;

            _velocity.y = jumpVelocity * (0.5f + difference / 2.0f);
        }
    }    

    protected void FindPath(Tile target)
    {
        ComputeAdjacencyLists(_jumpHeight, target);
    }
}
