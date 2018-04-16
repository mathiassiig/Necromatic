using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Necromatic.Utility;
using System.Linq;
using Necromatic.Character.Abilities;
using Necromatic.Character.NPC;
using Necromatic.Character.Inventory;

namespace Necromatic.Character
{
    [System.Serializable]
    public enum Faction
    {
        Undead,
        Human
    }

    public class CharacterInstance : MonoBehaviour, IDamagable
    {
        [SerializeField] protected Faction _faction;
        [SerializeField] protected Movement _movement;
        protected Death _death;
        public InventoryInstance Inventory;
        [SerializeField] protected Representation _representation;
        [SerializeField] protected Stat _health;

        private ArtificialIntelligence _ai;
        public ArtificialIntelligence AI
        {
            get
            {
                if (_ai == null)
                {
                    _ai = GetComponent<ArtificialIntelligence>();
                }
                return _ai;
            }
        }

        public Ability CurrentAbility;

        protected WeaponAnimator _animator = new WeaponAnimator();
        // accessors
        [HideInInspector] public Combat Combat = null;
        public Faction Faction => _faction;
        public Movement Movement => _movement;
        public Death Death => _death;
        public Representation Representation => _representation;
        public Stat Health => _health;

        private List<Buff> _buffs = new List<Buff>();
        public List<Buff> Buffs => _buffs;

        Combat IDamagable.Combat => Combat;

        private System.IDisposable _combatSwitchDisposable;

        public void AddBuff(Buff buff)
        {
            _buffs.Add(buff);
        }

        void Awake()
        {
            Init();
        }



        protected virtual void Init()
        {
            InitCombat();
            Movement.Init(this);
            _health._initialRegen = 0.5f;
            _health.Init(this);
            _health.Current.Subscribe(value =>
            {
                if (value <= 0)
                {
                    Death.Die(this);
                }
            });
            FindObjectOfType<MotherPool>().AddBarToCharacter(this);
            Inventory.Init(_representation);
            Inventory.EquipAny(ItemType.Weapon, ItemSlotLocation.Weapon);
            Inventory.EquipAny(ItemType.Offhand, ItemSlotLocation.Offhand);
        }

        public void UseOffhand(GameObject target)
        {
            if (Inventory.OffhandSlot.Value != null && Inventory.OffhandSlot.Value.GameObjectInstance != null)
            {
                var offhandScript = Inventory.OffhandSlot.Value.GameObjectInstance.GetComponent<IOffhand>();
                _representation.Offhand();
                Observable.Timer(System.TimeSpan.FromSeconds(Combat.ForwardTime))
                    .TakeUntilDestroy(this)
                    .Subscribe(x =>
                    {
                        offhandScript.Use(target, this);
                    });
                Combat.CurrentState.Value = CombatState.Offhand;
                Observable.Timer(System.TimeSpan.FromSeconds(Combat.ForwardTime + Combat.RetractTime))
                    .TakeUntilDestroy(this)
                    .Subscribe(x =>
                    {
                        Combat.CurrentState.Value = CombatState.Idle;
                    });
            }
        }

        void InitCombat()
        {
            this.ObserveEveryValueChanged(x => x.Combat).TakeUntilDestroy(this).Subscribe(x =>
            {
                if (_combatSwitchDisposable != null)
                {
                    _combatSwitchDisposable.Dispose();
                }
                if (x != null && x.CurrentState != null)
                {
                    x.CurrentState.TakeUntilDestroy(this).Subscribe(state =>
                    {
                        var attackSpeed = 1 / (x.ForwardTime + x.RetractTime);
                        _representation.SetAttackSpeed(attackSpeed);
                        _representation.Attack(state);
                    });
                }
            });
        }

        public void AttackNearest()
        {
            Combat.TryAttackNearest(this);
        }

    }
}