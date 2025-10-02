using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BallSpawnerConfig", menuName = "Marble Game/Ball Spawner Config")]
public class Ball : ScriptableObject
{
    [Header("Spawn Config")]
    public int ballAmount = 20;
    public GameObject ballToSpawn;
    public float spawnRadius = 3f;
    public float spawnHeight = 1f;

    [Header("Textures and Materials")]
    public List<Material> ballMaterial;
    public List<Texture2D> ballTextures;

    [Header("Ball Physics")]
    public float randomForceMin = 0.5f;
    public float randomForceMax = 2f;
    public bool applyInitialForce = false;
}
