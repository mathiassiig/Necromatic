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
    [ExecuteInEditMode]
    public class HotKeyButton : MonoBehaviour
    {
        [SerializeField] private string _title;
        [SerializeField] private KeyCode _key;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] Color _hotKeyStyle;
        [SerializeField] private Button _button;

        private void OnEnable()
        {
            SetTitle();
        }

        private void OnValidate()
        {
            SetTitle();
        }

        private void Update()
        {
            if(Application.isPlaying && Input.GetKeyDown(_key) && _button.interactable)
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
            var titleStyled = _title;
            if (isAlphaBet)
            {
                var lowerCase = keyChar.ToLower();
                if (_title.Contains(keyChar) || _title.Contains(lowerCase))
                {
                    var characters = new char[] { lowerCase[0], keyChar[0] };
                    var first = _title.IndexOfAny(characters);
                    var before = $"<{HotKeyRichTextBegin()}>";
                    titleStyled = titleStyled.Insert(first, before);
                    var after = $"</{HotKeyRichTextEnd()}>";
                    titleStyled = titleStyled.Insert(first + 1 + before.Length, after);
                }
            }
            _text.text = titleStyled;
        }

        string HotKeyRichTextBegin()
        {
            return "color=#"+ColorUtility.ToHtmlStringRGBA(_hotKeyStyle);
        }

        string HotKeyRichTextEnd()
        {
            return "color";
        }

    }
}