using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Necromatic.UI
{
    public class HotKeyButton : MonoBehaviour
    {
        [SerializeField] private string _title;
        [SerializeField] private KeyCode _key;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] FontStyles _hotKeyStyle;
        [SerializeField] private Button _button;

        private void Awake()
        {
            SetTitle();
        }

        private void Update()
        {
            if(Input.GetKeyDown(_key))
            {
                _button.onClick.Invoke();
            }
        }

        public void SetButton(UnityAction action)
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(action);
        }

        void SetTitle()
        {
            var keyChar = _key.ToString();
            bool isAlphaBet = Regex.IsMatch(keyChar, "[a-z]", RegexOptions.IgnoreCase);
            string hotkeyrt = HotKeyStyleRichText();
            if (isAlphaBet && hotkeyrt != null)
            {
                var lowerCase = keyChar.ToLower();
                if (_title.Contains(keyChar) || _title.Contains(lowerCase))
                {
                    var characters = new char[] { lowerCase[0], keyChar[0] };
                    var first = _title.IndexOfAny(characters);
                    var before = $"<{hotkeyrt}>";
                    _title = _title.Insert(first, before);
                    var after = $"</{hotkeyrt}>";
                    _title = _title.Insert(first + 1 + before.Length, after);
                }
            }
            _text.text = _title;
        }

        string HotKeyStyleRichText()
        {
            switch(_hotKeyStyle)
            {
                case FontStyles.Bold:
                    return "b";
                case FontStyles.Underline:
                    return "u";
                case FontStyles.Italic:
                    return "i";
            }
            return null;
        }

    }
}