


using System.Collections.Generic;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Infos;
using TurnFlow.Core.Managers.Engines;

namespace TurnFlow.Core.Mechanics;

public static class BasicInteractionMechanics
{
    
    public static List<TriggerParams> DealDamageOrHeal(
        IInfo info
    )
    {

        List<TriggerParams> triggers = new List<TriggerParams>();

        // get amount
        int amount = info.GetDamageAmount();

        // todo
        

        return new List<TriggerParams>();
    }

}