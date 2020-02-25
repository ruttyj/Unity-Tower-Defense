using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerController : MonoBehaviour
    
    //ISelectHandler, IDeselectHandler
{
    [SerializeField]
    [Tooltip("Layer that is targetable by the tower")]
    LayerMask targetableLayer;

    [SerializeField]
    [Tooltip("Cooldown (in seconds) between shots")]
    float cooldown;

    [SerializeField]
    [Tooltip("Projectile that is shot from the tower")]
    GameObject gProjectile;

    [SerializeField]
    [Tooltip("Location that the projectile will be spawned at")]
    GameObject projectileSpawn;

    [SerializeField]
    [Tooltip("string indicator of tower type")]
    string towerType;


    [SerializeField]
    bool rotateTowardEnemy = false;

    [SerializeField]
    float rotationSpeed = 1000F;

    [SerializeField]
    GameObject towerWepon = null;

    [SerializeField]
    public string formattedName;

    [SerializeField]
    public string nextTowerCost;

    //[SerializeField]
    //GameObject _GM;


    // Private tower settings
    private GameObject mTarget; // current target
    float shotCooldown = 0;

    // shooting sfx
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        //_GM = GameObject.FindGameObjectWithTag("_GM");
    }





    // Update is called once per frame
    void Update()
    {
        UpdateTimers();
        UpdateWeponRotation();
        Attack();
    }


    void UpdateWeponRotation()
    {
        if (rotateTowardEnemy && towerWepon != null)
        {
            if (mTarget != null)
            {
                Vector3 lookAt = mTarget.transform.position - towerWepon.transform.position;
                lookAt.y = 0;
                towerWepon.transform.rotation = Quaternion.RotateTowards(towerWepon.transform.rotation, Quaternion.LookRotation(lookAt), rotationSpeed * Time.deltaTime);
            }
        }
    }

    /**
     * Set a game object as new target
     */
    void SetTarget(GameObject newTarget) {
        mTarget = newTarget;
    }

    /**
     * Collider Event Listener
     */
    void OnTriggerEnter(Collider collision) {
        // currently no target and valid target enters detection
        // set colider as the new target
        if((targetableLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer && !mTarget) {
            SetTarget(collision.gameObject);
        }
    }

    /**
     * Target leaves trigger zone
     */
    void OnTriggerExit(Collider collision) {
        // currently no target and valid target enters detection
        // set colider as the new target
        if (collision.gameObject == mTarget) {
            mTarget = null;
        }
    }

    /**
     * Targets are within trigger zone
     */
    void OnTriggerStay(Collider collision) {
        if ((targetableLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer && !mTarget) {
            SetTarget(collision.gameObject);
        }
    }

    void UpdateTimers() {

        // Avoid putting the timer too low for now reason
        if(shotCooldown >= 0) {
            shotCooldown -= Time.deltaTime;
        }
    }

    /**
     * Attack the target
     */
    void Attack() {
        if (mTarget && shotCooldown <= 0) {
            shotCooldown = cooldown;
            SpawnProjectile();
            audioSource.Play();
        }
    }

    /**
     * Spawn the attack projectile
     */
    void SpawnProjectile() {
        GameObject shot = Instantiate(gProjectile) as GameObject;
        shot.GetComponent<ProjectileController>().SetSpawn(projectileSpawn.transform.position);
        shot.GetComponent<ProjectileController>().SetTarget(mTarget);
    }

    //accessors
    public string GetTowerType()
    {
        return towerType;
    }
    public void SetTowerType(string type)
    {
        towerType = type;
    }

    //public void OnSelect(BaseEventData eventData)
    //{
    //    transform.GetComponent<Outline>().enabled = true;

    //    if (_GM != null)
    //    {
    //        _GM.GetComponent<TowerBuildingManager>().SpawnMenu(transform.gameObject);
    //    }
    //}

    //public void OnDeselect(BaseEventData eventData)
    //{
    //    transform.GetComponent<Outline>().enabled = false;

    //    if(_GM != null)
    //    {
    //        _GM.GetComponent<TowerBuildingManager>().DeSpawnMenu();
    //    }
    //}
}
