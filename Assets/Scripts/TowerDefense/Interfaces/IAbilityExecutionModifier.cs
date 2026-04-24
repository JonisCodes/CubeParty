using System;
using TowerDefense.Abilities;

namespace TowerDefense.Interfaces
{
    public interface IAbilityExecutionModifier
    {
        void ModifyExecution(AbilityInstance ability, AbilityExecutionContext context, Action execute);
    }
}