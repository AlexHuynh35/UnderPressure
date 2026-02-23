using UnityEngine;

public class Ability : ScriptableObject
{
    [Header("Ability Metadata")]
    public new string name;

    [Header("Ability Timings")]
    public float maxChargeTime;
    public float windUpTime;
    public float activeTime;
    public float windDownTime;
    public float sustainSpeed;
    public float castSpeed;

    [Header("Ability Range")]
    public float range;

    public virtual bool ActivationCriteriaMet(EntityManager caster)
    {
        return true;
    }
    public virtual void OnPress(EntityManager caster, Vector3 aimLocation) { }
    public virtual void WhileHold(EntityManager caster, Vector3 aimLocation) { }
    public virtual void OnRelease(EntityManager caster, Vector3 aimLocation) { }
    public virtual void StartActive(EntityManager caster, Vector3 aimLocation, float chargeTime) { }
    public virtual void WhileActive(EntityManager caster, Vector3 aimLocation, float chargeTime) { }
    public virtual void EndActive(EntityManager caster) { }
}
