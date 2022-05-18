using NativeWebSocket;
using UnityEngine;

namespace Net.Samples.WebSocketExample
{
  public class Connection : MonoBehaviour
  {
    private WebSocket _websocket;

    private const string ServerAddr = "192.168.229.180:12345";
    // Start is called before the first frame update
    async void Start()
    {
      // websocket = new WebSocket("ws://echo.websocket.org");
//      _websocket = new WebSocket("ws://localhost:3000");

      _websocket = new WebSocket($"ws://{ServerAddr}");

      _websocket.OnOpen += () => { Debug.Log("Connection open!"); };

      _websocket.OnError += (e) => { Debug.LogError("Error! " + e); };

      _websocket.OnClose += (e) => { Debug.Log("Connection closed!"); };

      _websocket.OnMessage += (bytes) =>
      {
        // Reading a plain text message
        var message = System.Text.Encoding.UTF8.GetString(bytes);
        Debug.Log("Received OnMessage! (" + bytes.Length + " bytes) " + message);
      };

      // Keep sending messages at every 0.3s
      InvokeRepeating(nameof(SendWebSocketMessage), 0.0f, 0.3f);

      await _websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
      _websocket.DispatchMessageQueue();
#endif
    }

    async void SendWebSocketMessage()
    {
      if (_websocket.State == WebSocketState.Open)
      {
        // Sending bytes
        await _websocket.Send(new byte[] {10, 20, 30});

        // Sending plain text
        await _websocket.SendText("plain text message");
      }
    }

    private async void OnApplicationQuit()
    {
      await _websocket.Close();
    }
  }
}
