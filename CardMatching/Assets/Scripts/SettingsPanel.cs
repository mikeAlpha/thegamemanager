using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : ModalUIWindow<SettingsPanel>
{
    public Button OnRestartButton, OnMenuButton;

    public override ModalUIWindowBase Show()
    {
        OnRestartButton.onClick.RemoveAllListeners();
        OnRestartButton.onClick.AddListener(() =>
        {
            gameManager.TriggerRestartGame();
            Close();
        });

        OnMenuButton.onClick.AddListener(() =>
        {
            EventHandler.ExecuteEvent<ModalUIWindowtype, bool>(GameStaticEvents.OnTogglePanel, ModalUIWindowtype.Gameplay, false);
            EventHandler.ExecuteEvent<ModalUIWindowtype, bool>(GameStaticEvents.OnTogglePanel, ModalUIWindowtype.Menu, true);
            Close();
        });

        return base.Show();
    }
}
