using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using UnityEngine.SceneManagement;

public class NameManager : MonoBehaviour
{
    public TMP_InputField playerNameInputField;

    public void InputName()
    {
        if(playerNameInputField.text == ""|| playerNameInputField.text.Length >= 16)
        {
            return;
        }
        else
        {
            SetOnlineName();
            PlayerPrefs.SetString("PlayerName", playerNameInputField.text);
        }
    }
    public void SetOnlineName()
    {
        if (!PlayerScoreManager.SDKConnected) { return; }
        LootLockerSDKManager.SetPlayerName(playerNameInputField.text, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully set player name");
            }
            else
            {
                Debug.Log("Could not set player name " + response.Error);
            }
        });
        SceneManager.LoadScene(1);
    }
}
