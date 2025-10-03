using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour
{
    [Header("Win Conditions")]
    [SerializeField] private int minBallsToWin = 10;
    [SerializeField] private float checkInterval = 1f;

    [Header("Visual Feedback")]
    [SerializeField] private ParticleSystem winEffect;
    [SerializeField] private AudioClip winSound;

    private List<GameObject> ballsInBasin = new List<GameObject>();
    private bool levelCompleted = false;

    public System.Action<int, int> OnBallEnteredBasin;
    public System.Action OnLevelCompleted;

    private void Start()
    {
        // Start checking for win condition periodically
        InvokeRepeating(nameof(CheckWinCondition), checkInterval, checkInterval);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (levelCompleted) return;

        if (IsBall(other.gameObject) && !ballsInBasin.Contains(other.gameObject))
        {
            ballsInBasin.Add(other.gameObject);
            OnBallEnteredBasin?.Invoke(ballsInBasin.Count, minBallsToWin);

            Debug.Log($"Ball entered basin: {ballsInBasin.Count}/{minBallsToWin}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsBall(other.gameObject) && ballsInBasin.Contains(other.gameObject))
        {
            ballsInBasin.Remove(other.gameObject);
            OnBallEnteredBasin?.Invoke(ballsInBasin.Count, minBallsToWin);

            Debug.Log($"Ball left basin: {ballsInBasin.Count}/{minBallsToWin}");
        }
    }

    private bool IsBall(GameObject obj)
    {
        return obj.GetComponent<MarbleBall>() != null || obj.GetComponent<Rigidbody>() != null;
    }

    private void CheckWinCondition()
    {
        if (levelCompleted) return;

        // Clean up destroyed balls
        ballsInBasin.RemoveAll(ball => ball == null);

        if (ballsInBasin.Count >= minBallsToWin)
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        levelCompleted = true;

        Debug.Log($"LEVEL COMPLETED! Balls in basin: {ballsInBasin.Count}/{minBallsToWin}");

        // Visual effects
        if (winEffect != null)
            Instantiate(winEffect, transform.position, Quaternion.identity);

        // Sound effect
        if (winSound != null)
            AudioSource.PlayClipAtPoint(winSound, transform.position);

        OnLevelCompleted?.Invoke();
    }

    public int GetCurrentBallCount() => ballsInBasin.Count;
    public int GetRequiredBallCount() => minBallsToWin;
    public bool IsLevelCompleted() => levelCompleted;

    private void OnDrawGizmos()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            Gizmos.color = levelCompleted ? Color.green : new Color(0, 1, 1, 0.3f);
            Gizmos.matrix = transform.localToWorldMatrix;

            if (collider is BoxCollider boxCollider)
            {
                Gizmos.DrawCube(boxCollider.center, boxCollider.size);
            }
            else if (collider is SphereCollider sphereCollider)
            {
                Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);
            }
        }
    }
}
