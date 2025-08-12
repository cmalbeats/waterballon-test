using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    public GameObject balloonPrefab;
    public Transform throwOrigin;
    public float throwForce = 12f;
    public float upForce = 3f;
    public int maxBalloons = 5;
    public int currentBalloons = 3;

    void Start()
    {
        currentBalloons = Mathf.Clamp(currentBalloons, 0, maxBalloons);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            TryThrow();
        }
    }

    void TryThrow()
    {
        if (currentBalloons <= 0) return;
        Throw();
        currentBalloons--;
    }

    void Throw()
    {
        if (balloonPrefab == null || throwOrigin == null) return;

        GameObject b = Instantiate(balloonPrefab, throwOrigin.position, Quaternion.identity);
        Rigidbody rb = b.GetComponent<Rigidbody>();
        if (rb == null) rb = b.AddComponent<Rigidbody>();

        Vector3 forward = Camera.main.transform.forward;
        Vector3 velocity = forward * throwForce + Vector3.up * upForce;
        rb.velocity = velocity;
    }
}
