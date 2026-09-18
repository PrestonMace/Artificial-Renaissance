using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Seek : MonoBehaviour
{

    private Rigidbody AIRB;

    private GameObject TargetObject;

    [SerializeField]
    private Transform TargetTF;
    private Transform AITF;

    private float _desiredVelocity;
    private float _currentVelocity;
    private float _arriveForce;
    private float _arriveSpeed;
    private float _decelerationConstant = 0.02f;

    private Vector3 _direction;

    //whisker code
    private Vector3 whiskerOrigin;
    private Vector3 whiskerDirection;
    private float whiskerMaxDistance = 3f;


    void Start()
    {
        AIRB = GetComponent<Rigidbody>();
        AITF = GetComponent<Transform>();
    }

    private void SeekStuff()
    {
        _direction = TargetTF.position - transform.position;
        _currentVelocity = AIRB.linearVelocity.magnitude;

        _arriveSpeed = _direction.magnitude * _decelerationConstant;
        _desiredVelocity = _direction.magnitude + _arriveSpeed;
        _arriveForce = _desiredVelocity - _currentVelocity;

        AIRB.AddForce(_direction.normalized * _arriveForce, ForceMode.Impulse);
    }

    private void AvoidObstacles()
    {
        whiskerOrigin = AITF.position;
        whiskerDirection = _direction.normalized;
        RaycastHit hitInfo;

        if (Physics.Raycast(whiskerOrigin, whiskerDirection, out hitInfo, whiskerMaxDistance) && hitInfo.collider.gameObject != TargetObject)
        {
            Debug.Log("Hit: " + hitInfo.collider.name);

            Vector3 pointOfCollision = hitInfo.point;
            Vector3 whiskerEndPos = whiskerOrigin + whiskerDirection * whiskerMaxDistance;
            float whiskerPenetrationDistance = (whiskerEndPos - pointOfCollision).magnitude;

            float whiskerReturnForce = whiskerPenetrationDistance;

            Vector3 repelDirection = (transform.position - hitInfo.collider.gameObject.transform.position);
            float repelForce = repelDirection.magnitude * math.abs(whiskerReturnForce);

            AIRB.AddForce(repelDirection.normalized * repelForce, ForceMode.Impulse);

            //Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.green);
        }
        else
        {
            //Debug.DrawRay(whiskerOrigin, whiskerDirection * whiskerMaxDistance, Color.red);
        }

    }

    private void FixedUpdate()
    {
        SeekStuff();
        AvoidObstacles();
    }
}
