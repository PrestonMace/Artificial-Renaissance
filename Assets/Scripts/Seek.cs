using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Seek : MonoBehaviour
{

    //initializes variables
    private Rigidbody AIRB;

    [SerializeField]
    private GameObject TargetObject;

    [SerializeField]
    private Transform TargetTF;
    private Transform AITF;

    private float _desiredVelocity;
    private float _currentVelocity;
    private float _arriveForce;
    private float _arriveSpeed;
    private float _decelerationConstant = 0.4f;
    private float _upperSpeedLimit = 20f;

    private Vector3 _direction;

    //for whisker code
    private Vector3 whiskerOrigin;
    private float whiskerMaxDistance = 2.5f;
    private float wReturnForceStregnth = 10f;


    void Start()
    {
        AIRB = GetComponent<Rigidbody>();
        AITF = GetComponent<Transform>();
    }

    private void SeekStuff()
    {
        //get the direction to the target and the current velocity
        _direction = TargetTF.position - transform.position;
        _currentVelocity = AIRB.linearVelocity.magnitude;

        _arriveSpeed = _direction.magnitude * _decelerationConstant;

        //caps the exponential arrival speed if the AI is too far
        if (_arriveSpeed > 7f)
        {
            _arriveSpeed = 7f;
        }
        
        //add the extra arrive speed to our force
        if (_direction.magnitude < _upperSpeedLimit)
        {
            _desiredVelocity = _direction.magnitude + _arriveSpeed;
        } else //take into account long distances
        {
            _desiredVelocity = _upperSpeedLimit;
        }

            /*
             * the final force to arrive at takes into account how fast
             * we want to be going and how fast we are going already
             */
            _arriveForce = _desiredVelocity - _currentVelocity;

        //adds the force to arrive at the target
        AIRB.AddForce(_direction.normalized * _arriveForce);
    }

    //uses Raycasts for whiskers
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

            if (Physics.Raycast(whiskerOrigin, dir, out hitInfo, whiskerMaxDistance) && hitInfo.collider.gameObject != TargetObject && hitInfo.collider.gameObject.tag != "Wall")
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

    //run the physics-based functions at a fixed rate
    private void FixedUpdate()
    {
        SeekStuff();
        AvoidObstacles();
    }
}
