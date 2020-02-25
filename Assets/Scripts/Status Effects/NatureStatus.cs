using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureStatus : Status {
    [SerializeField]
    [Tooltip("Duration the status lasts")]
    public float duration = 12; // how long the effect lasts

    [SerializeField]
    [Tooltip("Time between between tics effect (Seconds)")]
    public float ticTime = 3;

    [SerializeField]
    [Tooltip("Elemental damage applied every tic")]
    public float ticDamage;

    //Settings
    float lastTic = 0; // time since last time tick was applied
    float appliedTime = 0; // how long has status been applied for (Seconds)
    GameObject mTarget; // Target being affected by status

    // Copy serialized fields
    public override void Copy(Status copyingGeneric) {
        NatureStatus copyingStatus = copyingGeneric as NatureStatus;
        duration = copyingStatus.duration;
        ticTime = copyingStatus.ticTime;
        ticDamage = copyingStatus.ticDamage;
    }

    // Start is called before the first frame update
    void Start() {
        appliedTime = Time.deltaTime;
    }

    // Update is called once per frame
    void Update() {
        Die();
        appliedTime += Time.deltaTime;
        lastTic += Time.deltaTime;
        ApplyTic();
    }

    void ApplyTic() {
        if (lastTic >= ticTime) {
            lastTic = 0;
            float mitigation = mTarget.GetComponent<EnemyController>().NatureMitigation;
            float damageTaken = ticDamage * (1 - mitigation);
            mTarget.GetComponent<EnemyController>().Damage(damageTaken);
        }
    }

    /**
     * Method called when the status gets applied to a target
     */
    public override void Apply(GameObject target, GameObject projectile) {
        mTarget = target;
        float initialDamage = projectile.GetComponent<ProjectileController>().damage;
        float resistance = target.GetComponent<EnemyController>().NatureResistance;
        float mitigation = target.GetComponent<EnemyController>().NatureMitigation;

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
            Destroy(this);
        }
    }
}
