using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace FaivMat
{
    public delegate int MatAction();

    [DisallowMultipleComponent]
    public class FaivData : MonoBehaviour
    {
        List<byte> m_temp;

        [HideInInspector] byte[] m_Src;
        [HideInInspector] byte[] m_Dst;

        FaivMat m_FaivMat;

        public MatAction matDecode;

        public Action matDecodeWriteText;

        private int inputCount = 0;

        private string InfoPath;

        public List<byte> TempList { get { return m_temp; } }

        public byte[] dst { get { return m_Dst; } }

        /// <summary>
        /// 디코드함수
        /// </summary>
        private int MatDecodeBCSD()
        {
            int i = 0, j = 0, k = 0, cnt = 0;

            // Src에 값을 할당해준다.
            for (int n = 0; n < m_temp.Count; n++)
                m_Src[n] = m_temp[n];

            try
            {
                for(i = 0; i < m_temp.Count; i++)
                {
                    if(m_Src[i] == 0)
                    {
                        for (k = 0; k < m_Src[i + 1]; k++)
                        {
                            m_Dst[j++] = 0;
                            cnt++;
                        }
                        i++;
                    }
                    else if (m_Src[i] == 255)
                    {
                        for (k = 0; k < m_Src[i + 1]; k++)
                        {
                            m_Dst[j++] = 255;
                            cnt++;
                        }
                        i++;
                    }
                    else if (m_Src[i] != 0 && m_Src[i] != 255)
                    {
                        m_Dst[j] = m_Src[i];
                        j++;
                        cnt++;
                    }
                }
            }
            catch (Exception ex) {
                Debug.LogError("MatDecodeBCSD 문제발생 : " + ex.ToString());
            }

            return cnt;
        }

        /// <summary>
        /// 디코드정보를 텍스트파일로 확인하기 위한 함수
        /// </summary>
        private void WriteTextFile()
        {
            // append default = false
            using (StreamWriter outputFile = new StreamWriter(InfoPath + @"\test.txt", true))
            {
                int count = 0;
                inputCount++;

                outputFile.WriteLine("Input Test " + inputCount);
                foreach (byte data in m_Dst)
                {
                    count++;
                    outputFile.Write("   " + data);

                    if (count % 53 == 0)
                        outputFile.WriteLine();

                    if (count == m_FaivMat.Row * m_FaivMat.Col)
                        break;
                }

                outputFile.WriteLine("총 데이터 갯수 : " + count);
            }

            Debug.Log("Write 완료");
        }

        private void Awake()
        {
            m_FaivMat = GetComponent<FaivMat>();

            m_Src = new byte[m_FaivMat.Row * m_FaivMat.Col + 300];
            m_Dst = new byte[m_FaivMat.Row * m_FaivMat.Col + 300];
            m_temp = new List<byte>();

            matDecode = MatDecodeBCSD;

            matDecodeWriteText = WriteTextFile;

            InfoPath = Application.dataPath + @"\Data";

            DirectoryInfo directoryInfo = new DirectoryInfo(InfoPath);

            if (!directoryInfo.Exists)
                directoryInfo.Create();
            
            using (StreamWriter sw = new StreamWriter(InfoPath + @"\test.txt"))
            {
                sw.WriteLine("입력된 정보");
            }
        }
    }
}