using UnityEngine;
using UnityEngine.AI;

public class AIRootMotionController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _maxTarget;

    private void OnValidate()
    {
        if (!_navMeshAgent) _navMeshAgent = GetComponent<NavMeshAgent>();
        if (!_animator) _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_navMeshAgent.hasPath)
        {
            var dir = (_navMeshAgent.steeringTarget - transform.position).normalized;
            var aniDir = transform.InverseTransformDirection(dir);
            var isFacingMoveDirection = Vector3.Dot(dir, transform.forward) > .5f;
            
            _animator.SetFloat("Horizontal", isFacingMoveDirection ? aniDir.x : 0, 0.5f, Time.deltaTime);
            _animator.SetFloat("Vertical", isFacingMoveDirection ? aniDir.z : 0, 0.5f, Time.deltaTime);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), 100 * Time.deltaTime);

            if (Vector3.Distance(transform.position, _navMeshAgent.destination) < _navMeshAgent.radius)
                _navMeshAgent.ResetPath();
        }
        else
        {
            _animator.SetFloat("Horizontal", 0, 0.25f, Time.deltaTime);
            _animator.SetFloat("Vertical", 0, 0.25f, Time.deltaTime);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var isHit = Physics.Raycast(ray, out RaycastHit hit, _maxTarget);

            if (isHit)
                _navMeshAgent.destination = hit.point;
        }
    }

    private void OnDrawGIzmos()
    {
        if (_navMeshAgent.hasPath)
        {
            for (int i = 0; i < _navMeshAgent.path.corners.Length -1; i++)
            {
                Debug.DrawLine(_navMeshAgent.path.corners[i], _navMeshAgent.path.corners[i + 1], Color.red);
            }
        }
    }
}