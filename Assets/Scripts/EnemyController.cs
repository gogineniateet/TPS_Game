using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    State currentState;

    public Transform player;    
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<Transform>();
        }
        currentState = new Idle(this.gameObject, agent, animator, player);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
    }
}
public class State
{
    public enum STATE{ATTACK, CHASE, IDLE, DEATH}
    public enum EVENTS{ENTER, UPDATE, EXIT}

    public STATE stateName;
    public EVENTS eventStage;

    public GameObject enemy;
    public NavMeshAgent agent;
    public Animator animator;
    public Transform playerPosition;
    public State nextState;
    public State(GameObject _enemy, NavMeshAgent _agent, Animator _animator, Transform _playerPosition)
    {
        this.enemy = _enemy;
        this.playerPosition = _playerPosition;
        this.agent = _agent;
        this.animator = _animator;
        eventStage = EVENTS.ENTER;
    }
    public virtual void Enter()
    {
        eventStage = EVENTS.UPDATE;
    }
    public virtual void Update()
    {
        eventStage = EVENTS.UPDATE;
    }
    public virtual void Exit()
    {
        eventStage = EVENTS.EXIT;
    }
    public State Process()
    {
        if (eventStage == EVENTS.ENTER)
        {
            Enter();
        }
        if (eventStage == EVENTS.UPDATE)
        {
            Update();
        }
        if (eventStage == EVENTS.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }
    public bool CanSeePlayer()
    {
        if (Vector3.Distance(enemy.transform.position, playerPosition.position) < 15f)
            return true;
        return false;
    }
}




public class Idle : State
{
    public Idle(GameObject enemy, NavMeshAgent agent, Animator animator, Transform playerPosition) : base(enemy, agent, animator, playerPosition)
    {
        stateName = STATE.IDLE;
    }
    public override void Enter()
    {
        animator.SetTrigger("isIdle");
        base.Enter();
    }
    public override void Update()
    {
        if (CanSeePlayer())
        {
            nextState = new Chase(enemy, agent, animator, playerPosition);
            eventStage = EVENTS.EXIT;
        }
        // base.Update();
    }
    public override void Exit()
    {
        animator.ResetTrigger("isIdle");
        base.Exit();
    }
}




public class Chase : State
{
    public Chase(GameObject enemy, NavMeshAgent agent, Animator animator, Transform playerPosition) : base(enemy, agent, animator, playerPosition)
    {
        stateName = STATE.CHASE;
        agent.stoppingDistance = 5f;
    }
    public override void Enter()
    {
        animator.SetTrigger("isWalking");
        base.Enter();
    }
    public override void Update()
    {
        agent.SetDestination(playerPosition.position);
        if (!CanSeePlayer())
        {
            nextState = new Idle(enemy, agent, animator, playerPosition);
            eventStage = EVENTS.EXIT;
        }
        if (Vector3.Distance(enemy.transform.position, playerPosition.position) <= agent.stoppingDistance)
        {
            nextState = new Attack(enemy, agent, animator, playerPosition);
            eventStage = EVENTS.EXIT;
        }
        // base.Update();
    }
    public override void Exit()
    {
        animator.ResetTrigger("isWalking");
        base.Exit();
    }
}




public class Attack : State
{
    public Attack(GameObject enemy, NavMeshAgent agent, Animator animator, Transform playerPosition) : base(enemy, agent, animator, playerPosition)
    {
        stateName = STATE.ATTACK;
    }
    public override void Enter()
    {
        animator.SetTrigger("isShooting");
        base.Enter();
    }
    public override void Update()
    {
        if (Vector3.Distance(enemy.transform.position, playerPosition.position) > agent.stoppingDistance + 1f)
        {
            nextState = new Idle(enemy, agent, animator, playerPosition);
            eventStage = EVENTS.EXIT;
        }
        // base.Update();
    }
    public override void Exit()
    {
        animator.ResetTrigger("isShooting");
        base.Exit();
    }
}




public class Death : State
{
    public Death(GameObject enemy, NavMeshAgent agent, Animator animator, Transform playerPosition) : base(enemy, agent, animator, playerPosition)
    {
        stateName = STATE.DEATH;
        agent.enabled = false;
    }
    public override void Enter()
    {
        animator.SetTrigger("isSleeping");
        base.Enter();
    }

    public override void Exit()
    {
        animator.ResetTrigger("isSleeping");
        base.Exit();
    }
}
