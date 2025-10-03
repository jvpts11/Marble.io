using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BallSpawnerController: MonoBehaviour
{
    [SerializeField] public Transform spawnPos;
    [SerializeField] private BallSpawnerConfig config;

    private List<GameObject> spawnedBalls = new List<GameObject>();
    private Coroutine spawnCoroutine;

    public event System.Action OnSpawningCompleted;
    void Start()
    {
        if (spawnPos == null)
        {
            spawnPos = transform;
        }

        //StartSpawn();
    }

    public void StartSpawn()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        spawnCoroutine = StartCoroutine(SpawnBallsCoroutine());
    }

    IEnumerator SpawnBallsCoroutine()
    {
        ClearBalls();

        if(config == null || config.ballToSpawn == null)
        {
            Debug.LogError("Error while spawning balls: null config, no ball to spawn");
            yield break;
        }

        for (int i = 0; i < config.ballAmount; ++i) 
        { 
            SpawnBalls(i);
            yield return new WaitForSeconds(0.1f);
        }

        OnSpawningCompleted?.Invoke();
        Debug.Log($"Spawn Finished! {config.ballAmount} balls created.");
    }

    void ClearBalls()
    {
        foreach (GameObject ball in spawnedBalls)
        {
            if(ball != null)
            {
                Destroy(ball);
            }
        }
        spawnedBalls.Clear();
    }

    void SpawnBalls(int index)
    {
        Vector2 randomPos = Random.insideUnitCircle * config.spawnRadius;
        Vector3 finalPos = spawnPos.position + new Vector3(randomPos.x, config.spawnHeight, randomPos.y);

        GameObject ball = Instantiate(config.ballToSpawn, finalPos, Random.rotation);
        ball.name = $"Ball {index:00}";

        ConfigBall(ball);

        spawnedBalls.Add(ball);
    }

    void ConfigBall(GameObject ball)
    {
        ApplyRandomAppearance(ball);

        if (config.applyInitialForce)
        {
            ApplyRandomForce(ball);
        }
    }

    void ApplyRandomAppearance(GameObject ball)
    {
        Renderer renderer = ball.GetComponent<Renderer>();
        if (renderer != null) 
        {
            if (config.ballMaterial != null && config.ballMaterial.Count > 0)
            {
                Material randomMaterial = config.ballMaterial[Random.Range(0, config.ballMaterial.Count)];
                renderer.material = randomMaterial;
            }
        }
    }

    void ApplyRandomForce(GameObject ball)
    {
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if(rb != null)
        {
            Vector3 randomDir = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-0.2f, 0.2f),
                Random.Range(-1f, 1f)
            ).normalized;

            float force = Random.Range(config.randomForceMin, config.randomForceMax);

            rb.AddForce(randomDir * force, ForceMode.Impulse);
        }
    }

    int GetSpawnedBalls()
    {
        return spawnedBalls.Count;
    }

    void OnDrawGizmosSelected()
    {
        if (spawnPos == null) return;

        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawWireSphere(spawnPos.position + Vector3.up * config.spawnHeight, config.spawnRadius);

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireSphere(spawnPos.position + Vector3.up * config.spawnHeight, 0.2f);
    }
}
