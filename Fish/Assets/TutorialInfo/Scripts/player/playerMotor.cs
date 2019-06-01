using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class playerMotor : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    [SerializeField] Animator animator;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (GetComponent<NavMeshAgent>().velocity != Vector3.zero)
        {
            animator.SetBool("Idle", false);
        }
        else animator.SetBool("Idle", true);
        if (target != null)
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }
 //       Debug.Log(GetComponent<NavMeshAgent>().velocity != Vector3.zero);


    }
    public void MovetoPoint (Vector3 point)
    {
        agent.SetDestination(point);
    }

    public void FollowTarget (Interactable newTarget)
    {
        
        agent.stoppingDistance = newTarget.radius * .8f;
        agent.updateRotation = false;
        target = newTarget.interactionTransform;
    }

    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;
        target = null;
    }
  public  void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0f,(direction.z)));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
