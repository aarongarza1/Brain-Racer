using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(CarController))]
    public class CarAIControlPF : MonoBehaviour
    {
        public float maxSpeed = 100000000.0f;
        public float maxForce = 1000000.0f;
        public float maxDistance = 1.0f;
        public float targetWeight = 1.0f;
        public float obstacleWeight = 5.0f;
        public LayerMask obstacleLayer;
        private CarController m_CarController;
        private Rigidbody rb;
        private GameObject TargetGameObject;
        [SerializeField] private Transform m_Target;
        private Vector3 prev_Target;
        private void Awake()
        {
            // get the car controller reference
            m_CarController = GetComponent<CarController>();


            rb = GetComponent<Rigidbody>();

            if (gameObject.name.Length == 6)//one-digit number AI car has 6 characters (for example: AICar2)
            {
                TargetGameObject = GameObject.Find("TrackerAICar" + gameObject.name.Substring(gameObject.name.Length - 1));
            }
            if (gameObject.name.Length == 7)//two-digit number AI car has 7 characters (for example: AICar14)
            {
                TargetGameObject = GameObject.Find("TrackerAICar" + gameObject.name.Substring(gameObject.name.Length - 2));
            }

            m_Target = TargetGameObject.GetComponent<Transform>();
            prev_Target = Vector3.zero;
        }
        // Start is called before the first frame update
        void Update()
        {
            // Check for a new target position
            /*if ((m_target.position != prev_target))
            {
                prev_Target = m_target.position;
                targetPosition = hit.point;
                
            }*/
        }

        void FixedUpdate()
        {
            // Calculate steering forces
            Vector3 targetForce = CalculateTargetForce();
            Vector3 obstacleForce = CalculateObstacleForce();

            // Combine forces and apply to rigidbody
            Vector3 steeringForce = targetForce * targetWeight + obstacleForce * obstacleWeight;
            steeringForce.y = 0f; // Remove vertical force
            Vector3 acceleration = steeringForce / rb.mass;
            rb.AddForce(acceleration);
            print(acceleration);
            print(rb.velocity.magnitude);
            // Limit velocity
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
                print(rb.velocity);
            }
            // Face object in direction of velocity
            /*if (rb.velocity.magnitude > 0.1f)
            {
                transform.forward = Vector3.Slerp(transform.forward, rb.velocity.normalized, 0.1f);
            }*/
        }
        Vector3 CalculateTargetForce()
        {
            // Calculate force towards target position
            Vector3 desiredVelocity = (m_Target.position - transform.position).normalized * maxSpeed;
            Vector3 steeringForce = (desiredVelocity - rb.velocity).normalized * maxForce;

            // Limit force based on distance to target position
            float distanceToTarget = Vector3.Distance(transform.position, m_Target.position);
            if (distanceToTarget > maxDistance)
            {
                steeringForce *= maxDistance / distanceToTarget;
            }

            return steeringForce;
        }

        Vector3 CalculateObstacleForce()
        {
            // Calculate force away from nearby obstacles
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxDistance, obstacleLayer);
            Vector3 obstacleForce = Vector3.zero;
            foreach (Collider hitCollider in hitColliders)
            {
                Vector3 obstacleDirection = transform.position - hitCollider.transform.position;
                float obstacleDistance = obstacleDirection.magnitude;
                if (obstacleDistance > 0)
                {
                    obstacleForce += obstacleDirection.normalized / obstacleDistance;
                }
            }

            // Limit force based on distance to obstacles
            if (obstacleForce.magnitude > maxForce)
            {
                obstacleForce = obstacleForce.normalized * maxForce;
            }

            return obstacleForce;
        }
    }
}
