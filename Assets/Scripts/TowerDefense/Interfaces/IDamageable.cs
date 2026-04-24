using TowerDefense.Abilities;

namespace TowerDefense.Interfaces
{
    public interface IDamageable : ITargetable
    {
        void TakeDamage(float amount, IDamageSource source);
        void ApplyStatus(StatusEffectSO effect, IDamageSource source, int stacks = 1);
    }
}