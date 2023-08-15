using UnityEngine;
[DisallowMultipleComponent]
public class CacodaemonController : CharacterControllerBase, IEffectable
{

    [SerializeField]
    private Stats _cacodaemonStats;

    private StatusEffectBase _statusEffect;
    private float _currentEffectTime;
    private float _nextTickTime;

    public void TakeDamage(Damage damage)
    {
        damage.OnApply(_cacodaemonStats);
    }

    public void ApplyEffect(StatusEffectBase statusEffect)
    {
        _statusEffect = statusEffect;
    }

    public void HandleEffect()
    {
        _currentEffectTime += Time.deltaTime;

        if (_currentEffectTime >= _statusEffect.duration)
        {
            RemoveEffect();
        }

        DamageOverTimeEffect damageOverTimeEffect = _statusEffect as DamageOverTimeEffect;
        SpeedMultiplierEffect speedMultiplierEffect = _statusEffect as SpeedMultiplierEffect;
        if (damageOverTimeEffect && _currentEffectTime > _nextTickTime)
        {
            _nextTickTime += damageOverTimeEffect.tickSpeed;
            _cacodaemonStats.GetStat("Health").Subtract(damageOverTimeEffect.dotAmount);
            Debug.Log("Health = " + _cacodaemonStats.GetStat("Health").Value);
        }
    }

    public void RemoveEffect()
    {
        _statusEffect = null;
        _currentEffectTime = 0;
        _nextTickTime = 0;
    }

    protected override void Start()
    {
        base.Start();

        SetupStateMachine();
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();
        _stateMachine.Enter();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }
}
