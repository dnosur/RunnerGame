using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyUpgrade : MonoBehaviour
{
    private float upgradeDuration;

    [SerializeField] private float jumpUpgradeDuration;
    [SerializeField] private float speedUpgradeDuration;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IUpgrade upgrade))
        {
            upgradeDuration = ChangeUpgradeDuration(other);

            StartCoroutine(GetBonus(upgrade, other));

        }
    }

    private IEnumerator GetBonus(IUpgrade upgrade, Collider other)
    {
        Destroy(other.gameObject);
        upgrade.UpgradePlayer();

        yield return new WaitForSeconds(upgradeDuration);

        upgrade.DisableUpgrade();
    }

    private float ChangeUpgradeDuration(Collider other)
    {

        if (other.gameObject.GetComponent<UpgradeJumpBehaviour>())
        {
            return jumpUpgradeDuration;
        }
        else if (other.gameObject.GetComponent<UpgradeSpeedBehaviour>())
        {
            return speedUpgradeDuration;
        }

        return 1;

    }
}
