using UnityEngine;
using UnityEngine.Networking;

using NativeWebSocket;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public class SocketCommunication : MonoBehaviour
{
    public ConfigLoader configLoader = null;

    public bool AutoConnect = true;

    private string accessToken = "";
    private WebSocket websocket = null;

    private bool connectionAttempted = false;
    private string errorStatus = null;

    // FIXME: make these audo-discard after a time limit
    private const int MAX_TX_QUEUE_SIZE = 5;
    private const int DISMISS_VISUAL_LOG_DELAY_SECONDS = 10;

    private ConcurrentQueue<string> txStringMessages = new ConcurrentQueue<string>();
    private ConcurrentQueue<byte[]> txByteMessages = new ConcurrentQueue<byte[]>();
    private ConcurrentQueue<byte[]> rxMessages = new ConcurrentQueue<byte[]>();


    private string GetWebSocketUrl()
    {        
        return "ws://" + configLoader.GetHost() + ":" + configLoader.GetPort();
    }

    private async Task ConnectWSAsync()
    {
        if (!configLoader.IsLoaded())
        {
            await configLoader.UpdateConfigInfo();
        }
        
        var url = GetWebSocketUrl();
        VisualLog.Log(url);

        // add custom header to differentiate client type
        Dictionary<string, string> customHeader = new Dictionary<string, string>();
        customHeader["websocket_client_type"] = "unity";

        websocket = new WebSocket(url, customHeader);

        // Setup WS Handlers. 
        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
            VisualLog.Log("Server Connected");

            Invoke(nameof(DismissVisualLog), DISMISS_VISUAL_LOG_DELAY_SECONDS);
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
            VisualLog.Log("Socket Error: " + e);
            errorStatus = e;
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
            VisualLog.Log("Socket Closed");
        };

        websocket.OnMessage += (bytes) =>
        {
            Debug.Log($"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()} received websocket data");
            ProcessMessage(bytes);
        };

        // Connect websocket. 
        await websocket.Connect();
    }

    private void DismissVisualLog()
    {
        VisualLog.DismissLog();
    }

    private IEnumerator LoginUser()
    {
        // accessToken = reply.data.access_token;
        if (AutoConnect)
        {
            VisualLog.Log("LoginUser");
            yield return ConnectWSAsync();
        }
    }

    private async void OnApplicationQuit()
    {
        // Cleanup websocket if open.
        if (websocket != null)
        {
            await websocket.Close();
        }

        txStringMessages.Clear();
        txByteMessages.Clear();
        rxMessages.Clear();

        websocket = null;
    }


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(LoginUser());
    }

    // Update is called once per frame
    async void Update()
    {
        if (AutoConnect && !connectionAttempted && configLoader != null)
        {
            connectionAttempted = true;
            StartCoroutine(LoginUser());
        }

        if (errorStatus != null)
        {
            ProcessErrorStatus(errorStatus);
            errorStatus = null;
        }

        if (websocket != null && connectionAttempted)
        {
            websocket.DispatchMessageQueue();

            string txDataString;
            if (websocket.State == WebSocketState.Open && !txStringMessages.IsEmpty && txStringMessages.TryDequeue(out txDataString))
            {            
                await websocket.SendText(txDataString);
                Debug.Log("Sent data: " + txDataString);
                //VisualLog.Log("Sent data: " + txDataString);
                VisualLog.Log("");
            }

            byte[] txDataBytes;
            if (websocket.State == WebSocketState.Open && !txByteMessages.IsEmpty && txByteMessages.TryDequeue(out txDataBytes))
            {
                await websocket.Send(txDataBytes);
                Debug.Log($"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()} sent websocket data");
            }
        }
    }

    void OnDestroy()
    {
        StopExchange();
    }

    private void StopExchange()
    {
        OnApplicationQuit();
    }

    private void ProcessErrorStatus(string status)
    {
        Debug.Log("Error:" + errorStatus);

        //// restart after disconnection?
        //if (AutoConnect && connectionAttempted)
        //{
        //    StopExchange();
        //    connectionAttempted = false;
        //}
    }

    private void ProcessMessage(byte[] bytes)
    {
        rxMessages.Enqueue(bytes);
    }

    /**
	 * return true if data received.
	 */
    public bool DataReceived()
    {
        return !rxMessages.IsEmpty;
    }

    // send a shallow copy of elements
    public List<byte[]> GetMessages()
    {
        List<byte[]> results = new List<byte[]>(rxMessages.ToArray());
        rxMessages.Clear();

        return results;
    }

    public void SendMessages(string message)
    {
        if (txStringMessages.Count >= MAX_TX_QUEUE_SIZE)
        {
            string discard;
            txStringMessages.TryDequeue(out discard);
            Debug.LogError("Discarded data: " + discard);
        }
        txStringMessages.Enqueue(message);
    }

    public void SendMessages(byte[] message)
    {
        if (txByteMessages.Count >= MAX_TX_QUEUE_SIZE)
        {
            byte[] discard;
            txByteMessages.TryDequeue(out discard);
            Debug.LogError("Discarded data: " + System.Text.Encoding.UTF8.GetString(discard));
        }
        txByteMessages.Enqueue(message);
    }

}
