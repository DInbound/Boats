using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    private GameObject _target;
    private Camera _camera;

    private float _minZoom = 40f;
    private float _maxZoom = 100f;

    public float ZoomSpeed = 20f;
    public float Sensitivity = 1f;

    public float MaxCamHeight = 10f;
    private float _maxAngle = 60f;


    // Use this for initialization
    public void Start()
    {
        _target = this.transform.parent.parent.gameObject;
        _camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    public void Update()
    {
        Pan();
        Zoom();
    }

    private void Pan()
    {
        // Rotate the target for the orbit view.
        _target.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * Sensitivity, 0));

        // Rotate the camera on the x axis
        transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * Sensitivity, 0, 0));
        transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localEulerAngles.x, 90f - _maxAngle, 90f + _maxAngle), 0, 0);

        // Move the camera up and down.
        if (transform.rotation.x <= 0)
            transform.position = new Vector3(transform.position.x, _target.transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(transform.position.x, _target.transform.position.y + 45f * Mathf.Sin(transform.rotation.x), transform.position.z);
    }

    private void Zoom()
    {
        if (_camera.fieldOfView <= _maxZoom && _camera.fieldOfView >= _minZoom)
        {
            _camera.fieldOfView += -Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        }
        if (_camera.fieldOfView > _maxZoom)
            _camera.fieldOfView = _maxZoom;
        if (_camera.fieldOfView < _minZoom)
            _camera.fieldOfView = _minZoom;
    }
}
