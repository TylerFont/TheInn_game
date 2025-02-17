using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public int destinationNumber;
    private float distance;
    public float distanceCheck = 0.3f;
    public float playerDistanceCheck = 3f;
    public float rayDistance = 5;
    public Transform eyes;
    private bool isFollowingPlayer;
    public int newDestinationNumber;
    public List<Transform> wayPoints;

    private GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.destination = wayPoints[destinationNumber].position;
    }


    private void FixedUpdate()
    {
        // go to  location x
        // If you are at destination. change to new destination
        Vector3 position = transform.position;
        distance = Vector3.Distance(wayPoints[destinationNumber].position, position);
        
        RaycastHit hit;
        Debug.DrawRay(eyes.position, eyes.forward * rayDistance, Color.red);
        if (Physics.Raycast(eyes.position, eyes.forward, out hit, rayDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                FollowPlayer();
            }
        }
        else if (distance<distanceCheck && !isFollowingPlayer)
        {
            GoToNewDestination(); 
        }
        if (isFollowingPlayer)
        {
            FollowPlayer();
        }
        
        // Make check points work with monster
    }
    

    private void FollowPlayer()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > playerDistanceCheck)
        {
            isFollowingPlayer = false;
            GoToNewDestination();
        }
        agent.destination = player.transform.position;
        Debug.Log("Моб гонится за игроком");
        isFollowingPlayer = true;
    }
    public void GoToNewDestination()
    {
        newDestinationNumber += 1;
        if (newDestinationNumber >= wayPoints.Count) 
            newDestinationNumber = 0;
        destinationNumber = newDestinationNumber;
        agent.destination = wayPoints[destinationNumber].position;
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) Debug.Log("Player should die here");
    }
}