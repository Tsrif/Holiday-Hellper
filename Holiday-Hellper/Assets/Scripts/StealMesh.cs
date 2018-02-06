using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class StealMesh : MonoBehaviour {
 
    //takes the mesh from the parent and sets it to the child's
    void Start () {
        GetComponent<MeshFilter>().mesh = transform.parent.GetComponent<DrawFOV>().viewMesh;
    }
}
