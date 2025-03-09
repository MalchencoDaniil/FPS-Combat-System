using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AI_Movement : MonoBehaviour
{
    private float _timer;

    private Animator _enemyAnimator;
    private Enemy _enemy;

    private NavMeshAgent _navMeshAgent;

    [Header("Movement Settings")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _chaseDistance = 10f;
    [SerializeField] private float _walkSpeed = 2;
    [SerializeField] private float _chaseSpeed = 4;
    [SerializeField] private float _angularSpeed = 15;

    [Header("Range")]
    [SerializeField] private float _viewRadius = 10f;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private Vector3 _attackRangeOffset;

    [Header("Wander")]
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float wanderTimer = 5f;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyAnimator = GetComponent<Animator>();

        _enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (_enemy._canLive)
        {
            float _distanceToTarget = Vector3.Distance(transform.position, _target.position);

            if (_distanceToTarget > _chaseDistance)
                Wander();
            else
                Chase();

            ApplyStats();
        }
    }

    private void ApplyStats()
    {
        _navMeshAgent.angularSpeed = _angularSpeed * 10;
        _navMeshAgent.stoppingDistance = 1;
    }

    private void Chase()
    {
        _navMeshAgent.speed = _chaseSpeed;
        _navMeshAgent.SetDestination(_target.position);

        float _distanceToTarget = Vector3.Distance(transform.position, _target.position);

        if (_distanceToTarget <= _attackRange)
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.velocity = Vector3.zero;
            _enemyAnimator.SetBool("CanRun", false);
        }

        _enemyAnimator.SetBool("CanRun", true);
    }

    private void Wander()
    {
        _navMeshAgent.speed = _walkSpeed;
        _timer += Time.deltaTime;

        if (_timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            _navMeshAgent.SetDestination(newPos);
            _timer = 0;
        }

        _enemyAnimator.SetBool("CanRun", false);
        _enemyAnimator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
    }

    private static Vector3 RandomNavSphere(Vector3 _origin, float _distation, int _layerMask)
    {
        Vector3 _randDirection = Random.insideUnitSphere * _distation;

        _randDirection += _origin;

        NavMesh.SamplePosition(_randDirection, out NavMeshHit _navHit, _distation, _layerMask);

        return _navHit.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _attackRangeOffset, _attackRange);
    }
}