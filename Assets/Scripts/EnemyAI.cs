using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //Enemy sẽ có 3 range chính: vision, attackRange và returnRange
    //Vision: Khoảng cách mà enemy có thể nhìn thấy player, khi nhìn thấy, enemy sẽ bắt đầu bắn và đuổi theo player
    //AttackRange: Khoảng cách mà enemy có thể bắn player, khi player ra khỏi khoảng cách này, enemy sẽ đuổi theo để bắn tiếp
    //ReturnRange: Khoảng cách mà enemy sẽ quay lại vị trí ban đầu sau khi player ra khỏi khoảng cách này, khoảng cách này có thể dựa trên enemy với player hoặc vị trí ban đầu của enemy

    //Khi nhìn thấy player qua vision, enemy sẽ bắn player
    //Khi player ra khỏi attackRange, enemy sẽ đuổi theo player và ngưng bắn
    //Khi enemy đuổi kịp player, enemy sẽ dừng lại và bắn player
    //Enemy không thể vưa bắn vừa di chuyển
    //Khi enemy quay lại vị trí ban đầu, enemy sẽ ngưng bắn

    //Khi vừa bắt đầu, vision của enemy sẽ được mở
    //Khi player bước vào vision của enemy, vision sẽ được tắt
    //Khi enemy quay lại vị trí ban đầu, vision sẽ được mở lại
    //Khi enemy bị bắn, vision sẽ tắt và enemy sẽ bắn trả player

    //Cách tuần tra khi ở vị trí ban đầu của enemy:
    //Enemy sẽ di chuyển theo 1 hình tròn, đường thẳng hoặc theo 1 đường zic zac nào đó, 1 đường nào đó được lập trình sẵn.
    //Enemy sẽ đứng yên tại chỗ và xoay qua xoay lại.

    private NavMeshAgent agent;
    private Transform player;

    private Vector3 initialPosition;
    [Header("Distance and range")]
    [SerializeField] private float chaseDistance = 100f;
    [SerializeField] private float targetDistance = 50f;
    [SerializeField] private float attackRange = 50f;
    [SerializeField] private float returnRange = 100f;
    [SerializeField] private float wanderRange = 5f;
    [SerializeField] private float stoppingDistanceWithPlayer = 50f;
    [SerializeField] private float visionRaycastRange = 15f;

    //Animator
    private Animator animator;
    private int isChasingHash;
    private int isShootingHash;

    [Header("Check")]
    [SerializeField] private bool isTargetToPlayer;
    [SerializeField] private bool chasingToPlayerWhenSaw;
    public bool isChasing;
    public bool isAttacking;
    private float delayWanderTime = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        isChasingHash = Animator.StringToHash("isChasing");
        isShootingHash = Animator.StringToHash("isShooting");

        initialPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        isChasing = false;
        isAttacking = false;
        isTargetToPlayer = false;
        chasingToPlayerWhenSaw = true;//Mới đầu, enemy đợi nhìn thấy player thì mới mở cửa
    }

    // Update is called once per frame
    void Update()
    {
        var isAlive = GetComponent<EnemyHealth>().alive;
        if (!isAlive)
        {
            return;
        }

        ReturnToInitialPosition();
        TargetPlayer();
        if (isTargetToPlayer) 
        {
            RotateToPlayer();
            ChaseTarget();
            AttackTarget();
        }
    }

    private void ReturnToInitialPosition()
    {
        var distance = Vector3.Distance(initialPosition, transform.position);
        if (distance > returnRange)
        {
            Debug.Log("return");
            AttackTarget();
            isTargetToPlayer = false;
            chasingToPlayerWhenSaw = true;//Khi quay trở về vị trí, enemy tiếp tục đợi khi nhìn thấy player mới đuổi theo
            agent.stoppingDistance = 0f;
            agent.SetDestination(initialPosition);
        }
        else if (distance <= wanderRange && !isTargetToPlayer)//Nếu enemy đến gần vị trí ban đầu, enemy sẽ đi lòng vòng
        {
            StartCoroutine(Wander());
        }
    }

    private void TargetPlayer()
    {
        if (Vector3.Distance(player.position, transform.position) <= targetDistance)
        {
            isTargetToPlayer = true;
            StopCoroutine(Wander());
        }
    }

    private void RotateToPlayer()
    {
        Vector3 direction = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 2f * Time.deltaTime);

        //transform.LookAt(player);
    }

    private void ChaseTarget()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        bool sawPlayer = true;

        if (chasingToPlayerWhenSaw)
        {
            sawPlayer = SawPlayer();
        }

        if (distance > chaseDistance && sawPlayer)
        {
            isChasing = true;
            animator.SetBool(isChasingHash, true);//Set anim chuyển động của enemy

            agent.stoppingDistance = stoppingDistanceWithPlayer;//Khi đuổi theo player, enemy sẽ dừng lại khi cách 1 khoang để bắn
            agent.speed = 10f;//hi đuổi theo player, enemy sẽ co tốc độ = 10f
            agent.SetDestination(player.position);
        }
        else
        {
            isChasing = false;
        }
    }

    private void AttackTarget()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        //Xac dinh tam nhin cua player, neu player trong tam nhin cua enemy, enemy se bắn player
        bool sawPlayer = SawPlayer();

        if (distance <= attackRange && sawPlayer)
        {
            agent.isStopped = true;
            isAttacking = true;
            animator.SetBool(isShootingHash, true);//Set anim ban cua enemy
        }
        else
        {
            agent.isStopped = false;
            isAttacking = false;
            animator.SetBool(isShootingHash, false);//Set anim ban cua enemy
        }
    }

    IEnumerator Wander()
    {
        animator.SetBool(isChasingHash, false);//Set anim chuyển động của enemy
        agent.speed = 3.5f;//hi enemy đi lòng vòng, enemy sẽ co tốc độ = 3.5f
        agent.stoppingDistance = 0f;//Khi enemy đi lòng vòng, khoang cach toi diem dung = 0

        if (delayWanderTime > 0)
        {
            delayWanderTime -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Wandering");

            var randomPoint = Random.insideUnitSphere * wanderRange;
            randomPoint += initialPosition;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPoint, out hit, wanderRange, NavMesh.AllAreas);
            agent.SetDestination(hit.position);

            delayWanderTime = 2f;
        }
        yield return null;
    }

    bool SawPlayer()
    {
        bool sawPlayer = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.GetChild(0).position, transform.GetChild(0).forward, out hit, visionRaycastRange))//transform.GetChild(0) là vị trí sung của enemy
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                sawPlayer = true;
                chasingToPlayerWhenSaw = false;//Khi đã nhìn thấy player, enemy sẽ đuổi theo dù player trốn trong góc khuất
            }
        }
        return sawPlayer;
    }
}
