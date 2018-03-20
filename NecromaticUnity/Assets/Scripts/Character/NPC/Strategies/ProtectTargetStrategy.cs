using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character.NPC.Strategies.Results;
using UniRx.Triggers;
using UniRx;

namespace Necromatic.Character.NPC.Strategies
{
    public class ProtectTargetStrategy : Strategy
    {
        private ProtectionManager _toProtect;
        private Vector3 _localTarget;
        private bool _hasTarget = false;
        public override StrategyResult Act(CharacterInstance sender, StrategyResult parameters)
        {
            if (!_hasTarget)
            {
                _localTarget = _toProtect.Subscribe(sender);
                _hasTarget = true;
                sender.Death.Dead.TakeUntilDestroy(sender).Subscribe(dead =>
                {
                    if (dead)
                    {
                        _toProtect.Unsubcribe(sender);
                    }
                });
            }
            var targetWorldPos = _toProtect.transform.position + _localTarget;
            var distance = ((targetWorldPos - sender.transform.position)).magnitude;
            return new MoveResult(targetWorldPos, 0.5f);
        }

        public ProtectTargetStrategy(Transform toProtect)
        {
            var protectionManager = toProtect.GetComponent<ProtectionManager>();
            if (protectionManager == null)
            {
                protectionManager = toProtect.gameObject.AddComponent<ProtectionManager>();
            }
            _toProtect = protectionManager;
        }
    }
}