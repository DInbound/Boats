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


    // Use this for initialization
    public void Start()
    {
        _target = this.transform.parent.gameObject;
        _camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    public void Update()
    {
        // TODO : FIXME rotate target and move camera forwards/backwards to zoom
        Pan();
        Zoom();
    }

    private void Pan()
    {
        // Rotate the target for the orbit view.
        _target.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * Sensitivity, 0));

        // Rotate the camera on the x axis
        // todo limit to the actual values (60 and -60 degrees) idk why it ignores this.
        if (transform.rotation.x > 60)
            transform.rotation = new Quaternion(60, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        else if (transform.rotation.x < -60)
            transform.rotation = new Quaternion(-60, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        else
            transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * Sensitivity, 0, 0));

        // Move the camera up and down.
        if (transform.rotation.x <= 0)
            transform.position = new Vector3(transform.position.x, _target.transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(transform.position.x, _target.transform.position.y + 90 * Mathf.Sin(transform.rotation.x), transform.position.z);
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
