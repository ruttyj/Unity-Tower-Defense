using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStatus : Status {
    [SerializeField]
    [Tooltip("Duration the status lasts")]
    public float duration = 12; // how long the effect lasts

    [SerializeField]
    [Tooltip("Percent movement slowed by")]
    public float slowPercent = 10; // how long the effect lasts

    //Settings
    float appliedTime = 0; // how long has status been applied for (Seconds)
    GameObject mTarget; // Target being affected by status
    float originalSpeed = 0;

    // Copy serialized fields
    public override void Copy(Status copyingGeneric) {
        IceStatus copyingStatus = copyingGeneric as IceStatus;
        duration = copyingStatus.duration;
        slowPercent = copyingStatus.slowPercent;
    }

    // Start is called before the first frame update
    void Start() {
        appliedTime = Time.deltaTime;
    }

    // Update is called once per frame
    void Update() {
        Die();
        appliedTime += Time.deltaTime;
    }

    /**
     * Method called when the status gets applied to a target
     */
    public override void Apply(GameObject target, GameObject projectile) {
        mTarget = target;
        float initialDamage = projectile.GetComponent<ProjectileController>().damage;
        float resistance = target.GetComponent<EnemyController>().IceResistance;
        float mitigation = target.GetComponent<EnemyController>().IceMitigation;

        // first time applying slow effect (this is so it does not stack)
        if (originalSpeed == 0) {
            originalSpeed = target.GetComponent<EnemyController>().speed;
            target.GetComponent<EnemyController>().speed -= originalSpeed * ((slowPercent - mitigation) / 100);
        }

        duration *= 1 - resistance;

        float damageTaken = initialDamage * (1 - mitigation);
        target.GetComponent<EnemyController>().Damage(damageTaken);
    }

    /**
     * Method called when target already has the effect active and gets reapplied the status
     */
    public override void Refresh() {
        appliedTime = 0;
    }

    /**
     * Check if the object should die
     */
    void Die() {
        if (appliedTime >= duration || !mTarget) {
            mTarget.GetComponent<EnemyController>().speed = originalSpeed;
            Destroy(this);
        }
    }
}
