using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // IMPORTANT: !Movement in an isometric environment is differnet from a perspective environment (camera)!
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    private Vector3 _input;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    // Getting teh input from the player
    void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    void Move()
    {
        _rb.MovePosition(transform.position + transform.forward * _speed * Time.deltaTime);
    }
}
