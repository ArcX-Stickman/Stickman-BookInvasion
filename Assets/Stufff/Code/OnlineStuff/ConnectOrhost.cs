using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectOrhost : MonoBehaviour
{


    public Button hostButton;
    public Button connectButton;



    private void Awake()
    {
        hostButton.onClick.AddListener(() => {
            Debug.Log("Host button clicked");
            NetworkManager.Singleton.StartHost();
            Hide();
        });
        connectButton.onClick.AddListener(() => {
            Debug.Log("Host button clicked");
            NetworkManager.Singleton.StartClient();
            Hide();
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

   
}
