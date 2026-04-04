

using System.Collections.Generic;
using TurnFlow.Core.Actions;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Effects;
using TurnFlow.Core.Triggers;

namespace TurnFlow.Core.Infos;

public  class Info : IInfo
{

    protected ITrigger? fromTrigger;
    protected IEffect? fromEffect;
    protected IAction? fromAction;
    protected ICharacter? fromCharacter;

    protected Dictionary<string, int> details;

    public Info(ITrigger? fromTrigger, IEffect? fromEffect, IAction? fromAction, ICharacter? fromCharacter)
    {
        this.fromTrigger = fromTrigger;
        this.fromEffect = fromEffect;
        this.fromAction = fromAction;
        this.fromCharacter = fromCharacter;

        details = new Dictionary<string, int>();
    }

    public Info(IInfo info)
    {
        this.fromTrigger = info.FromTrigger();
        this.fromEffect = info.FromEffect();
        this.fromAction = info.FromAction();
        this.fromCharacter = info.FromCharacter();

        details = new Dictionary<string, int>();
    }

    public ITrigger? FromTrigger()
    {
        return fromTrigger;
    }

    public IEffect? FromEffect()
    {
        return fromEffect;
    }

    public IAction? FromAction()
    {
        return fromAction;
    }

    public ICharacter? FromCharacter()
    {
        return fromCharacter;
    }

    public int GetDetails(string detailName)
    {
        if (details.ContainsKey(detailName))
        {
            return details[detailName];
        }
        else
        {
            return 0;
        }
    }
}