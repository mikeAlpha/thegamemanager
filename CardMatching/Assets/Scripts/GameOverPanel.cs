using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : ModalUIWindow<GameOverPanel>
{
    public Button OnRestartBtn, OnNextBtn;
    public TMP_Text scoreInfo;

    public override ModalUIWindowBase Show()
    {
        scoreInfo.text = $"{gameManager.score}";
        OnRestartBtn.onClick.RemoveAllListeners();
        OnRestartBtn.onClick.AddListener(() =>
        {
            gameManager.TriggerRestartGame();
            Close();
        });

        OnNextBtn.onClick.RemoveAllListeners();
        OnNextBtn.onClick.AddListener(() => {
            EventHandler.ExecuteEvent<ModalUIWindowtype, bool>(GameStaticEvents.OnTogglePanel, ModalUIWindowtype.Gameplay, false);
            gameManager.TriggerNextRound();
            Close();
        });

        if((gameManager.numOfTurns <= 0 && gameManager.matchedPairs < gameManager.totalPairs) || gameManager.currentPlayerData.last_level_id >= gameManager.levelDatas.Length - 1)
            OnNextBtn.interactable = false;

        return base.Show();
    }
}
