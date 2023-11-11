using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private const float Speed = 30f;
    private Rigidbody _rigidbody;
    private Vector3 _velocity;
    private Renderer _renderer;
    
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        Invoke(nameof(Launch), 0.5f);
    }

    void Launch()
    {
        _rigidbody.velocity = Vector3.up * Speed;
    }
    
    void FixedUpdate()
    {
        _rigidbody.velocity = _rigidbody.velocity.normalized * Speed;
        _velocity = _rigidbody.velocity;

        if (!_renderer.isVisible)
        {
            GameManager.Instance.Balls--;
            Destroy(gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        _rigidbody.velocity = Vector3.Reflect(_velocity, collision.GetContact(0).normal);
    }

    private void OnCollisionExit(Collision other)
    {  
        //Debug.Log(other.gameObject.name);
 
        // if (other.gameObject.gameObject.name == "Blocks")
        //     Destroy(other.gameObject);
    }
}
