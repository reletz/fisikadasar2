using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public static ActorManager instance;

    public bool isHidden;

    [SerializeField] Actor actorPrefab;

    private List<Actor> actors = new List<Actor>();
    private Actor selectedActor;

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    public void NewCharacter(string instantiatedName)
    {
        if (instance != this) {instance.NewCharacter(instantiatedName); return;}
        
        Actor newActor = Instantiate(actorPrefab, transform);
        newActor.actorName = instantiatedName;
        actors.Add(newActor);
    }

    public void RemoveCharacter()
    {
        if (instance != this) {instance.RemoveCharacter(); return;}

        actors.Remove(selectedActor);
        Destroy(selectedActor);
        selectedActor = null;
    }

    public void Clean()
    {
        if (instance != this) {instance.RemoveCharacter(); return;}

        actors.ForEach(actor => {
            Destroy(actor);
        });
        actors.Clear();
        selectedActor = null;
    }

    public void SelectCharacter(string actorName)
    {
        if (instance != this) {instance.SelectCharacter(actorName); return;}

        selectedActor = actors.Find(actor => { return actor.actorName == actorName; });
    }

    public void DeselectCharacter()
    {
        if (instance != this) {instance.DeselectCharacter(); return;}

        selectedActor = null;
    }

    public void PositionCharacterX(float newPosition)
    {
        if (instance != this) {instance.PositionCharacterX(newPosition); return;}

        selectedActor.transform.position = new Vector3(newPosition, selectedActor.transform.position.y, 0);
    }

    public void PositionCharacterY(float newPosition)
    {
        if (instance != this) {instance.PositionCharacterY(newPosition); return;}

        selectedActor.transform.position = new Vector3(selectedActor.transform.position.x, newPosition, 0);
    }

    public void ScaleCharacterX(float newScale)
    {
        if (instance != this) {instance.ScaleCharacterX(newScale); return;}

        selectedActor.transform.localScale = new Vector3(newScale, selectedActor.transform.localScale.y, 1);
    }

    public void ScaleCharacterY(float newScale)
    {
        if (instance != this) {instance.ScaleCharacterY(newScale); return;}

        selectedActor.transform.localScale = new Vector3(selectedActor.transform.localScale.x, newScale, 1);
    }

    public void ChangeCharacterSprite(Sprite newSprite)
    {
        if (instance != this) {instance.ChangeCharacterSprite(newSprite); return;}

        selectedActor.ChangeSprite(newSprite);
    }

    public void ChangeCharacterEyebrow(Sprite newSprite)
    {
        if (instance != this) {instance.ChangeCharacterEyebrow(newSprite); return;}

        selectedActor.ChangeEyebrow(newSprite);
    }

    public void ChangeCharacterEye(Sprite newSprite)
    {
        if (instance != this) {instance.ChangeCharacterEye(newSprite); return;}

        selectedActor.ChangeEye(newSprite);
    }

    public void ChangeCharacterMouth(Sprite newSprite)
    {
        if (instance != this) {instance.ChangeCharacterMouth(newSprite); return;}

        selectedActor.ChangeMouth(newSprite);
    }

    public void ChangeCharacterExpression(Sprite newSprite)
    {
        if (instance != this) {instance.ChangeCharacterExpression(newSprite); return;}

        selectedActor.ChangeExpression(newSprite);
    }

    void Awake()
    {
        Initialize();
    }
}
