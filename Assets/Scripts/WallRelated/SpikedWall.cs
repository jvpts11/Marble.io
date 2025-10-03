using UnityEngine;

public class SpikedWall : WallController
{
    [Header("Destruction Config")]
    [SerializeField] private bool destroyBallsOnRaised = true;

    [Header("Effects")]
    [SerializeField] private ParticleSystem destructionEffect;
    [SerializeField] private AudioClip destructionSound;


    protected override void Start()
    {
        base.Start();

        // Configuração automática do comportamento oposto
        isRaised = !startRaised;
        targetPosition = isRaised ? raisedPosition : loweredPosition;
        transform.position = targetPosition;

    }

    private bool IsBall(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null && !rb.isKinematic)
            return true;

        if (obj.GetComponent<MarbleBall>() != null)
            return true;

        if (obj.name.ToLower().Contains("ball") || obj.name.ToLower().Contains("bola"))
            return true;

        return false;
    }

    private void DestroyBall(GameObject ball)
    {
        Debug.Log($"Destroyed Ball: {ball.name}", ball);

        if (destructionEffect != null)
        {
            ParticleSystem effect = Instantiate(destructionEffect, ball.transform.position, Quaternion.identity);
            Destroy(effect.gameObject, 2f);
        }

        if (destructionSound != null)
        {
            AudioSource.PlayClipAtPoint(destructionSound, ball.transform.position, 0.5f);
        }

        Destroy(ball);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!destroyBallsOnRaised || !isRaised) return;

        if (IsBall(collision.gameObject))
        {
            DestroyBall(collision.gameObject);
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (isRaised && destroyBallsOnRaised)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, GetComponent<Collider>().bounds.size);
            Gizmos.DrawIcon(transform.position + Vector3.up * 2f, "warning");
        }
    }
}
