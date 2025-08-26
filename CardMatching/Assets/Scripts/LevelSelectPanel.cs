using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectPanel : ModalUIWindow<LevelSelectPanel>
{
    public UILevelItem LvlItemPrefab;
    public Transform Parent;

    private List<UILevelItem> items = new List<UILevelItem>();

    public override ModalUIWindowBase Show()
    {
        for(int i = 0; i<gameManager.levelDatas.Length; i++)
        {
            int level_index = i;

            var uiLvlItem = Instantiate(LvlItemPrefab, Parent);
            uiLvlItem.OnPlayBtn.onClick.AddListener(() =>
            {
                gameManager.currentPlayerData.last_level_id = level_index;
                gameManager.numOfTurns = gameManager.levelDatas[gameManager.currentPlayerData.last_level_id].levelData.numOfTurns;
                EventHandler.ExecuteEvent(GameStaticEvents.OnStartGame);
                Close();
            });



            if (level_index > gameManager.currentPlayerData.levels.Count - 1)
            {
                uiLvlItem.levelInfo.text = $"Lvl:{level_index + 1} Scr:0";
                uiLvlItem.OnPlayBtn.interactable = false;
            }
            else if(level_index <= gameManager.currentPlayerData.levels.Count - 1)
            {
                uiLvlItem.levelInfo.text = $"Lvl:{level_index + 1} Scr:{gameManager.currentPlayerData.levels[level_index].score}";
                uiLvlItem.OnPlayBtn.interactable = true;
            }

            items.Add(uiLvlItem);
        }

        return base.Show();
    }

    public override ModalUIWindowBase Close()
    {
        if (items.Count > 0)
        {
            items.ForEach(x => Destroy(x.gameObject));
            items.Clear();
        }
        return base.Close();
    }
}
