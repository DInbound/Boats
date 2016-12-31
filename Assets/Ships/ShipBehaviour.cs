using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipBehaviour : MonoBehaviour
{
    public bool Controllable = false;

    public List<GameObject> Parts = new List<GameObject>();

    public Rigidbody myBody;

    public float ShipVolume = 0;
    public float ShipMass = 0;
    public float ShipDensity = 0;

    private WaterMeshMaker _wmm;

    // Use this for initialization
    void Start()
    {
        _wmm = GameObject.FindWithTag("Water").GetComponent<WaterMeshMaker>();

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
        if (Controllable)
        {
            if (Input.GetKey(KeyCode.W))
                myBody.AddRelativeForce(Vector3.forward * 100f);

            if (Input.GetKey(KeyCode.S))
                myBody.AddRelativeForce(Vector3.forward * -100f);

            if (Input.GetKey(KeyCode.D))
                myBody.AddRelativeTorque(Vector3.up * -100f);

            if (Input.GetKey(KeyCode.A))
                myBody.AddRelativeTorque(Vector3.up * 100f);
        }

        FloatyBoaty();
    }

    private void FloatyBoaty()
    {
        // Add some force to every object.
        foreach(GameObject part in Parts)
        {
            // Get the objects center of mass.
            Vector3 com = part.transform.position + part.GetComponent<BoatPart>().CenterOfMass;
            // Calculate how far the center of mass is from the water surface.
            float distance = _wmm.CalculateY(com) - com.y;
            // TODO Take in account the amount of buoyancy the ship has.
            // Calculate the amount of force that should be applied.
            float force = 9.81f * distance;

            // Add the force
            myBody.AddForceAtPosition(Vector3.up * force, com);
            Debug.DrawLine(com, com + new Vector3(0, distance, 0), distance < 0 ? Color.green : Color.red);
        }
    }

    private void UpdatePartStats()
    {
        float totalDensity = 0;
        float totalVolume = 0;
        float totalMass = 0;
        int amount = 0;

        foreach (GameObject part in Parts)
        {
            BoatPart boatPartScript = part.GetComponent<BoatPart>();
            // NEEDS FIX
            totalVolume += boatPartScript.GetVolume();
            totalDensity += boatPartScript.GetDensity();
            totalMass += boatPartScript.Mass;
            amount++;
        }

        this.GetComponent<Rigidbody>().mass = ShipMass;

        ShipDensity = totalDensity / amount;
        ShipVolume = totalVolume;
        ShipMass = totalMass;
    }
}
