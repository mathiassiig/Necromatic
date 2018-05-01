using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Necromatic.Utility;
using System.Linq;
using Necromatic.Character.Abilities;
using Necromatic.Character.NPC;
using Necromatic.Character.Inventory;
using Necromatic.Character.Weapons;

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
        public CharacterInventory Inventory;
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

        void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            _representation.Init(this);
            Movement.Init(this);
            _health.Init(this);
            _health.Current.Subscribe(value =>
            {
                if (value <= 0)
                {
                    Death.Die(this);
                }
            });
            FindObjectOfType<MotherPool>().AddBarToCharacter(this);

            Inventory.WeaponSlot.TakeUntilDestroy(this).Subscribe(weapon =>
            {
                Combat.CurrentWeapon = weapon as Weapon;
            });

            Inventory.Init(_representation);
            Inventory.EquipAny(ItemType.Weapon, ItemSlotLocation.Weapon);
            Inventory.EquipAny(ItemType.Offhand, ItemSlotLocation.Offhand);


        }

        public bool UseOffhand(GameObject target)
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
                Combat.CurrentState.Value = CombatState.Attacking;
                Observable.Timer(System.TimeSpan.FromSeconds(Combat.TotalTime))
                    .TakeUntilDestroy(this)
                    .Subscribe(x =>
                    {
                        Combat.CurrentState.Value = CombatState.Idle;
                    });
                return true;
            }
            return false;
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
                        var attackSpeed = x.CurrentWeapon.Speed;
                        _representation.SetAttackSpeed(attackSpeed);
                    });
                }
            });
        }

    }
}