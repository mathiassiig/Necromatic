using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace Necromatic.UI
{
    /// <summary>
    /// Handles the hotkeys and showing of buildable options
    /// </summary>
    public class BuildMenu : MonoBehaviour
    {
        [SerializeField] private Transform _menuItems;
        [SerializeField] private List<BuildHotKey> _hotKeys;
        [SerializeField] private HotKeyButton _hotkeyPrefab;
        [SerializeField] private BuildInterface _buildInterface;

        private bool _activated
        {
            get
            {
                return _menuItems.gameObject.activeInHierarchy;
            }
            set
            {
                _menuItems.gameObject.SetActive(value);
            }
        }
        public readonly ReactiveProperty<bool> Activated = new ReactiveProperty<bool>();

        private void DestroyBuildMenu()
        {
            foreach (Transform t in _menuItems)
            {
                Destroy(t.gameObject);
            }
        }

        private void InitBuildMenu()
        {
            foreach (var h in _hotKeys)
            {
                var b = Instantiate(_hotkeyPrefab);
                b.transform.SetParent(_menuItems);
                b.Init(h);
                var rect = b.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
                rect.localScale = Vector3.one;
                b.SetButton(() =>
                {
                    _buildInterface.BeginBuild(h.BuildingPrefab);
                    SetActivate(false);
                });
            }
        }

        private void Awake()
        {
            SetActivate(false);
            Activated.Subscribe(x =>
            {
                if (x)
                {
                    InitBuildMenu();
                }
                else
                {
                    DestroyBuildMenu();
                }
            });
        }

        private void SetActivate(bool value)
        {
            _activated = value;
            Activated.Value = _activated;
        }

        private void Toggle()
        {
            SetActivate(!_activated);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                Toggle();
            }
            if (Input.GetKeyDown(KeyCode.Escape) && _activated)
            {
                SetActivate(false);
            }
        }
    }
}