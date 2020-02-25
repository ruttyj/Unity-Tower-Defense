using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    [SerializeField]
    [Tooltip("ExplosionPrefab")]
    public GameObject explosion;

    [SerializeField]
    [Tooltip("Health unit starts with")]
    float health;

    [SerializeField]
    [Tooltip("Speed unit moved at")]
    public float speed = 5.0f;

    [SerializeField]
    [Tooltip("Speed unit rotates at")]
    public float angularSpeed = 450.0f;

    [SerializeField]
    [Tooltip("Next position to move towards")]
    Transform targetWaypoint;

    /**
     * Elemental Resistances
     */
    // raw
    [SerializeField]
    [Tooltip("Raw Damage Resistance (Damage Reduction)[0-1]")]
    public float RawMitigation = 0;

    // fire
    [SerializeField]
    [Tooltip("Resist Fire (Timer shortened)[0-1]")]
    public float FireResistance = 0;

    [SerializeField]
    [Tooltip("Mitigation Fire (Damage Reduction)[0-1]")]
    public float FireMitigation = 0;

    // ice
    [SerializeField]
    [Tooltip("Resist Ice (Timer shortened)[0-1]")]
    public float IceResistance = 0;

    [SerializeField]
    [Tooltip("Mitigation Ice (Damage Reduction)[0-1]")]
    public float IceMitigation = 0;

    // nature 
    [SerializeField]
    [Tooltip("Resist Nature (Timer shortened)[0-1]")]
    public float NatureResistance = 0;

    [SerializeField]
    [Tooltip("Mitigation Nature (Damage Reduction)[0-1]")]
    public float NatureMitigation = 0;

    [SerializeField]
    [Tooltip("enemy type (to fetch revenue info)")]
    public string enemyType;

    [SerializeField]
    [Tooltip("gold per enemy killed of this type")]
    public int reward = 10;

    //private List<Status> statusEffects = new List<Status>();

    private int waypointIndex = 0;

    //Health Bar UI
    [SerializeField]
    public Image healthBar;
    private float initialHealth;

    // Start is called before the first frame update
    void Start()
    {
        //set reward
        reward = RevenueTemplate.revenueDictMonsters[enemyType];
        //set target waypoint
        targetWaypoint = FindObjectOfType<Waypoints>().GetNextWaypoint(waypointIndex);
        waypointIndex++;
        //Initial health bar fill
        healthBar.fillAmount = 1;
        initialHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        Die();
        Move();
    }
    
    /**
     * Public methods
     */
    void Move() {

        //transform.position += Vector3.left * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, targetWaypoint.position) <= 0.7f)
        {
            targetWaypoint = FindObjectOfType<Waypoints>().GetNextWaypoint(waypointIndex);
            if (targetWaypoint == null)
            {
                GetComponentInParent<EnemySpawnerController>().ReachedEnd();
                Kill();
                return;
            }
            waypointIndex++;
        }

        Vector3 direction = targetWaypoint.position - transform.position;
        transform.position += direction.normalized * speed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), angularSpeed * Time.deltaTime);
    }

    public void Damage(float damage) {
        health -= damage;
        float healthCalc = health / initialHealth;
        healthBar.fillAmount = healthCalc;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    /**
     * Private methods
     */
    public void ApplyStatus(GameObject projectile, Status status) {
        Type type = status.GetType();
        Status alreadyApplied = gameObject.GetComponent(type) as Status;

        // New Status to apply
        if (!alreadyApplied) {
            Status appliedStatus = gameObject.AddComponent(type) as Status;
            appliedStatus.Copy(status);
            appliedStatus.Apply(gameObject, projectile);
        }

        // Status currently applied
        else {
            alreadyApplied.Refresh();
            alreadyApplied.Apply(gameObject, projectile);
        }
    }

    void MakeExplode() 
    {
        // Explosions make me happy =D
        if (explosion != null)
            Instantiate(explosion, transform.position, transform.rotation);
    }

    public void Kill()
    {
        MakeExplode();
        Destroy(gameObject, 0);
    }

    /**
     * Check if the object should die
     */
    void Die() {
        if(health <= 0) {
            GameController.EnemyKilled(reward);
            Kill();
        }
    }
}
