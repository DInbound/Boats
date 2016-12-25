using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipBehaviour : MonoBehaviour
{
    public List<GameObject> Parts = new List<GameObject>();

    public Vector3 Volume = new Vector3(0, 0, 0);
    public Rigidbody myBody;
    public float Density = 0;

    public float waterDrag = 5f;
    public float airDrag = 0.05f;

    public float force;
    private OceanGenerator _og;

    // Use this for initialization
    void Start()
    {
        _og = GameObject.FindWithTag("Water").GetComponent<OceanGenerator>();

        myBody = GetComponent<Rigidbody>();

        foreach (Transform child in this.transform)
        {
            if(child.CompareTag("Part"))
                Parts.Add(child.gameObject);
        }

        UpdatePartStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Physics update
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
            myBody.AddRelativeForce(Vector3.forward * 100f);

        if (Input.GetKey(KeyCode.S))
            myBody.AddRelativeForce(Vector3.forward * -100f);

        if (Input.GetKey(KeyCode.D))
            myBody.AddRelativeTorque(Vector3.up * -100f);

        if (Input.GetKey(KeyCode.A))
            myBody.AddRelativeTorque(Vector3.up * 100f);

        FloatyBoaty();
    }

    private void FloatyBoaty()
    {
        foreach(GameObject go in Parts)
        {
            Mesh m = go.GetComponent<MeshFilter>().mesh;

            //float k = force * go.

            // TODO Center of mass object
            foreach(Vector3 v in m.vertices)
            {
                Vector3 pos = v + go.transform.position;

                myBody.AddForceAtPosition(Vector3.up * _og.IsInWater(pos) * force, pos);
                //Debug.DrawLine(pos, pos + new Vector3(0, _og.IsInWater(pos), 0), _og.IsInWater(pos) < 0 ? Color.green : Color.red);
            }
        }
    }

    private void UpdatePartStats()
    {
        float totalDensity = 0;
        float totalMass = 0;
        int amount = 0;

        foreach (GameObject part in Parts)
        {
            BoatPart boatPartScript = part.GetComponent<BoatPart>();
            // NEEDS FIX
            Volume += new Vector3(boatPartScript.Dimensions.x, boatPartScript.Dimensions.y, boatPartScript.Dimensions.z);
            totalDensity += boatPartScript.Mass / (boatPartScript.Dimensions.x * boatPartScript.Dimensions.y * boatPartScript.Dimensions.z);
            totalMass += boatPartScript.Mass;
            amount++;
        }

        this.GetComponent<Rigidbody>().mass = totalMass;
        Density = totalDensity / amount;
    }
}
