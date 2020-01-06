using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaivMat
{
    [RequireComponent(typeof(FaivMat))]
    public sealed class FaivMove : InputModule
    {
        FaivMat m_FaivMat;

        [SerializeField] bool m_isPressedLeft = false;
        [SerializeField] bool m_isPressedRight = false;

        /// <summary>
        /// Mat를 가로로 놓았을 경우만을 고려
        /// 데이터정보 판별.
        /// </summary>
        private void DataDiscriminate()
        {
            int middleIdx = (int)(m_FaivMat.Col * 0.5);

            m_isPressedLeft = false;
            m_isPressedRight = false;

            /// 오른발 판별하는 반복문
            for ( int i = 0 ; i < middleIdx; i++)
            {
                for(int j = 0; j < m_FaivMat.Row; j++)
                {
                    if (!m_isPressedRight) {
                        if (m_FaivMat.Data.dst[j * 53 + i] > 0)
                            m_isPressedRight = true;
                    }
                    else
                        break;
                }

                if (m_isPressedRight)
                    break;
            }

            /// 왼발 판별하는 반복문
            for (int i = middleIdx+1; i < m_FaivMat.Col; i++)
            {
                for(int j = 0; j < m_FaivMat.Row; j++)
                {
                    if (!m_isPressedLeft)
                    {
                        if (m_FaivMat.Data.dst[j * 53 + i] > 0)
                            m_isPressedLeft = true;
                    }
                    else
                        break;
                }

                if (m_isPressedLeft)
                    break;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            m_FaivMat = GetComponent<FaivMat>();
        }

        private void Start()
        {
            if(!GameManager.instance.GetUsingOfFaivMat)
                this.enabled = false;
        }

        protected override void FixedUpdate()
        {
            DataDiscriminate();

            base.FixedUpdate();
        }

        protected override void FootKeyInput()
        {
            if (m_isPressedRight && m_isPressedLeft)
                return;

            if (m_isPressedLeft)
                _inputSystem.LeftFootBinding?.Invoke();

            if (m_isPressedRight)
                _inputSystem.RightFootBinding?.Invoke();
        }
    }
}