// based on https://gist.github.com/amimaro/10e879ccb54b2cacae4b81abea455b10

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace TOM.Common.Communication
{
    public class UnityHttpListener : MonoBehaviour
    {
        private HttpListener listener;
        private Thread listenerThread;

        private Dictionary<string, string> data = new Dictionary<string, string>();
        private bool dataReceived = false;

        // Start is called before the first frame update
        void Start()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5050/");
            listener.Prefixes.Add("http://127.0.0.1:5050/");
            //listener.Prefixes.Add("http://*:5050");
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            listener.Start();

            listenerThread = new Thread(startListener);
            listenerThread.Start();
            Debug.Log("Server Started");
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void startListener()
        {
            while (true)
            {
                var result = listener.BeginGetContext(ListenerCallback, listener);
                result.AsyncWaitHandle.WaitOne();
            }
        }

        private void ListenerCallback(IAsyncResult result)
        {
            var context = listener.EndGetContext(result);

            Debug.Log("Method: " + context.Request.HttpMethod);
            Debug.Log("LocalUrl: " + context.Request.Url.LocalPath);

            data.Clear();

            if (context.Request.QueryString.AllKeys.Length > 0)
            {
                foreach (var key in context.Request.QueryString.AllKeys)
                {
                    var value = context.Request.QueryString.GetValues(key)[0];
                    Debug.Log("Key: " + key + ", Value: " + value);

                    data.Add(key, value);
                }
            }


            if (context.Request.HttpMethod == "POST")
            {
                Thread.Sleep(1000);
                var data_text = new StreamReader(context.Request.InputStream,
                    context.Request.ContentEncoding).ReadToEnd();
                Debug.Log(data_text);
            }

            dataReceived = true;

            context.Response.Close();
        }

        /**
         * return null if not found
         */
        public string GetData(string key)
        {
            if (key == null || !data.ContainsKey(key))
            {
                return null;
            }

            return data[key];
        }

        public List<string> GetAllKeys()
        {
            return new List<string>(data.Keys);
        }

        /**
         * return true if data received.
         */
        public bool DataReceived()
        {
            if (dataReceived)
            {
                dataReceived = false;
                return true;
            }

            return false;
        }

    }
}
