using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : ModalUIWindow<MenuPanel>
{
    public Button OnPlayBtn, OnLevelSelectBtn;

    public override ModalUIWindowBase Show()
    {
        OnPlayBtn.onClick.RemoveAllListeners();
        OnPlayBtn.onClick.AddListener(() =>
        {
            gameManager.score = 0;
            gameManager.numOfTurns = gameManager.levelDatas[gameManager.currentPlayerData.last_level_id].levelData.numOfTurns;
            EventHandler.ExecuteEvent(GameStaticEvents.OnStartGame);
        });

        OnLevelSelectBtn.onClick.RemoveAllListeners();
        OnLevelSelectBtn.onClick.AddListener(() =>
        {
            EventHandler.ExecuteEvent<ModalUIWindowtype,bool>(GameStaticEvents.OnTogglePanel, ModalUIWindowtype.LevelSelect,true);
        });

        return base.Show();
    }
}
