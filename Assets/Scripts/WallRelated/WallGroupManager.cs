using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallGroupManager : MonoBehaviour
{
    [Header("Wall Groups")]
    [SerializeField] private List<NormalWall> group1Walls; 
    [SerializeField] private List<SpikedWall> group2Walls;

    [Header("Configs")]
    [SerializeField] private InputActionReference toggleAction;
    private bool controlsEnabled = true;

    private void OnEnable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.Enable();
            toggleAction.action.performed += OnToggleWalls;
        }
    }

    private void OnDisable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.performed -= OnToggleWalls;
            toggleAction.action.Disable();
        }
    }

    private void OnToggleWalls(InputAction.CallbackContext context)
    {
        ToggleAllWalls();
    }

    public void ToggleAllWalls()
    {
        // Alternar todas as paredes normais
        foreach (var wall in group1Walls)
        {
            if (wall != null && !wall.IsWallMoving())
            {
                wall.ToggleThisWall();
            }
        }

        // Alternar todas as paredes com espinhos
        foreach (var wall in group2Walls)
        {
            if (wall != null && !wall.IsWallMoving())
            {
                wall.ToggleWall();
            }
        }
    }

    public void SetWallControlsEnabled(bool enabled)
    {
        controlsEnabled = enabled;
    }

    public void ToggleGroup1()
    {
        foreach (var wall in group1Walls)
        {
            if (wall != null) wall.ToggleThisWall();
        }
    }

    public void ToggleGroup2()
    {
        foreach (var wall in group2Walls)
        {
            if (wall != null) wall.ToggleWall();
        }
    }
}
