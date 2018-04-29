using System.Collections;
using System.Collections.Generic;
using Necromatic.Utility;
using Necromatic.World;
using UnityEngine;
using System.Linq;
using UniRx;
using Necromatic.Character.NPC.Strategies.Results;

namespace Necromatic.Character.NPC
{
    public class HumanCharacter : CharacterInstance, IRaisable, IClickReceiver
    {
        protected CharacterType _undeadToRaise;
        [SerializeField] private Transform _pelvis;

        public void Click(List<ISelectable> senders)
        {
            if(Death.Dead.Value)
            {
                Raise();
            }
            else if(senders.Count > 0)
            {
                senders.ForEach(x => x.AI.AddTask(new EnemySpottedResult(this)));
            }
        }

        public void Raise()
        {
            if(!Death.Dead.Value)
            {
                return;
            }
            var motherpool = FindObjectOfType<MotherPool>();
            var undeadPrefab = motherpool.GetCharacterPrefab(_undeadToRaise);
            var undead = Instantiate(undeadPrefab, _pelvis.position, Quaternion.identity);
            var ai = undead.GetComponent<ArtificialIntelligence>();
            ai.SetBrainState(false);
            //var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterInstance>();
            undead.Representation.ReviveAnimation(() =>
            {
                ai.SetBrainState(true);
                //undead.Representation.LookDirectionAnim(MathUtils.PlaneDirection(undead.transform, player.transform), 0.3f);
            });
            Destroy(gameObject);
        }

        protected override void Init()
        {
            _death = new HumanDeath();
            var combat = new Combat(this);
            Combat = combat;
            _undeadToRaise = CharacterType.Skeleton;
            base.Init();
            _death.Dead.TakeUntilDestroy(this).Subscribe(dead =>
            {
                if(dead)
                {                    
                    var collider = GetComponent<Collider>();
                    if (collider != null)
                    {
                        Destroy(collider);
                    }
                    var sphereCollider = gameObject.AddComponent<SphereCollider>();
                    
                    Observable.EveryUpdate().TakeUntilDestroy(this).Subscribe(y =>
                    {
                        var offset = _pelvis.transform.position - transform.position;
                        sphereCollider.center = offset;
                    });
                }

            });
        }
    }
}