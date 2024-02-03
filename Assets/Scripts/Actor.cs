using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public string actorName;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer eyebrowRenderer;
    [SerializeField] private SpriteRenderer eyeRenderer;
    [SerializeField] private SpriteRenderer mouthRenderer;
    [SerializeField] private SpriteRenderer expressionRenderer;

    public void ChangeSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }

    public void ChangeEyebrow(Sprite newExpression)
    {
        eyebrowRenderer.sprite = newExpression;
    }

    public void ChangeEye(Sprite newExpression)
    {
        eyeRenderer.sprite = newExpression;
    }

    public void ChangeMouth(Sprite newExpression)
    {
        mouthRenderer.sprite = newExpression;
    }

    public void ChangeExpression(Sprite newExpression)
    {
        expressionRenderer.sprite = newExpression;
    }
}
