using Unity.AI.Navigation;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public static ObstacleSpawner Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else   
            Destroy(gameObject);
    }

    [SerializeField] NavMeshSurface _nmSurface;
    [field: SerializeField] public MeshCollider[] Platforms { get; private set; }
    [SerializeField] GameObject _obstaclePrefab;

    [SerializeField] private int _spawnCount;
    [SerializeField] private Vector2 _spawnSize;
    [SerializeField] private LayerMask _floorMask;

    private void Start()
    {
        foreach (var platform in Platforms)
        {
            for (int i = 0; i < _spawnCount;)
            {
                var minBounds = platform.bounds.min;
                var maxBounds = platform.bounds.max;

                float xPos = Random.Range(minBounds.x, maxBounds.x);
                float zPos = Random.Range(minBounds.z, maxBounds.z);

                Vector3 newPos = new Vector3(xPos, transform.position.y, zPos);

                float xSize = Random.Range(_spawnSize.x, _spawnSize.y);
                float zSize = Random.Range(_spawnSize.x, _spawnSize.y);
                
                Vector3 newSize = new Vector3(xSize, 1, zSize);

                Ray ray = new Ray(newPos, Vector3.down);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, _floorMask))
                {
                    Collider[] colliders = Physics.OverlapSphere(hit.point, SphereRadius(xSize, zSize));

                    if (colliders.Length == 1)
                    {
                        Vector3 newObsPos = new Vector3 (hit.point.x, 0.5f, hit.point.z);
                        GameObject obstacleClone = Instantiate(_obstaclePrefab, newObsPos, Quaternion.identity);
                        obstacleClone.transform.localScale = newSize;
                        obstacleClone.name = $"Obstacle: {platform.name}-{++i}";
                    }
                }
            }
        }

        _nmSurface.BuildNavMesh();
    }

    private float SphereRadius(float xSize, float zSize)
    {
        return Mathf.Max(xSize, zSize);
    }
}