using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using WindowStateManagement;

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