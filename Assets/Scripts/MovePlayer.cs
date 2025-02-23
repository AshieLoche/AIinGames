using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MovePlayer : MonoBehaviour
{
    NavMeshAgent _agent;
    Camera _cam;
    [SerializeField] float jumpDuration, jumpHeight;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 location = new Vector3(hit.point.x, 0, hit.point.z);
                _agent.SetDestination(location);
            }
        }
        if (_agent.isOnOffMeshLink) // Detect when AI reaches a link
        {
            StartCoroutine(JumpAcrossLink());
        }
    }

    IEnumerator JumpAcrossLink()
    {
        OffMeshLinkData linkData = _agent.currentOffMeshLinkData;
        Vector3 start = _agent.transform.position;
        Vector3 end = linkData.endPos + Vector3.up * _agent.baseOffset; // Adjust to match agent height
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float heightOffset = jumpHeight * Mathf.Sin(t * Mathf.PI); // Create an arc

            _agent.transform.position = Vector3.Lerp(start, end, t) + new Vector3(0, heightOffset, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _agent.transform.position = end; // Ensure AI lands properly
        _agent.CompleteOffMeshLink();
    }
}