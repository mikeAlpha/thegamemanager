using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ModalUIWindow<T> : ModalUIWindowBase where T : ModalUIWindow<T>
{
    [SerializeField]
    protected Button closeButton;

    protected GameManager gameManager;

    public virtual void Awake()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() =>
            {
                Close();
            });
        }
    }

    public override ModalUIWindowBase Show()
    {
        gameObject.SetActive(true);
        return (T)this;
    }

    public override ModalUIWindowBase Close()
    {
        gameObject.SetActive(false);
        return (T)this;
    }

    public override void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }
}