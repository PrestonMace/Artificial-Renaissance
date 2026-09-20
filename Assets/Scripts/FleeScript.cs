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

    private void FixedUpdate()
    {
        AIRB.AddForce(_direction.normalized * _maxAcceleration, ForceMode.Impulse);
    }

}
