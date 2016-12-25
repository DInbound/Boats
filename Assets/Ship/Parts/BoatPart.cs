using UnityEngine;
using System.Collections;

public class BoatPart : MonoBehaviour
{
    public Vector3 Dimensions = new Vector3(1, 1, 1);
    [Range(0, 100)]
    public float Mass = 0.1f;
                                      
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void OnCollisionEnter(Collision col)
    {
        ;
    }
}
