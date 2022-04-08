using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

namespace WindowStateManagement {
    [AddComponentMenu("WindowStateManagement/WindowStateManager")]
    public class WindowStateManager : MonoBehaviour {
        public const int MainWindowIndex = -1;
        [SerializeField, Min(-1)] int index = MainWindowIndex;
        public int Index => this.index;
        [SerializeField] WindowControl mainWindow = null;
        [SerializeField] WindowControl[] windows = null;

        public void ChangeMainWindow() => this.ChangeWindow(MainWindowIndex);
        public void ChangeWindow(int index) {
            
            this.index = index;
            for (int i = 0; i < this.windows.Length; i++) {
                WindowControl w = this.windows[i];
                ChangeWindow(w, this.index == i);
            }
            if (this.mainWindow != null) ChangeWindow(this.mainWindow, this.index == MainWindowIndex);
            void ChangeWindow(WindowControl window, bool change) {
                window.gameObject.SetActive(change);
                if (change) SetWindowToEventSystem(window);
            }
            void SetWindowToEventSystem(WindowControl window) {
                EventSystem eventSystem = EventSystem.current;
#if UNITY_EDITOR
                if (eventSystem == null) {
                    Debug.Log("WindowStateManager/EventSystem is null.");
                    return;
                }
#endif
                eventSystem.firstSelectedGameObject = window.gameObject;
                if (window.firstSelectUI != null)
                    eventSystem.SetSelectedGameObject(window.firstSelectUI);
            }
        }
        public void ChangeThisWindow(WindowControl window) {
            int index = GetThisWindowIndex(window);
            ChangeWindow(index);
        }
        public int GetThisWindowIndex(WindowControl window) {
            for (int i = 0; i < this.windows.Length; i++) {
                if (this.windows[i] == window) return i;
            }
            Debug.LogError($"WindowStateManager/this window was not attacked. window name:{window.gameObject.name}");
            return -2;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(WindowStateManager))]
    internal class WindowStateManagerEditor : Editor {
        private bool attachWindow = true;
        private int index = WindowStateManager.MainWindowIndex;
        private WindowControl window;
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var cls = target as WindowStateManager;
            EditorGUILayout.LabelField("~~~ Controller ~~~");
            this.attachWindow = EditorGUILayout.ToggleLeft("Attach Window", this.attachWindow);
            if (this.attachWindow) this.window = EditorGUILayout.ObjectField("Window", this.window, typeof(WindowControl), true) as WindowControl;
            else this.index = EditorGUILayout.IntField("Index", this.index);
            if (GUILayout.Button("ChangeWindow")) cls.ChangeWindow(this.index);
        }
    }
#endif
}