using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class PlayerScoreManager : MonoBehaviour
{
    public static bool SDKConnected;
    private void Start()
    {
        StartCoroutine(LoginRoutine());
    }

    private IEnumerator LoginRoutine()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                SDKConnected = true;
            }
            else
            {
                Debug.Log("Could not start session");
                SDKConnected = true;
            }
        });
        yield return new WaitWhile(() => SDKConnected == false);
    }
}
