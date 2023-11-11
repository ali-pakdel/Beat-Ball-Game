using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Block : MonoBehaviour
{

    public int health = 2;
    public int points = 100;
    public Vector3 rotator;
    public Material hitMaterial;

    private Material _orgMaterial;
    private Renderer _renderer;
    
    
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(rotator * (transform.position.y) * 0.01f);
        _renderer = GetComponent<Renderer>();
        _orgMaterial = _renderer.sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotator * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        health--;
        if (health <= 0)
        {
            GameManager.Instance.Score += points;
            Destroy(gameObject);
        }

        _renderer.sharedMaterial = hitMaterial;
        Invoke("RestoreMaterial", 0.05f);
    }

    private void RestoreMaterial()
    {
        _renderer.sharedMaterial = _orgMaterial;
    }
}
