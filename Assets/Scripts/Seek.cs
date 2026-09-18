using UnityEngine;

public class Seek : MonoBehaviour
{

    private Rigidbody AIRB;

    [SerializeField]
    private Transform TargetTF;
    private Transform AITF;

    private float _maxAcceleration = 0.7f;

    private Vector3 _direction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AIRB = GetComponent<Rigidbody>();
        AITF = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _direction = TargetTF.position - transform.position;
    }

    private void FixedUpdate()
    {
        AIRB.AddForce(_direction.normalized * _maxAcceleration, ForceMode.Impulse);
    }
}
