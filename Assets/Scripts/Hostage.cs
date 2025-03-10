using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Hostage : MonoBehaviour
{
    [SerializeField] private Vector3 RescuePosition;
    [SerializeField] private float rescueDistance = 2f;
    [SerializeField] private Transform player;
    
    public bool isRescue;
    private NavMeshAgent agent;
    private CharacterController controller;

    private bool canRescue;

    private Animator animator;
    private int isRescuehash;
    private int isDeadHash;

    [SerializeField] private List<GameItem> dropItems = new List<GameItem>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        RescuePosition = GameObject.FindGameObjectWithTag("RescuePosition").transform.position;

        isRescuehash = Animator.StringToHash("isRescue");
        isDeadHash = Animator.StringToHash("isDead");
        canRescue = false;
        isRescue = false;
    }

    // Update is called once per frame
    void Update()
    {
        var isAlive = GetComponent<HostageHealth>().alive;
        if (!isAlive)
        {
            return;
        }

        if (!animator.GetBool(isRescuehash))
        {
            RotateToPlayer();
        }

        if (Input.GetKeyDown(KeyCode.F))//Neu con song moi duoc giai cuu
        {
            if (canRescue)
            {
                BeingRescue();
            }
        }

        if (Vector3.Distance(player.position, transform.position) <= rescueDistance)
        {
            canRescue = true;
        }
        else
        {
            canRescue = false;
        }
    }

    private void RotateToPlayer()
    {
        Vector3 direction = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = rotation;

        //transform.LookAt(player);
    }

    private void BeingRescue()
    {
        Debug.Log("Hostage is save");
        animator.SetBool(isRescuehash, true);
        agent.SetDestination(RescuePosition);
        canRescue = false;//Da cuu roi thi khong cuu nua
        isRescue = true;
        DropItem();
    }

    void DropItem()
    {
        int random = UnityEngine.Random.Range(0, 100);

        if (random <= 100)
        {
            //drop ammo
            var item = dropItems.Find(i => i.name == "FirstAid");
            item.SpawnEntities(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));
        }
    }
}
