using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.AI;

public class EnemyController1 : MonoBehaviour
{
    //public float moveSpeed;
    //public Rigidbody rb;

    private bool chasing;
    public float distanceToChase = 10f, distanceToLose = 15f, distanceToStop = 2f;
  
        Vector3 targetPoint, startPoint;
 
        public float keepChasingTime = 5f;
        private float chaseCounter;

        private NavMeshAgent agent;
 
        [Header("Bullet Variables")]
        public GameObject bullet;
        public Transform firePoint;

        public float fireRate, waitBetweenShots = 1f, timeToShoot = 2f;
        private float fireCount, shotWaitCounter, shootTimeCounter;

        public Animator anim;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPoint = transform.position;//will store his starting position when we start the game

        agent = GetComponent<NavMeshAgent>();

        shootTimeCounter = timeToShoot;
        shotWaitCounter = waitBetweenShots;
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(PlayerController1.instance.transform.position);
        //rb.linearVelocity = transform.forward * moveSpeed;

        
        targetPoint = PlayerController1.instance.transform.position;
        targetPoint.y = transform.position.y;//make y axis the same, do not depend on player y axis
        
        if (!chasing)//chasing = false
        {
            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            //within the range chasing equals to true
            {


                chasing = true;
                fireCount = 1f;
            }
            
            if (chaseCounter > 0)
            {
                chaseCounter -= Time.deltaTime;//counting until 0
                if (chaseCounter <= 0)
                {
                    agent.destination = startPoint;
                }
            }
            
            if (agent.remainingDistance < .25f)
            {
                anim.SetBool("isMoving", false);
            }
            else
            {
                anim.SetBool("isMoving", true);
            }
            
        }
            else//chasing = true, where the enemy will chase us
            {
                //transform.LookAt(targetPoint);
                //rb.linearVelocity = transform.forward * moveSpeed;
                
                if (Vector3.Distance(transform.position, targetPoint) > distanceToStop)
                {
                    agent.destination = targetPoint;//this will make agent chase the player
                }
                else
                {
                    //making distance for 2m, stop at his own current position
                    agent.destination = transform.position;
                }

               
                if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)//out of range stop chasing
                {
                    chasing = false;

                    //agent.destination = startPoint;//back to his starting position

                    chaseCounter = keepChasingTime;//give certain time for enemy
                }
                
                if (shotWaitCounter > 0)
                {
                    shotWaitCounter -= Time.deltaTime;

                    if (shotWaitCounter <= 0)
                    {
                        shootTimeCounter = timeToShoot;
                    }

                    anim.SetBool("isMoving", true);
                }
                else if (PlayerController1.instance.gameObject.activeInHierarchy)//only run the code whem he is active when he is alive
                {
                    shootTimeCounter -= Time.deltaTime;
                    if (shootTimeCounter > 0)
                    {
                        fireCount -= Time.deltaTime;



                        if (fireCount <= 0)
                        {
                            fireCount = fireRate;

                            firePoint.LookAt(PlayerController1.instance.transform.position + new Vector3(0f, 0.3f, 0f));

                            Vector3 targetDir = PlayerController1.instance.transform.position - transform.position;//get direction
                            float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

                            if (Mathf.Abs(angle) <= 30f)//abs:绝对值（因为有时候角度会为负数）
                            {
                                Instantiate(bullet, firePoint.position, firePoint.rotation);
                                anim.SetTrigger("fireShot");

                            }
                            else
                            {
                                shotWaitCounter = waitBetweenShots;
                            }

                            agent.destination = transform.position;

                        }
                    }
                    else
                    {
                        shotWaitCounter = waitBetweenShots;
                    }

                    anim.SetBool("isMoving", false);

                }
                

            }



        }
    }
