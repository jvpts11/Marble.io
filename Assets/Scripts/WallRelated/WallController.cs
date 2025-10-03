using UnityEngine;

public abstract class WallController : MonoBehaviour
{
    [Header("Movement configs")]
    [SerializeField] protected float movementSpeed = 5f;
    [SerializeField] protected float raisedHeight = 3f;
    [SerializeField] protected bool startRaised = false;

    [Header("Positions")]
    [SerializeField] protected Vector3 loweredPosition;
    [SerializeField] protected Vector3 raisedPosition;

    protected bool isRaised = false;
    protected bool isMoving = false;
    protected Vector3 targetPosition;

    public System.Action OnWallStartMoving;
    public System.Action OnWallReachedTarget;

    protected virtual void Start()
    {
        loweredPosition = transform.position;
        raisedPosition = transform.position + Vector3.up * raisedHeight;

        isRaised = startRaised;
        targetPosition = startRaised ? raisedPosition : loweredPosition;
        transform.position = targetPosition;
    }

    protected virtual void Update()
    {
        MoveWall();
    }

    protected virtual void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            ToggleWall();
        }
    }

    public virtual void ToggleWall()
    {
        if (isMoving) return;

        isRaised = !isRaised;
        targetPosition = isRaised ? raisedPosition : loweredPosition;
        isMoving = true;

        OnWallStartMoving?.Invoke();
    }

    protected virtual void MoveWall()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            movementSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            isMoving = false;
            OnWallReachedTarget?.Invoke();
        }
    }

    public void RaiseWall()
    {
        if (!isRaised && !isMoving)
        {
            ToggleWall();
        }
    }

    public void LowerWall()
    {
        if (isRaised && !isMoving)
        {
            ToggleWall();
        }
    }

    public bool IsWallRaised() => isRaised;
    public bool IsWallMoving() => isMoving;

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + Vector3.up * raisedHeight / 2,
                           new Vector3(transform.localScale.x, raisedHeight, transform.localScale.z));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * raisedHeight);
    }
}
