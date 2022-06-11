# WindowStateManagement

ウィンドウ（GameObject）が常にどれか一つだけ表示するようにする。ウィンドウの切り替えとEventSystemへの選択オブジェクトの受け渡しを管理する。

# Requirement
* System.Collections.Generic
* UnityEngine
* UnityEngine.EventSystems
* UnityEngine.UI
* UnityEditor

# Usage
① WindowStateManagementを常に存在するオブジェクトにコンポーネント.\
② WindowControlをウィンドウとして扱いたいオブジェクトにコンポーネント.\
③ WindowStateManagementのメソッドを呼んで,任意のウィンドウを表示.\

# WindowStateManager

WindowControlを管理する. 任意のウィンドウに切り替える.

# Contains

## Inspector

--

## Public Variable
```
int MainWindowIndex
int CurrentIndex
bool ExistPopupWindows
WindowControl CurrentWindow
```

## Public Function
```
void ChangeMainWindow()
void ChangeWindow(int index)
void ChangeThisWindow(WindowControl window)
void ShowPopupWindow(WindowControl popupWindow)
void ShowPopupWindows(params WindowControl[] popupWindows)
int GetThisWindowIndex(WindowControl window)
```

# WindowControl

任意のオブジェクトにWindowとしての機能を与える. 動作させるにはWindowStateManagerをアタッチする必要がある.

# Contains

## Inspector

--

## Public Variable
```
bool Interactable
```

## Public Function
```
void InitSelectables()
void ChangeMainWindow()
void ChangeWindow(int index)
void ChangeThisWindow()
void SetInteractable(bool interactable)
void SelectWindow()
void SelectWindow(EventSystem eventSystem)
int GetThisWindowIndex()
```

# Note
調整中です.

# License

"WindowStateManagement" is under [MIT license](https://en.wikipedia.org/wiki/MIT_License).
