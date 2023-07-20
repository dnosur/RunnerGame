using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeJumpBehaviour : MonoBehaviour, IUpgrade
{

    
    public void UpgradePlayer()
    {
        Debug.Log("Upgraded Jump player");
    }

    public void DisableUpgrade()
    {
        Debug.Log("Disable Jump Upgrade player");
    }
}
