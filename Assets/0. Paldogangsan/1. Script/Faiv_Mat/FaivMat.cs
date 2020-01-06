using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

namespace FaivMat
{
    [RequireComponent(typeof(FaivData))]
    public class FaivMat : Singleton<FaivMat>
    {
        private const int m_Row = 84;
        private const int m_Col = 53;
        private const string sendText = "F0";

        int millisecondsTimeout = 100;

        int count = 0;                                  // 포트를 통해 데이터를 수신받았을 때 이용하는 변수
        bool isOpened;

        [SerializeField] [Range(0.01f, 0.5f)]
        float waitTime = 0.1f;                          // 보내는 주기 설정

        [SerializeField]
        private bool m_SaveDecodeData;

        SerialPort m_SerialPort;                        // 시리얼포트
        FaivData m_FaivData;

        Thread writeThread;

        public int Row { get { return m_Row; } }
        public int Col { get { return m_Col; } }

        public FaivData Data { get { return m_FaivData; } }

        private void Initialize()
        {
            m_SerialPort = new SerialPort
            {
                PortName = "COM3",
                BaudRate = 9600,
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = Parity.None
            };
        }

        /// <summary>
        /// 유니티에서 SerialPort.DataReceive 이벤트 핸들러를 사용할 수 없는 현상이 발견
        /// </summary>
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Debug.Log("Receive");
            
            byte[] array = new byte[1024];
            int temp = m_SerialPort.Read(array, 0, 1024);

            for(int i = 0; i < temp; i++)
            {
                m_FaivData.TempList.Add(array[i]);

                if(array[i] == 255)
                {
                    /// 받는 데이터가 255(0xFF) 일 경우에 count를 1씩 증가
                    /// FAIV MAT로부터 데이터를 수신받을 때,
                    /// 마지막 2개의 데이터가 FF FF  
                    if (++count > 1)
                    {
                        // 디코드함수 실행
                        m_FaivData.matDecode();

                        count = 0;
                        
                        // 확인용
                        writeThread = new Thread(new ThreadStart(m_FaivData.matDecodeWriteText));
                        writeThread.Start();

                        // 임시공간 리스트를 정리해준다.
                        m_FaivData.TempList.Clear();
                    }
                }
            }
        }

        private void ErrorReceive(object sender, SerialErrorReceivedEventArgs e)
        {
            Debug.Log("ErrorReceive :: " + e.ToString());
        }

        private void DataReceived()
        {
            byte[] array = new byte[1024];
            int temp = m_SerialPort.Read(array, 0, 1024);

            for (int i = 0; i < temp; i++)
            {
                m_FaivData.TempList.Add(array[i]);

                if (array[i] == 255)
                {
                    /// 받는 데이터가 255(0xFF) 일 경우에 count를 1씩 증가
                    /// FAIV MAT로부터 데이터를 수신받을 때,
                    /// 마지막 2개의 데이터가 FF FF  
                    if (++count > 1)
                    {
                        // 디코드함수 실행
                        m_FaivData.matDecode();

                        count = 0;

                        // 확인용
                        if (m_SaveDecodeData)
                        {
                            writeThread = new Thread(new ThreadStart(m_FaivData.matDecodeWriteText));
                            writeThread.Start();
                        }

                        // 임시공간 리스트를 정리해준다.
                        m_FaivData.TempList.Clear();
                    }
                }
            }
        }

        private IEnumerator SendCoroutine()
        {
            WaitForSeconds waitForseconds = new WaitForSeconds(waitTime);

            while (true)
            {
                m_SerialPort.Write(sendText);

                DataReceived();

                yield return waitForseconds;
            }
        }

        private void SendThreadUpdate()
        {
            string strData = string.Empty;

            while (true)
            {
                m_SerialPort.Write(sendText);
                
                // 쓰레드에 텀을 주기
                Thread.Sleep(millisecondsTimeout);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Initialize();

            m_FaivData = GetComponent<FaivData>();
        }

        private void Start()
        {
            try
            {
                if (GameManager.instance.GetUsingOfFaivMat)
                {
                    m_SerialPort.Open();

                    if (!isOpened) isOpened = true;

#if UNITY_EDITOR
                    Debug.Log("연결 완료");
#endif
                    StartCoroutine(SendCoroutine());
                }
                else
                {
                    this.enabled = false;
                }
            }
            catch
            {
                if (isOpened)
                    isOpened = false;

                Debug.Log("포트가 열리지 않음");
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (isOpened)
            {
                isOpened = false;
                m_SerialPort.Close();
            }
        }
    }
}