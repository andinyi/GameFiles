using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ServerUI : MonoBehaviour
{
    [SerializeField]
    private Button startHost;

    [SerializeField]
    private Button startClient;

    [SerializeField]
    private TextMeshProUGUI playerText;

    private void Awake() {
        Cursor.visible = true;
    }

    private void Update() {
        //playerText.text = $"Players connected: {PlayersManager.Instance.PlayersInGame}";
    }

    private void Start() {

        startHost.onClick.AddListener(() => {
            if(NetworkManager.Singleton.StartHost()) {
                Debug.Log("Host started");
            } else {
                Debug.Log("Host failed to start");
            }
        });

        startClient.onClick.AddListener(() => {
            if(NetworkManager.Singleton.StartClient()) {
                Debug.Log("Client started");
            } else {
                Debug.Log("Host failed to start");
            }
        });
    }
}
