using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromatic.Character
{
    public class ProtectionManager : MonoBehaviour
    {
        private List<CharacterInstance> _protectors = new List<CharacterInstance>();
		private float _distance = 4;
        public Vector3 Subscribe(CharacterInstance protector)
        {
            _protectors.Add(protector);
			var position = GetLocalPosition(_protectors.Count - 1);
            print(position);
			return position;
        }

        Vector3 GetLocalPosition(int index)
        {
            switch (index)
            {
                case 0:
                    return _distance * Vector3.right;
				case 1:
					return _distance * Vector3.left;
				case 2:
					return _distance * Vector3.forward;
				case 3:
					return _distance * Vector3.back;
            }
			return Vector3.zero;
        }



        public void Unsubcribe(CharacterInstance protector)
        {
            _protectors.Remove(protector);
        }
    }
}