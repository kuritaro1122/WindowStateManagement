using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using WindowStateManagement;

#if UNITY_EDITOR
[CustomEditor(typeof(WindowControl))]
internal class WindowControlEditor : Editor {
    private bool interactable = true;
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        var cls = target as WindowControl;
        EditorGUILayout.LabelField("~~~ Controller ~~~");
        this.interactable = cls.Interactable;
        this.interactable = EditorGUILayout.ToggleLeft("interactable", this.interactable);
        if (cls.Interactable != this.interactable) {
            cls.SetInteractable(this.interactable);
        }
        if (GUILayout.Button("InitSelectables")) {
            cls.InitSelectables();
        }
    }
}
#endif