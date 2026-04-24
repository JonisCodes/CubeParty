using TowerDefense.Enums;

namespace TowerDefense.Interfaces
{
    public interface IStatModifier
    {
        StatType Stat { get; }
        float Apply(float baseValue);
    }
}