using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkLauncher : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;

    private const string HostAddress = "127.0.0.1";
    private const ushort HostPort = 7777;

    public void StartHost()
    {
        NetworkManager manager = NetworkManager.Singleton;
        if (manager.IsListening)
        {
            return;
        }
        UnityTransport transport = manager.GetComponent<UnityTransport>();
        // (접속 주소, 포트, 서버 수신 주소) - 루프백이라 다른 컴퓨터 접속은 받지 않음
        transport.SetConnectionData(HostAddress, HostPort, HostAddress);
        bool started = manager.StartHost();
        statusText.text = started ? "Host 시작" : "Host 시작 실패";
    }

    public void StartClient()
    {
        NetworkManager manager = NetworkManager.Singleton;
        if (manager.IsListening)
        {
            return;
        }
        UnityTransport transport = manager.GetComponent<UnityTransport>();
        transport.SetConnectionData(HostAddress, HostPort);
        // true는 시작 성공이지 접속 완료가 아님 (완료 여부는 Update에서 확인)
        bool started = manager.StartClient();
        statusText.text = started ? "Client 연결 시도" : "Client 시작 실패";
    }

    private void Update()
    {
        NetworkManager manager = NetworkManager.Singleton;
        if (manager == null)
        {
            return;
        }
        if (manager.IsHost)
        {
            statusText.text = $"Host 실행 / 참여자 {manager.ConnectedClientsIds.Count}명";
        }
        else if (manager.IsConnectedClient)
        {
            statusText.text = "Client 연결 완료";
        }
    }
}