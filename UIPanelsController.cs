using System;
using System.Linq;
using UnityEngine;

public class UIPanelsController : Singleton<UIPanelsController>
{
	[SerializeField] private MainPanel[] mainPanels;
	[SerializeField] private MiscPanel[] miscPanels;
	[SerializeField] private UIPanel bgPanel;
	
	public UIPanel BgPanel => bgPanel;

	[Serializable]
	private struct MainPanel
	{
		public MainPanelType panelType;
		public UIPanel uiPanel;
	}
	
	[Serializable]
	private struct MiscPanel
	{
		public MiscPanelType panelType;
		public UIPanel uiPanel;
	}
	
	public bool IsOnlyThisMainPanelShown(MainPanelType mainPanelType) 
	{
		return mainPanels.First(x => x.panelType == mainPanelType).uiPanel.UIPanelState &&  // Is this panel shown
				mainPanels.Count(x => x.uiPanel.UIPanelState || x.uiPanel.IsInAnimation) == 1 && // Is only this panel shown and animated
				 miscPanels.Count(x => x.uiPanel.UIPanelState || x.uiPanel.IsInAnimation) == 0; // Is none of misc panels shown
	}
	
	private void Awake()
	{
		foreach (var miscPanel in miscPanels)
		{
			miscPanel.uiPanel.Hide(false, false);
		}
		
		foreach (var mainPanel in mainPanels)
		{
			if(mainPanel.panelType != MainPanelType.MainMenu)
				mainPanel.uiPanel.Hide(false, false);
		}
		
		mainPanels.First(x => x.panelType == MainPanelType.MainMenu).uiPanel.Show(false, false);
	}
	
	private void OnEnable()
	{
		foreach (var miscPanel in miscPanels)
		{
			miscPanel.uiPanel.OnHideEvent.AddListener(OnMiscPanelClose);
		}
	}
	
	private void OnDisable()
	{
		foreach (var miscPanel in miscPanels)
		{
			miscPanel.uiPanel.OnHideEvent.RemoveListener(OnMiscPanelClose);
		}
	}
	
	public void ShowMainPanel(MainPanelType panelType, bool notifyPanel = true, bool playAnimation = true)
	{
		mainPanels.First(x => x.panelType == panelType).uiPanel.Show(notifyPanel, playAnimation);
		
		foreach (var mainPanel in mainPanels)
		{
			if(mainPanel.panelType != panelType && mainPanel.uiPanel.UIPanelState)
				mainPanel.uiPanel.Hide(notifyPanel, playAnimation);
		}
	}
	
	public void ShowMiscPanel(MiscPanelType panelType, bool notifyPanel = true, bool playAnimation = true)
	{
		if(bgPanel != null && !bgPanel.UIPanelState)
			bgPanel.Show();
		
		miscPanels.First(x => x.panelType == panelType).uiPanel.Show(notifyPanel, playAnimation);
		
		foreach (var miscPanel in miscPanels)
		{
			if(miscPanel.panelType != panelType && miscPanel.uiPanel.UIPanelState)
				miscPanel.uiPanel.Hide(notifyPanel, playAnimation);
		}
	}
	
	private void OnMiscPanelClose()
	{
		if(miscPanels.Count(x => x.uiPanel.UIPanelState) == 0)
			bgPanel.Hide();
	}
}

public enum MainPanelType
{
	MainMenu,
	Gameplay		
}

public enum MiscPanelType
{
	Settings,
	Store
}

public static class CanvasGroupExtensions
{
	// I'm not changing "interactable" in canvas groups since it changes all of buttons to "non-interactable" state
	// which changes their appearance
	
	public static void Show(this CanvasGroup canvasGroup)
	{
		canvasGroup.alpha = 1;
		canvasGroup.blocksRaycasts = true;
	}

	public static void Hide(this CanvasGroup canvasGroup)
	{
		canvasGroup.alpha = 0;
		canvasGroup.blocksRaycasts = false;
	}
}
