using System;
using TowerDefense.Interfaces;

namespace TowerDefense.Abilities.Execution.Modifiers
{
    public class DoubleShotModifier : IAbilityExecutionModifier
    {
        public void ModifyExecution(AbilityInstance ability, AbilityExecutionContext context, Action execute)
        {
            execute();
            execute();
        }
    }
}