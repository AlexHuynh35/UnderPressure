using UnityEngine;

public class Ability : ScriptableObject
{
    [Header("Ability Metadata")]
    public new string name;

    [Header("Ability Timings")]
    public float maxChargeTime;
    public float windupTime;
    public float activeTime;
    public float cooldownTime;
    public float sustainSpeed;
    public float castSpeed;


    public virtual bool ActivationCriteriaMet(EntityManager caster)
    {
        return true;
    }
    public virtual void OnPress(EntityManager caster, Vector2 direction) { }
    public virtual void WhileHold(EntityManager caster, Vector2 direction) { }
    public virtual void OnRelease(EntityManager caster, Vector2 direction) { }
    public virtual void StartActive(EntityManager caster, Vector2 direction, float chargeTime) { }
    public virtual void WhileActive(EntityManager caster, Vector2 direction, float chargeTime) { }
    public virtual void EndActive(EntityManager caster) { }
}
