using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceShader : MonoBehaviour {
    Camera _camera;
    public Shader _shader;
	// Use this for initialization
	void Start () {
        _camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        _camera.SetReplacementShader(_shader, null);
	}
}
