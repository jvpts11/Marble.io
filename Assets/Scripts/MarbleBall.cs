using UnityEngine;

public class MarbleBall : MonoBehaviour
{
    [Header("Ball Config")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;


    void Start()
    {
        float scale = Random.Range(minScale, maxScale);
        transform.localScale = Vector3.one * scale;

        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.mass *= scale;
        }
    }
}
