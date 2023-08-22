using System.Collections.Generic;

using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _weaponDisplay;
    
    [SerializeField]
    private CharacterData _playerData;

    private ItemBase _currentlyHolding;
    private GameObject _pooledArrows;
    private Transform _heldItemContainer;
    private List<ArrowController> _pooledArrowList;
    private Vector3 _mousePositon;
    private Plane _detectionPlane;
    private Quaternion _weaponFlipAngle;
    private Vector3 _flipVector;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private PlayerController _player;

    private void Awake()
    {
        WeaponDamage.OnWeaponHit += DealDamage;
    }
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _player = GetComponent<PlayerController>();

        _pooledArrows = transform.GetChild(1).gameObject;
        _pooledArrowList = new List<ArrowController>();

        foreach (Transform child in _pooledArrows.transform)
        {
            _pooledArrowList.Add(child.
                GetComponent<ArrowController>());
        }

        _detectionPlane = new Plane(Vector3.up, -0.7f);

        _heldItemContainer = transform.GetChild(0);
        _weaponFlipAngle = Quaternion.Euler(0f, -360f, 0f);
        _flipVector = new(-1f, 1f, 1f);
        // TODO (Cheng Jun): This should be updated to try
        // and fetch the player's local save instead of performing
        // a reset once the save system is ready
        _playerData.Reset();
    }

    private void Update()
    {
        CalculateMousePos();
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_currentlyHolding is IConsumable consumable)
            {
                consumable.ApplyConsumptionEffect(_playerData.CharacterStats, _player);
                _playerData.HandInventory.Remove(_currentlyHolding, 1);
            }

            if (_currentlyHolding is WeaponBase weapon && !weapon.CanAttack)
            {
                return;
            }
             
            if (_currentlyHolding is ISwordWeapon swordWeapon)
            {
                _animator.Play(swordWeapon.AnimationName);
                _rigidbody.AddForce(2 * transform.right, ForceMode.Impulse);
            }

            if (_currentlyHolding is IBowWeapon bowWeapon)
            {
                _animator.Play(bowWeapon.AnimationName);
                _rigidbody.AddForce(1f * transform.right, ForceMode.Impulse);

                Vector3 aimDirection = _mousePositon - _rigidbody.position;
                Vector3 shootDirection =
                     new(aimDirection.x, 0f, aimDirection.z);

                for (int i = 0; i < _pooledArrowList.Count; ++i)
                {
                    if (_pooledArrowList[i].gameObject.activeSelf)
                    {
                        continue;
                    }

                    bowWeapon.Shoot(_pooledArrowList[i],
                        shootDirection.normalized, _pooledArrows.transform);

                    break;
                }
            }

            if (_currentlyHolding is IMagicWeapon magicWeapon)
            {
                _animator.Play(magicWeapon.AnimationName);
                _rigidbody.AddForce(2f * transform.right, ForceMode.Impulse);

                Vector3 aimDirection = _mousePositon - _rigidbody.position;
                Vector3 shootDirection =
                     new(aimDirection.x, 0f, aimDirection.z);

                for (int i = 0; i < _pooledArrowList.Count; ++i)
                {
                    if (_pooledArrowList[i].gameObject.activeSelf)
                    {
                        continue;
                    }

                    magicWeapon.Shoot(_pooledArrowList[i],
                        shootDirection.normalized, _pooledArrows.transform);

                    _playerData.CharacterStats.GetStat("Sanity")
                        .Subtract(magicWeapon.SanityCost);

                    break;
                }
            }

            if (_currentlyHolding is IBeginUseHandler beginUseHandler)
            {
                beginUseHandler.OnUseEnter();
            }
        }

        if (_currentlyHolding is IUseHandler useHandler)
        {
            useHandler.OnUse();
        }
            
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (_currentlyHolding is IEndUseHandler endUseHandler)
            {
                endUseHandler.OnUseExit();
            }
        }  
            
            _heldItemContainer.transform.localScale =
                transform.rotation.eulerAngles.y < 180f ? Vector3.one :
                _flipVector;

            _heldItemContainer.transform.rotation =
                transform.rotation.eulerAngles.y < 180f ? _weaponFlipAngle :
                Quaternion.identity;
    }

    private void DealDamage(IEffectable effectable, Vector3 hitPos)
    {
        if (_currentlyHolding is WeaponBase weapon)
        {
            Vector3 knockbackForce = 
                (hitPos - transform.position).normalized *
                weapon.WeaponStats.GetStat("Knockback").Value;
            
            effectable.TakeDamage(weapon.WeaponDamageType.
                AddModifier(Modifier.Multiply(
                _playerData.CharacterStats.GetStat("DamageMultiplier").Value,3)
                ), knockbackForce);

            if (weapon.WeaponStatusEffect)
            {
                effectable.ApplyEffect(weapon.WeaponStatusEffect.Clone());
            }
        }
    }

    public void Equip(ItemBase itemToEquip, uint quantity)
    {
        if (itemToEquip is WeaponBase)
        {
            _weaponDisplay.sprite = itemToEquip.Sprite;
        }
        
        _currentlyHolding = itemToEquip;   
    }

    private void CalculateMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (_detectionPlane.Raycast(ray, out float distance))
        {
            _mousePositon = ray.GetPoint(distance);
        }
    }
}
