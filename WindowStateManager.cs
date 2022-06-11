using System.Collections.Generic;
using UnityEngine;
using WindowStateManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WindowStateManagement {
    [AddComponentMenu("WindowStateManagement/WindowStateManager")]
    public class WindowStateManager : MonoBehaviour {
        public const int MainWindowIndex = -1;
        [SerializeField, Min(-1)] int index = MainWindowIndex;
        public int CurrentIndex => this.index;
        [SerializeField] WindowControl mainWindow = null;
        [SerializeField] List<WindowControl> windows = new List<WindowControl>();
        private List<WindowControl> popupWindows = new List<WindowControl>();
        public bool ExistPopupWindows => this.popupWindows.Count > 0;
        //private bool popup = false;

        void Update() {
            UpdatePopupWindow();
        }

        public WindowControl CurrentWindow {
            get {
                if (this.index == WindowStateManager.MainWindowIndex) return this.mainWindow;
                else return this.windows[this.index];
            }
        }

        private void UpdatePopupWindow() {
            if (!this.ExistPopupWindows) return;
            if (this.popupWindows[0] == null || !this.popupWindows[0].gameObject.activeInHierarchy) {
                this.popupWindows.RemoveAt(0);
                if (this.ExistPopupWindows) {
                    this.CurrentWindow.SetInteractable(false);
                    WindowControl popupWindow = this.popupWindows[0];
                    popupWindow.gameObject.SetActive(true);
                    popupWindow.SetInteractable(true);
                    popupWindow.SelectWindow();
                } else {
                    this.CurrentWindow.gameObject.SetActive(true);
                    this.CurrentWindow.SetInteractable(true);
                    this.CurrentWindow.SelectWindow();
                }
            }
        }

        public void ShowPopupWindows(params WindowControl[] popupWindows) {
            foreach (var w in popupWindows) ShowPopupWindow(w);
        }
        public void ShowPopupWindow(WindowControl popupWindow) {
            if (popupWindow == null) {
                Debug.LogError($"WindowStateManager/ShowPopupWindow/this window is null.");
                return;
            }
            if (this.ExistPopupWindows) {
                popupWindow.gameObject.SetActive(false);
            } else {
                this.CurrentWindow.SetInteractable(false);
                popupWindow.gameObject.SetActive(true);
                popupWindow.SetInteractable(true);
                popupWindow.SelectWindow();
            }
            this.popupWindows.Add(popupWindow);
        }

        public void ChangeMainWindow() => this.ChangeWindow(WindowStateManager.MainWindowIndex);
        public void ChangeWindow(int index) {
            this.index = index;
            for (int i = 0; i < this.windows.Count; i++) {
                WindowControl w = this.windows[i];
                if (i == this.index) {
                    w.gameObject.SetActive(true);
                    w.SetInteractable(true);
                    w.SelectWindow();
                } else {
                    w.gameObject.SetActive(false);
                }
            }
            if (this.mainWindow != null) {
                if (this.index == WindowStateManager.MainWindowIndex) {
                    this.mainWindow.gameObject.SetActive(true);
                    this.mainWindow.SetInteractable(true);
                    this.mainWindow.SelectWindow();
                } else {
                    this.mainWindow.gameObject.SetActive(false);
                }
            }
        }
        public void ChangeThisWindow(WindowControl window) {
            int index = GetThisWindowIndex(window);
            ChangeWindow(index);
        }
        public int GetThisWindowIndex(WindowControl window) {
            for (int i = 0; i < this.windows.Count; i++) {
                if (this.windows[i] == window) return i;
            }
            if (window == null || window.gameObject == null) {
                Debug.LogError($"WindowStateManager/this window is null.");
            } else {
#if UNITY_EDITOR
                Debug.Log($"WindowStateManager/this window was not attacked. window name:{window.gameObject.name}");
#endif
                this.windows.Add(window);
                return GetThisWindowIndex(window);
            }
            return -2;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WindowStateManager))]
internal class WindowStateManagerEditor : Editor {
    //changeWindow
    private bool attachWindow = false;
    private int index = WindowStateManager.MainWindowIndex;
    private WindowControl window;
    //popupWindow
    [SerializeField] List<WindowControl> popupWindows = new List<WindowControl>();
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        var cls = target as WindowStateManager;
        EditorGUILayout.LabelField("~~~ Controller ~~~");
        EditorGUILayout.Space(4f);
        EditorGUILayout.LabelField("--- Change Window ---");
        this.attachWindow = EditorGUILayout.ToggleLeft("Attach Window", this.attachWindow);
        if (this.attachWindow) this.window = EditorGUILayout.ObjectField("Window", this.window, typeof(WindowControl), true) as WindowControl;
        else this.index = EditorGUILayout.IntField("Index", this.index);
        if (GUILayout.Button("ChangeWindow")) {
            if (this.attachWindow) cls.ChangeThisWindow(this.window);
            else cls.ChangeWindow(this.index);
        }
        EditorGUILayout.Space(4f);
        EditorGUILayout.LabelField("--- Popup Window ---");
        var so = new SerializedObject(this);
        so.Update();
        EditorGUILayout.PropertyField(so.FindProperty("popupWindows"), true);
        so.ApplyModifiedProperties();
        if (Application.isPlaying) {
            if (GUILayout.Button("Show Popup Window")) {
                cls.ShowPopupWindows(this.popupWindows.ToArray());
            }
        } else {
            EditorGUILayout.HelpBox("Execute is playing time only.", MessageType.Info);
        }
    }
}
#endif