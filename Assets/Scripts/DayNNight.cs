using UnityEngine;
using System.Collections;

public class DayNNight : MonoBehaviour
{
    /// <summary>
    /// Amount of degrees to rotate per second.
    /// </summary>
    [Range(0, 360)]
    public float RotationSpeed = 60f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        this.transform.Rotate(Vector3.right * Time.deltaTime * RotationSpeed);
    }
}
