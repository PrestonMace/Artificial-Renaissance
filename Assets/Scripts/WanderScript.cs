using UnityEngine;

public class WanderScript : MonoBehaviour
{

    // Set the initial speed of the translation movement and time it takes to get to the point.
    // These can also be altered in Unity.

    public int MoveSpeed = 5;
    public int WanderTime = 3;
    private Vector3 PointDirection;
    private float ChangeDirection;

    void Start()
    {

        ChooseDirection();
    }

    void Update()
    {

        // Algorithm to determine movement behavior based on selected point.

        transform.Translate(PointDirection * MoveSpeed * Time.deltaTime, Space.World);

        ChangeDirection -= Time.deltaTime;

        if (ChangeDirection <= 0)
        {

            ChooseDirection();
        }
    }
    void ChooseDirection()
    {

        // Chooses a random point based on its current location.

        int ranX = Random.Range(-5, 5);
        int ranZ = Random.Range(-5, 5);

        PointDirection = new Vector3(ranX, 1, ranZ);
        ChangeDirection = WanderTime;
    }
}