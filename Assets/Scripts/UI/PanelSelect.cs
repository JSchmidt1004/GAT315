using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSelect : MonoBehaviour
{
    [System.Serializable]
    public struct PanelInfo
    {
        public GameObject panel;
        public Button button;
        public KeyCode keyCode;
        public Action.eActionType actionType;
    }

    public KeyCode toggleKey;
    public GameObject masterPanel;
    public PanelInfo[] panelInfos;
    public Editor editor;

    void Start()
    {
        foreach (PanelInfo panelInfo in panelInfos)
        {
            panelInfo.button.onClick.AddListener(delegate { ButtonEvent(panelInfo); });
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            masterPanel.SetActive(!masterPanel.activeSelf);
        }

        foreach (PanelInfo panelInfo in panelInfos)
        {
            if (Input.GetKeyDown(panelInfo.keyCode))
            {
                SetPanelActive(panelInfo);
            }
        }
    }

    void SetPanelActive(PanelInfo panelInfo)
    {
        for (int i = 0; i < panelInfos.Length; i++)
        {
            bool active = panelInfos[i].Equals(panelInfo);
            panelInfos[i].panel.SetActive(active);

            if (active)
            {
                editor.SetAction(panelInfos[i].actionType);
            }
        }
    }

    void ButtonEvent(PanelInfo panelInfo)
    {
        SetPanelActive(panelInfo);
    }
}
