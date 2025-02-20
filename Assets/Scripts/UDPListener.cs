﻿using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class UDPListener : MonoBehaviour
{
  private UdpClient client;

  [SerializeField]
  private int portIn = 5555;
  [SerializeField]
  private bool printToConsole = false;

  [HideInInspector]
  public string Message { get; set; }

  private static Thread receiveThread;

  private static void Quit()
  {
    Debug.Log("Aborting UDP thread.");
    receiveThread.Abort( );
  }

  [RuntimeInitializeOnLoadMethod]
  private static void RunOnStart()
  {
    Application.quitting += Quit;
  }

  public void Start()
  {
    Message = "";

    receiveThread = new Thread(new ThreadStart(() =>
    {
      client = new UdpClient(portIn);
      while (true)
      {
        try
        {
          IPEndPoint ip = new IPEndPoint(IPAddress.Any, 0);
          byte[] data = client.Receive(ref ip);

          Message = Encoding.UTF8.GetString(data);

          if (printToConsole)
            Debug.Log(Message);
        }
        catch (Exception e)
        {
          print(e.ToString());
        }
      }
    }))
    {
      IsBackground = true
    };

    receiveThread.Start();
  }
}