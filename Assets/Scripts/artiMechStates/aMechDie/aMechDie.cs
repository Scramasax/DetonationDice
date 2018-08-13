/// Artimech
/// 
/// Copyright © <2017> <George A Lancaster>
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
/// and associated documentation files (the "Software"), to deal in the Software without restriction, 
/// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
/// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
/// is furnished to do so, subject to the following conditions:
/// The above copyright notice and this permission notice shall be included in all copies 
/// or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
/// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS 
/// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
/// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
/// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
/// OTHER DEALINGS IN THE SOFTWARE.

using UnityEngine;
using System.Collections;

namespace Artimech
{
    public class aMechDie : stateMachineBase
    {
        [Header("Die Vars:")]
        [SerializeField]
        [Tooltip("Time it takes for the die to consider it resting.")]
        float m_RestTimeLimit = 0.1f;
        [SerializeField]
        [Tooltip("Fall velocity threshold to consider a die to start to try to rest.")]
        float m_FallVelocityThreshold = 0.01f;

        [SerializeField]
        [Tooltip("Grid movement distance for X/Z movement.")]
        Vector2 m_MoveDistVect;
        [SerializeField]
        [Tooltip("Move curve by distance. (X) of the curve is distance.")]
        AnimationCurve m_MoveCurveByDistance;
        [SerializeField]
        [Tooltip("Rotation angle via MoveDistVect magnitude. (In degrees.)")]
        float m_RotateAngle = 90.0f;

        [SerializeField]
        [Tooltip("Time it takes for the die to consider it falling after a move.")]
        float m_FallTimeLimit = 0.05f;

        [SerializeField]
        [Tooltip("Distance to snap to grid.")]
        float m_SnapDist = 0.05f;

        [SerializeField]
        [Tooltip("Collision triggers offset from die faces.")]
        GameObject[] m_DieFaceTriggerObjs;

        Vector3 m_MoveVector;
        Quaternion m_RotateTo;
        bool m_MoveBool = false;

        #region Accessors

        public float RestTimeLimit
        {
            get
            {
                return m_RestTimeLimit;
            }

            set
            {
                m_RestTimeLimit = value;
            }
        }

        public float FallVelocityThreshold
        {
            get
            {
                return m_FallVelocityThreshold;
            }

            set
            {
                m_FallVelocityThreshold = value;
            }
        }

        public Vector2 MoveDistVect
        {
            get
            {
                return m_MoveDistVect;
            }

            set
            {
                m_MoveDistVect = value;
            }
        }

        public AnimationCurve MoveCurveByDistance
        {
            get
            {
                return m_MoveCurveByDistance;
            }

            set
            {
                m_MoveCurveByDistance = value;
            }
        }

        public float FallTimeLimit
        {
            get
            {
                return m_FallTimeLimit;
            }

            set
            {
                m_FallTimeLimit = value;
            }
        }

        public GameObject[] DieFaceTriggerObjs
        {
            get
            {
                return m_DieFaceTriggerObjs;
            }

            set
            {
                m_DieFaceTriggerObjs = value;
            }
        }

        public Vector3 MoveVector
        {
            get
            {
                return m_MoveVector;
            }

            set
            {
                m_MoveVector = value;
            }
        }

        public bool MoveBool
        {
            get
            {
                return m_MoveBool;
            }

            set
            {
                m_MoveBool = value;
            }
        }

        public float SnapDist
        {
            get
            {
                return m_SnapDist;
            }
        }

        public float RotateAngle
        {
            get
            {
                return m_RotateAngle;
            }

            set
            {
                m_RotateAngle = value;
            }
        }

        public Quaternion RotateTo
        {
            get
            {
                return m_RotateTo;
            }

            set
            {
                m_RotateTo = value;
            }
        }

        #endregion

        new void Awake()
        {
            base.Awake();
            CreateStates();
        }

        // Use this for initialization
        new void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update();
        }

        new void FixedUpdate()
        {
            base.FixedUpdate();
        }

        /// <summary>
        /// Autogenerated state are created here inside this function.
        /// </summary>
        void CreateStates()
        {

            m_CurrentState = AddState(new diespawnPointStart(this.gameObject), "diespawnPointStart");

            //<ArtiMechStates>
            AddState(new dieMoveOnSurface(this.gameObject),"dieMoveOnSurface");
            AddState(new dieMoveInAir(this.gameObject),"dieMoveInAir");
            AddState(new dieOnSurface(this.gameObject), "dieOnSurface");
            AddState(new dieFall(this.gameObject), "dieFall");

        }
    }
}