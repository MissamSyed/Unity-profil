using UnityEngine;


public class ExplosionDamage : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float explosionDamage = 100f;

    public void Explode(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, explosionRadius);

        foreach (Collider hit in colliders)
        {
           Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null) 
            {
                rb.AddExplosionForce(explosionForce, position, explosionRadius);
            }
        }

        DebugExplosion(position);
    }

    void DebugExplosion(Vector3 position)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.transform.localScale = Vector3.one * explosionRadius * 2; 
        sphere.GetComponent<Collider>().enabled = false;
        Destroy(sphere, 0.5f);
    }
}
