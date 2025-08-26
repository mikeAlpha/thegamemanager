using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public List<ModalUIWindowBase> modalUIWindows;

    private void OnEnable()
    {
        EventHandler.RegisterEvent(GameStaticEvents.OnStartGame, StartGame);
        EventHandler.RegisterEvent<GameManager>(GameStaticEvents.OnGameManagerDataUpdate, Init);
        EventHandler.RegisterEvent<ModalUIWindowtype,bool>(GameStaticEvents.OnTogglePanel, TogglePanel);
    }

    private void OnDisable()
    {
        EventHandler.RegisterEvent(GameStaticEvents.OnStartGame, StartGame);
        EventHandler.UnregisterEvent<GameManager>(GameStaticEvents.OnGameManagerDataUpdate, Init);
        EventHandler.RegisterEvent<ModalUIWindowtype,bool>(GameStaticEvents.OnTogglePanel, TogglePanel);
    }

    void Init(GameManager manager)
    {
        foreach (var window in modalUIWindows) 
        { 
            window.SetGameManager(manager);
        }

        var menu = modalUIWindows.FirstOrDefault(x => x.modalUIWindowtype == ModalUIWindowtype.Menu);
        if (menu != null) menu.Show();
    }

    void StartGame()
    {
        var menu = modalUIWindows.FirstOrDefault(x => x.modalUIWindowtype == ModalUIWindowtype.Menu);
        if (menu != null) menu.Close();

        var gameplay = modalUIWindows.FirstOrDefault(x => x.modalUIWindowtype == ModalUIWindowtype.Gameplay);
        if (gameplay != null) gameplay.Show();
    }

    void TogglePanel(ModalUIWindowtype panelType, bool val)
    {
        var panel = modalUIWindows.FirstOrDefault(x => x.modalUIWindowtype == panelType);
        if (panel != null)
        {
            if(val) panel.Show();
            else panel.Close();
        }
    }
}
