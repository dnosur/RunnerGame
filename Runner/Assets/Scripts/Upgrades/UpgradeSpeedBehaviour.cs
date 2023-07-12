using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSpeedBehaviour : MonoBehaviour, IUpgrade
{

    
    public void UpgradePlayer()
    {
        Debug.Log("Upgraded Speed player");
    }

    public void DisableUpgrade()
    {
        Debug.Log("Disable Speed Upgrade player");
    }

}
