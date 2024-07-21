using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{	
	public const float AnimationTime = 0.5f;

	protected virtual Vector2 EndPos => new (AttachedCanvasRT.sizeDelta.x, 0);
	protected virtual Vector2 StartPos => new (AttachedCanvasRT.sizeDelta.x, 0);
	protected virtual Vector2 PosInside => new (0, 0);
	
	// InstantCallback
	public UnityEvent OnShowEvent { get; } = new ();
	public UnityEvent OnHideEvent { get; } = new ();
	// End InstantCallback
	
	// Components
	private CanvasGroup cachedCanvasGroup;
	protected CanvasGroup AttachedCanvasGroup 
	{
		get
		{
			cachedCanvasGroup ??= GetComponent<CanvasGroup>();
			return cachedCanvasGroup;
		}
	}
	
	private RectTransform cachedRectTransform;
	protected RectTransform AttachedRectTransform 
	{
		get
		{
			cachedRectTransform ??= transform as RectTransform;
			return cachedRectTransform;
		}
	}

	private RectTransform cachedCanvasRT;
	protected RectTransform AttachedCanvasRT
	{
		get
		{
			cachedCanvasRT ??= GetComponentInParent<Canvas>().transform as RectTransform;
			return cachedCanvasRT;
		}
	}
	// End Components
	
	public bool UIPanelState {get; protected set;}
	public bool IsInAnimation {get; protected set;}
	
	public void Show(bool notifyPanel = true, bool playAnimation = true)
	{
		UIPanelState = true;
		
		AttachedCanvasGroup.Show();
		
		OnShowEvent.Invoke();
		
		if(notifyPanel)
			OnShow();
			
		ShowPanel(playAnimation);
	}
	
	public void Hide(bool notifyPanel = true, bool playAnimation = true)
	{
		UIPanelState = false;
		
		OnHideEvent.Invoke();
		
		HidePanel(notifyPanel, playAnimation);
	}
	
	protected virtual void ShowPanel(bool playAnimation, Action animationCallback = null)
	{
		if(playAnimation)
		{
			IsInAnimation = true;
			AttachedRectTransform.anchoredPosition = StartPos;
			AttachedRectTransform.DOAnchorPos(PosInside, AnimationTime).OnComplete(() => 
			{
				IsInAnimation = false;	
				animationCallback?.Invoke();
			});			
		}
		else
			AttachedRectTransform.anchoredPosition = PosInside;
	}
	
	protected virtual void HidePanel(bool notifyPanel, bool playAnimation)
	{
		if(!playAnimation)
		{
			AttachedRectTransform.anchoredPosition = EndPos;
			HideCanvas(notifyPanel);
			return;
		}
		
		IsInAnimation = true;
		AttachedRectTransform.DOAnchorPos(EndPos, AnimationTime).OnComplete(() => 
		{
			HideCanvas(notifyPanel);
			IsInAnimation = false;
		});
	}
	
	protected void HideCanvas(bool notifyPanel)
	{
		AttachedCanvasGroup.Hide();
					
		if(notifyPanel)
			OnHide();
	}
	
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) && UIPanelState && !IsInAnimation)
		{
			OnEscapeClick();
		}
	}
	
	protected virtual void OnEscapeClick(){}
	protected virtual void OnHide(){}
	protected virtual void OnShow(){}
	
	#if UNITY_EDITOR
	[ContextMenu("Show Panel")]
	public void EditorShowPanel()
	{
		CanvasGroup cg = GetComponent<CanvasGroup>();
		cg.Show();
		EditorUtility.SetDirty(cg);
	}
	
	[ContextMenu("Hide Panel")]
	public void EditorHidePanel()
	{
		CanvasGroup cg = GetComponent<CanvasGroup>();
		cg.Hide();
		EditorUtility.SetDirty(cg);
	}
	#endif
}