using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public float rotateSpeed;
    public float scaleFactor;
    public GameObject parent;
    public GameObject mesh;
    public Material material;

    private Transform t;
    // Start is called before the first frame update
    void Start()
    {
         t = this.transform;
         if (mesh != null)
         {
             GameObject ring = Instantiate(mesh, parent.transform.position, Quaternion.identity, parent.transform);
             GameObject ringMesh = ring.transform.GetChild(0).gameObject;
             ringMesh.GetComponent<MeshRenderer>().material = material;
             Rigidbody rb = ringMesh.AddComponent<Rigidbody>();
             rb.useGravity = false;
             rb.isKinematic = true;

         }
    }

    // Update is called once per frame
    void Update()
    {

        t.Rotate(new Vector3(0, rotateSpeed, 0));
        t.localScale = (new Vector3(scaleFactor, scaleFactor*1.5f, scaleFactor));
    }
    
    public void IncrementSize()
    {
        StartCoroutine(this.Expand(scaleFactor + 0.4f));
    }

    private IEnumerator Expand(float size)
    {
        while (scaleFactor < size)
        {
            scaleFactor += 0.04f;
            yield return null;
        }
    }
}
