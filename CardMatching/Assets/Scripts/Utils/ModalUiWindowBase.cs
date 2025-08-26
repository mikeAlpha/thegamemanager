using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModalUIWindowBase : MonoBehaviour
{
    public abstract ModalUIWindowBase Show();
    public abstract ModalUIWindowBase Close();

    public abstract void SetGameManager(GameManager manager);

    public ModalUIWindowtype modalUIWindowtype;

}

public enum ModalUIWindowtype
{
    Menu,
    LevelSelect,
    Gameplay,
    Settings,
    Gameover
}
