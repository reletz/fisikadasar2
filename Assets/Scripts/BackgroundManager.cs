using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;

    [SerializeField] SpriteRenderer backgroundSprite;

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    public void SwitchBackground(Sprite newSprite)
    {
        if (instance != this) {instance.SwitchBackground(newSprite); return;}

        backgroundSprite.sprite = newSprite;
    }

    public void ChangeBackgroundScaleX(float newScale)
    {
        if (instance != this) {instance.ChangeBackgroundScaleX(newScale); return;}

        gameObject.transform.localScale = new Vector3(newScale, transform.localScale.y, 1);
    }

    public void ChangeBackgroundScaleY(float newScale)
    {
        if (instance != this) {instance.ChangeBackgroundScaleY(newScale); return;}

        gameObject.transform.localScale = new Vector3(transform.localScale.x, newScale, 1);
    }

    void Awake()
    {
        Initialize();
    }
}
