using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using WindowStateManagement;
//#if UNITY_EDITOR
using UnityEditor;
//#endif

namespace WindowStateManagement {
    public class WindowControl : MonoBehaviour {
        [SerializeField] WindowStateManager manager = null;
        [SerializeField] public GameObject firstSelectUI = null;
        [SerializeField] public bool InitOnStart = true;
        [SerializeField] Selectable[] selectables = null;
        [SerializeField] bool[] selectables_interactable = null;
        public bool Interactable { get; private set; } = true;

        public void ChangeMainWindow() => manager.ChangeMainWindow();
        public void ChangeWindow(int index) => manager.ChangeWindow(index);
        public void ChangeThisWindow() => manager.ChangeThisWindow(this);
        public int GetThisWindowIndex() => manager.GetThisWindowIndex(this);

        void Start() {
            if (this.InitOnStart) InitSelectables();
        }

        public void InitSelectables() {
            this.selectables = this.GetComponentsInChildren<Selectable>();
            this.selectables_interactable = new bool[this.selectables.Length];
            for (int i = 0; i < this.selectables.Length; i++) {
                this.selectables_interactable[i] = this.selectables[i].interactable;
            }
        }
        public void SetInteractable(bool interactable) {
            this.Interactable = interactable;
            if (this.selectables == null || this.selectables.Length != this.selectables_interactable.Length) {
#if UNITY_EDITOR
                Debug.LogWarning("WindowsControl/Prease init!! (editor only)");
#endif
                InitSelectables();
            }
            for (int i = 0; i < this.selectables.Length; i++) {
                if (this.selectables_interactable[i]) {
                    this.selectables[i].interactable = this.Interactable;
                }
            }
        }

        public void SelectWindow(EventSystem eventSystem) {
            if (eventSystem == null) {
                Debug.LogWarning("WindowStateManager/EventSystem is null.");
                return;
            }
            eventSystem.firstSelectedGameObject = this.firstSelectUI;
            if (this.firstSelectUI != null) {
                eventSystem.SetSelectedGameObject(this.firstSelectUI);
            }
        }
        public void SelectWindow() => SelectWindow(EventSystem.current);
    }
}

