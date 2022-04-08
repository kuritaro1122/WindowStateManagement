using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindowStateManagement {
    public class WindowControl : MonoBehaviour {
        [SerializeField] WindowStateManager manager = null;
        [SerializeField] public GameObject firstSelectUI = null;
        public void ChangeMainWindow() => manager.ChangeMainWindow();
        public void ChangeWindow(int index) => manager.ChangeWindow(index);
        public void ChangeThisWindow() => manager.ChangeThisWindow(this);
        public int GetThisWindowIndex() => manager.GetThisWindowIndex(this);
    }
}