using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
    [SerializeField]
    [Tooltip("Speed at which the projectile moves")]
    float speed;

    [SerializeField]
    [Tooltip("Damage the projective inflicts on a target")]
    public float damage;

    [SerializeField]
    [Tooltip("How long the projective should live before being destroyed (Garbage collection)")]
    float ttl;
    
    [SerializeField]
    [Tooltip("Status Effect that is applied")]
    GameObject statusEffect;

    // Settings
    GameObject mTarget;
    float lived = 0;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        SelfDestruct();
    }

    /**
     * Collider Event Listener
     */
    void OnTriggerEnter(Collider collision) {
        // currently no target and valid target enters detection
        // set colider as the new target
        if (collision.gameObject == mTarget) {
            if(statusEffect != null) {
                collision.gameObject.GetComponent<EnemyController>().ApplyStatus(gameObject, statusEffect.GetComponent<Status>());
            } else {
                // base case: no effect, do true damage to target
                collision.gameObject.GetComponent<EnemyController>().Damage(damage);
            }
            Destroy(gameObject);
        }
    }

    /*
     * Private Methods
     */
    void Move() {
        transform.position += GetVector() * speed * Time.deltaTime;
    }

    /**
     * Check if this object should be self destroyed for any reason
     */
    void SelfDestruct() {
        if(!mTarget || lived > ttl) {
            Destroy(gameObject);
        }
    }

    /**
     * Acquire attack vector
     */
    Vector3 GetVector() {
        if (mTarget) {
            return Vector3.Normalize(mTarget.transform.position - transform.position);
        }
        return new Vector3(0,0,0);
    }

    void UpdateTimers() {

        // Avoid putting the timer too low for now reason
        lived += Time.deltaTime;
    }

    /*
     * Setters
     */
    public void SetSpawn(Vector3 _spawn) {
        transform.position = _spawn;
    }

    public void SetTarget(GameObject _target) {
        mTarget = _target;
    }
}
