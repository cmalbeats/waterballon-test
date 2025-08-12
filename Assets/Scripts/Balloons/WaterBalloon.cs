using UnityEngine;

public class WaterBalloon : MonoBehaviour
{
    public float splashRadius = 3f;
    public float soakAmount = 25f;
    public GameObject splashVFX;
    public AudioClip splashSound;
    public float destroyDelay = 0.05f;

    void OnCollisionEnter(Collision collision)
    {
        Splash(transform.position);
    }

    void Splash(Vector3 pos)
    {
        if (splashVFX != null) Instantiate(splashVFX, pos, Quaternion.identity);
        if (splashSound != null) AudioSource.PlayClipAtPoint(splashSound, pos);

        Collider[] hits = Physics.OverlapSphere(pos, splashRadius);
        foreach (var c in hits)
        {
            Soakable s = c.GetComponentInParent<Soakable>();
            if (s != null)
            {
                s.ApplySoak(soakAmount);
            }
        }

        Destroy(gameObject, destroyDelay);
    }
}
