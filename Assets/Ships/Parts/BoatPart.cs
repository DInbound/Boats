using UnityEngine;
using System.Collections;

public class BoatPart : MonoBehaviour
{
    /// <summary>
    /// Local coordinates of center of mass.
    /// </summary>
    public Vector3 CenterOfMass = new Vector3(0, 0, 0);
    [Range(0, 100)]
    public float Mass = 0.1f;

    public float GetDensity()
    {
        return Mass / (transform.lossyScale.x * transform.lossyScale.y * transform.lossyScale.z);
    }

    public float GetVolume()
    {
        return transform.localScale.x * transform.localScale.y * transform.localScale.z;
    }
}