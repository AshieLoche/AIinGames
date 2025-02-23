using System.Linq;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    [SerializeField] private Vector2 _speed;
    private float _moveSpeed;
    private bool _isSwitchDir;

    private void Start()
    {
        _moveSpeed = Random.Range(_speed.x, _speed.y);
    }

    private void Update()
    {
        var platform = ObstacleSpawner.Instance.Platforms.FirstOrDefault(platform => name.Contains(platform.name));

        if (platform == null) return;

        if (transform.position.x >= platform.bounds.max.x)
        {
            _isSwitchDir = true;
        }
        else if (transform.position.x <= platform.bounds.min.x)
        {
            _isSwitchDir = false;
        }

        if (!_isSwitchDir)
        {
            transform.Translate(_moveSpeed * Time.deltaTime * Vector3.right);
        }
        else
        {
            transform.Translate(_moveSpeed * Time.deltaTime * Vector3.left);
        }
    }
}