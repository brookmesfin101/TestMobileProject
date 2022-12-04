using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerPiece : MonoBehaviour
{
    [SerializeField] Animator _animator;

    private bool _isSelected = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        _animator.SetBool("isSelected", _isSelected);
    }

    public void ToggleSelected()
    {
        _isSelected = !_isSelected;
    }
}
