using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ModalUIWindow<T> : MonoBehaviour where T : ModalUIWindow<T>
{
    [SerializeField]
    protected Button closeButton;

    public virtual void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            Close();
        });
    }

    public virtual T Show()
    {
        gameObject.SetActive(true);
        return (T)this;
    }

    public virtual T Close()
    {
        gameObject.SetActive(false);
        return (T)this;
    }
}