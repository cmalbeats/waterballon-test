using UnityEngine;

public class BalloonPickup : MonoBehaviour
{
    public int balloonsGiven = 3;
    public float respawnTime = 8f;
    public GameObject pickupVisual;

    private bool available = true;

    void OnTriggerEnter(Collider other)
    {
        if (!available) return;
        PlayerThrow p = other.GetComponentInParent<PlayerThrow>();
        if (p != null)
        {
            p.currentBalloons = Mathf.Min(p.maxBalloons, p.currentBalloons + balloonsGiven);
            StartCoroutine(Respawn());
        }
    }

    System.Collections.IEnumerator Respawn()
    {
        available = false;
        if (pickupVisual != null) pickupVisual.SetActive(false);
        yield return new WaitForSeconds(respawnTime);
        if (pickupVisual != null) pickupVisual.SetActive(true);
        available = true;
    }
}
