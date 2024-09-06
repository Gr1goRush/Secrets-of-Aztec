using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SuperGameCard : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Animator _animator;
    [SerializeField] private Button _button;

    [SerializeField] private Vector2 defaultSize, expandedSize;
    [SerializeField] private float expandSpeed = 1f;

    public event GameAction OnSelectAnimationShowedEvent;

    public void SetAnimatorController(AnimatorOverrideController animatorOverrideController)
    {
        _animator.runtimeAnimatorController = animatorOverrideController;
    }

    public void AddClickListener(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }

    public void SetInteractable(bool interactable)
    {
        _button.interactable = interactable;
    }

    public void SetDefault()
    {
        rectTransform.sizeDelta = defaultSize;
        _animator.SetBool("Show", false);
        _animator.Play("Default");

    }

    public void ShowSelectAnimation()
    {
        StartCoroutine(ExpandAnimation());
    }

    private IEnumerator ExpandAnimation()
    {
        Vector2 currentSize = defaultSize;
        rectTransform.sizeDelta = currentSize;

        while (Vector2.Distance(currentSize, expandedSize) >= (expandSpeed * Time.deltaTime))
        {
            yield return new WaitForEndOfFrame();

            currentSize += Vector2.one * expandSpeed * Time.deltaTime;
            rectTransform.sizeDelta = currentSize;
        }

        yield return new WaitForEndOfFrame();
        rectTransform.sizeDelta = expandedSize;

        _animator.SetBool("Show", true);

        this.OnAnimation(_animator, "Show", OnShowAnimationFinished, 0.99f);
    }

    public void OnShowAnimationFinished()
    {
        OnSelectAnimationShowedEvent?.Invoke();
    }
}
