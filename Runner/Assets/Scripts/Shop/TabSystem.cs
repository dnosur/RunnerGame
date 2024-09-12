using UnityEngine;
using UnityEngine.UI;

public class TabSystem : MonoBehaviour
{
    [SerializeField] GameObject[] contentTabs;
    [SerializeField] Button[] tabButtons;
    [SerializeField] Color activeTabColor;
    [SerializeField] Color inactiveTabColor;
    private Image[] tabButtonImages;

    private int activeTabIndex = 0;

    private void Start()
    {
        tabButtonImages = new Image[tabButtons.Length];
        for (int i = 0; i < tabButtons.Length; i++)
        {
            tabButtonImages[i] = tabButtons[i].GetComponent<Image>();
        }

        SwitchTab(0);
    }

    public void SwitchTab(int tabIndex)
    {
        contentTabs[activeTabIndex].SetActive(false);
        tabButtonImages[activeTabIndex].color = inactiveTabColor;

        contentTabs[tabIndex].SetActive(true);
        tabButtonImages[tabIndex].color = activeTabColor;

        activeTabIndex = tabIndex;
    }
}
