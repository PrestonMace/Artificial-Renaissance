using UnityEngine;

public class SecondWanderScript : MonoBehaviour
{
    /*[SerializeField] private float _moveSpeed = 0.3f;
    [SerializeField] private float _minWanderDistance = 3f;
    [SerializeField] private float _maxWanderDistance = 8f;
    [SerializeField] private float _viewAngle = 45f;
    [SerializeField] private float _arrivalDistance = 1.5f;
    private bool _needingNewPoint = true;
    private Vector3 _targetPoint;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Wander();
    }

    void Wander()
    {
        if (_needingNewPoint)
        {
            //find where to go
            _targetPoint = GetRandomPoint();
            _needingNewPoint = false;
        }

        //find direction to destination
        Vector3 direction = _targetPoint - transform.position;
        direction.y = 0;

        //are we there yet??
        if (direction.magnitude <= _arrivalDistance)
        {
            _needingNewPoint = true;
        }

        //go there or keep moving anymways
        direction.Normalize();
        rb.AddForce(direction * _moveSpeed, ForceMode.Impulse);
    }

    //get da point
    Vector3 GetRandomPoint()
    {
        float randomAngle = Random.Range(-_viewAngle / 2, _viewAngle / 2);

        Vector3 randomDirection = Quaternion.Euler(0, randomAngle, 0) * transform.forward;

        float randomDistance = Random.Range(_minWanderDistance, _minWanderDistance);

        Vector3 randomPoint = transform.position + randomDirection * randomDistance;

        return randomPoint;
    }*/
}