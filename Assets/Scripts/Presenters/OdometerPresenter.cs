using System.Globalization;
using System.Timers;
using Models;
using Services.Interfaces;
using UnityEngine;
using Views.Interfaces;
using UnityWebSocket;

namespace Presenters
{
    public class OdometerPresenter
    {
        private readonly IOdometerView _odometerView;
        private readonly WebSocket _webSocket;
        private readonly Timer _timer;
        private bool _serverIsActive;
        private bool _gotCurrentOdometer;
        private string _value;
        
        public OdometerPresenter(IOdometerView view, IWebSocketService webSocketService)
        {
            _timer= new Timer(10000);
            _odometerView = view;
            _webSocket = webSocketService.GetWebSocket();
            _webSocket.OnOpen += WebSocketOnOpen;
            _webSocket.OnMessage += WebSocketOnMessage;
            _webSocket.OnClose += WebSocketOnClose;
            _timer.Elapsed+= TimerOnElapsed;
            _webSocket.ConnectAsync();
            _timer.Start();
        }

        private void WebSocketOnClose(object sender, CloseEventArgs e)
        {
            _serverIsActive = false;
            Debug.Log("Подключение прервано. Ошибка: " + e.Reason);
            _odometerView.ChangeServerStatus(_serverIsActive);
            _webSocket.ConnectAsync();
        }

        private void WebSocketOnOpen(object sender, OpenEventArgs e)
        {
            _serverIsActive = true;
            _odometerView.ChangeServerStatus(_serverIsActive);
            SendCurrentOdometer();
            SendRandomStatus();
        }

        private void WebSocketOnMessage(object sender, MessageEventArgs e)
        {
            var response = JsonUtility.FromJson<Response>(e.Data);
            Debug.Log(response.operation);
            if (!string.IsNullOrEmpty(response.odometer))
            {
                _odometerView.OdometerNumber = response.odometer;
            }
            if (!string.IsNullOrEmpty(response.value))
            {
                float value = float.Parse(response.value, CultureInfo.InvariantCulture);
                _odometerView.AnimateNumberTransition(value);
                _odometerView.UpdateArrowPosition(value);
            } 
            if (!string.IsNullOrEmpty(response.status))
            {
                _odometerView.IsRandom = bool.Parse(response.status);
            }
            
            if (_gotCurrentOdometer)
            {
                SendRandomStatus();
            }
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            SendOdometerValue(_value);
            _timer.Start();
        }

        private void SendOdometerValue(string odometerValue)
        {
            _webSocket.SendAsync("{\"operation\":\"odometer_val\",\"value\":" + odometerValue + "}");
        }

        private void SendCurrentOdometer()
        {
            _webSocket.SendAsync("{\"operation\":\"getCurrentOdometer\"}");
            _gotCurrentOdometer = true;
        }

        private void SendRandomStatus()
        {
            _webSocket.SendAsync("{\"operation\":\"getRandomStatus\"}");
            _gotCurrentOdometer = false;
        }
    }
}