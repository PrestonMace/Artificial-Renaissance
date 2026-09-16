using UnityEngine;

public class FleeScript : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 0.5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Flee();
    }

    void Flee()
    {
        
        //Define direction to go
        Vector3 forceDirection = transform.forward * _moveSpeed;

        //Execute going
        rb.AddForce(forceDirection, ForceMode.Impulse);
    }

}
