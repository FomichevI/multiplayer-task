using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MenuUiManager : MonoBehaviour
{
    
    public void OnHostButtonClick()
    {
        GameManager.Instance.LoadGameplayScene(ConnectionType.Host);
    }

    public void OnClientButtonClick()
    {
        GameManager.Instance.LoadGameplayScene(ConnectionType.Client);
    }

}
