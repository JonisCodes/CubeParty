using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.UI
{
    public class TowerContainerUI : MonoBehaviour
    {
        [SerializeField] private Image towerIcon;
        [SerializeField] private TMP_Text towerName;
        [SerializeField] private TMP_Text towerDescription;
        [SerializeField] private TMP_Text towerCost;
    }
}