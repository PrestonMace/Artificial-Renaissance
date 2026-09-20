using UnityEngine;

public class Wander : MonoBehaviour
{

    // This is the target that wander is based off of.
    // It should be invisible, it is just a game object.

    [SerializeField]
    private Transform wanderTarget;

    // Offset changes the general distance in front of the object that is counted as a valid wander location.
    // Larger numbers means further away.

    [SerializeField]
    private float wanderOffset = 4f;

    // Radius increases the area of randomness the target can wander to.
    // Larger numbers means a wider range.

    [SerializeField]
    private float wanderRadius = 3f;

    // The distance the object must be within in order to wander before the artificial delay.
    // Smaller numbers require the object to be closer.

    [SerializeField]
    private float arrivalDistance = 0.5f;

    // Creates a natural degree of rotation when wandering.
    // Larger numbers allow the direction to change more from the previous.

    [SerializeField]
    private float wanderRate = 30f;

    // Controls the turn rate of wandering to make things feel more smooth.
    // Larger numbers produce a wider turn radius.

    [SerializeField]
    private float turnRate = 90f;

    // If the velocity drops below this point, the program assumes the object has stopped moving.
    // Change the number as see fit.

    [SerializeField]
    private float minimumSpeed = 0.1f;

    // How long the object has to be below the velocity threshold before moving again before reaching the target.

    [SerializeField]
    private float stuckTime = 1.5f;

    private Rigidbody rb;

    // Stores the current direction of wandering.

    private float wanderOrientation;
    private bool hasTarget = false;
    private float timeStuck = 0f;
    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        wanderOrientation = 0f;
        ChooseNewWanderTarget();
    }

    private void Update()
    {

        // If there is currently no target, no check will be applied.

        if (!hasTarget || wanderTarget == null)
        {

            return;
        }

        // Creates a vector from the AI to the target location.

        Vector3 direction = wanderTarget.position - transform.position;
        direction.y = 0f;
        float distance = direction.magnitude;

        // Checks if the target has been reached.

        if (distance <= arrivalDistance)
        {

            timeStuck = 0f;
            ChooseNewWanderTarget();
            return;
        }

        // This is used to check the speed of the object for the purposes of stuck detection.

        float currentSpeed = rb.linearVelocity.magnitude;

        if (currentSpeed < minimumSpeed)
        {

            timeStuck += Time.deltaTime;
        }
        else
        {

            timeStuck = 0f;
        }

        // This activates if the object has been stuck for long enough and needs a new target.

        if (timeStuck >= stuckTime)
        {

            timeStuck = 0f;
            ChooseNewWanderTarget();
        }
    }

    private void ChooseNewWanderTarget()
    {

        // This will choose how many degrees the wander point can be from the current direction.

        float randomChange = Random.Range(-1f, 1f) * wanderRate;
        wanderOrientation += randomChange;

        // The center of the possible wander radius is moved by the offset based on the direction the object faces.

        Vector3 targetCenter = transform.position + transform.forward * wanderOffset;

        // Determines which direction from the center of the radius the point will be placed.

        Vector3 targetDirection = Quaternion.AngleAxis(wanderOrientation, Vector3.up) * transform.forward;

        // Determines where the actual targeted point is based on the previous information created above.

        Vector3 targetPosition = targetCenter + targetDirection.normalized * wanderRadius;
        targetPosition.y = transform.position.y;

        if (wanderTarget != null)
        {

            wanderTarget.position = targetPosition;
        }

        timeStuck = 0f;
        hasTarget = true;
    }

    private void FixedUpdate()
    {

        if (!hasTarget || wanderTarget == null)
        {

            return;
        }

        Vector3 direction = wanderTarget.position - transform.position;
        direction.y = 0f;

        // If the target is too close, direction doesn't change to prevent weird behavior.

        if (direction.sqrMagnitude < 0.001f)
        {

            return;
        }

        direction.Normalize();

        // Calculates the angle between the object and target.

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float currentAngle = rb.rotation.eulerAngles.y;

        // Finds the smaller angle to rotate (90 degrees right instead of 270 left).

        float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

        // Limits how fast the object can rotate.

        float maxTurnThisFrame = turnRate * Time.fixedDeltaTime;
        float turnAmount = Mathf.Clamp(angleDifference, -maxTurnThisFrame, maxTurnThisFrame);

        float newAngle = currentAngle + turnAmount;

        // Applies the rotation to the object.

        rb.MoveRotation(Quaternion.Euler(0f, newAngle, 0f));
    }
}