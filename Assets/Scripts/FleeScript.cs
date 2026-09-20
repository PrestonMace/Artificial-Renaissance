using UnityEngine;
using Unity.Mathematics;

public class Flee : MonoBehaviour
{

    private Rigidbody AIRB;

    [SerializeField]
    private Transform EvadeTF;
    private Transform AITF;

    private float _maxAcceleration = 0.4f;

    private Vector3 _direction;

    //for whisker code
    private Vector3 whiskerOrigin;
    private float whiskerMaxDistance = 2f;
    private float wReturnForceStregnth = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AIRB = GetComponent<Rigidbody>();
        AITF = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _direction = transform.position - EvadeTF.position;
    }

    private void AvoidObstacles()
    {

        whiskerOrigin = AITF.position;

        //make 5 of them rotated on the Z axis in even increments
        Vector3[] whiskerDirections = new Vector3[]
        {
        _direction.normalized,
        Quaternion.AngleAxis(45, Vector3.up) * _direction.normalized,
        Quaternion.AngleAxis(-45, Vector3.up) * _direction.normalized,
        Quaternion.AngleAxis(90, Vector3.up) * _direction.normalized,
        Quaternion.AngleAxis(-90, Vector3.up) * _direction.normalized,
        };

        //impliments functionality for each whisker
        foreach (Vector3 dir in whiskerDirections)
        {
            RaycastHit hitInfo;

            if (Physics.Raycast(whiskerOrigin, dir, out hitInfo, whiskerMaxDistance) && hitInfo.collider.gameObject.tag != "Wall")
            {
                //Debug.Log("Hit " + hitInfo.collider.name);

                //get the distance between the end of the whisker and the point it collided at
                Vector3 pointOfCollision = hitInfo.point;
                Vector3 whiskerEndPos = whiskerOrigin + dir * whiskerMaxDistance;
                float whiskerPenetrationDistance = (whiskerEndPos - pointOfCollision).magnitude;

                //allows us to tweak the distance-based force
                float whiskerReturnForce = whiskerPenetrationDistance * wReturnForceStregnth;

                //gets the repel direction from the center of the object so there is no equilibrium
                Vector3 repelDirection = (transform.position - hitInfo.collider.gameObject.transform.position);
                float repelForce = repelDirection.magnitude * math.abs(whiskerReturnForce);

                //applies the final force
                AIRB.AddForce(repelDirection.normalized * repelForce);

                //for viewport debugging
                Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.green);
            }
            else
            {
                Debug.DrawRay(whiskerOrigin, dir * whiskerMaxDistance, Color.red);
            }
        }
    }

    private void FixedUpdate()
    {
        AIRB.AddForce(_direction.normalized * _maxAcceleration, ForceMode.Impulse);
        AvoidObstacles();
    }

}
