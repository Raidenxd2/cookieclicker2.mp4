using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshoptimizer : MonoBehaviour
{
    
    public float ReduceBy;

    // Start is called before the first frame update
    void Start()
    {
        var originalMesh = GetComponent<MeshFilter>().sharedMesh;
        var meshSimplifier = new UnityMeshSimplifier.MeshSimplifier();
        meshSimplifier.Initialize(originalMesh);
        meshSimplifier.SimplifyMesh(ReduceBy);
        var destMesh = meshSimplifier.ToMesh();
        GetComponent<MeshFilter>().sharedMesh = destMesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
