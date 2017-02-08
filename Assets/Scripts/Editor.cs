using UnityEngine;
using System.Collections;

public class Editor : MonoBehaviour
{
    private Camera _camera;
    
    void Start()
    {
        foreach(Transform child in this.transform)
        {
            if (child.CompareTag("MainCamera"))
                _camera = child.GetComponent<Camera>();
        }
    }

    void Update()
    {
        Vector3 MousePos = Input.mousePosition;
        Vector3 WorldPos = _camera.ScreenToWorldPoint(MousePos);
        Debug.Log(WorldPos);
    }
}
