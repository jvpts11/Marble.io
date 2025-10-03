using UnityEngine;

public class NormalWall : WallController
{
    [Header("Normal wall config")]
    [SerializeField] private Material raisedMaterial;
    [SerializeField] private Material loweredMaterial;
    [SerializeField] private Renderer wallRenderer;

    protected override void Start()
    {
        base.Start();

        UpdateVisual();
    }

    protected override void HandleInput()
    {

    }

    public void ToggleThisWall()
    {
        if (!isMoving)
        {
            ToggleWall();
        }
    }

    protected override void MoveWall()
    {
        base.MoveWall();
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (wallRenderer != null)
        {
            if (isRaised && raisedMaterial != null)
                wallRenderer.material = raisedMaterial;
            else if (!isRaised && loweredMaterial != null)
                wallRenderer.material = loweredMaterial;
        }
    }
}
