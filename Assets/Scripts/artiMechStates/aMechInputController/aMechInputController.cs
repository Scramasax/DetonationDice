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
    public class aMechInputController : stateMachineBase
    {
        [Header("Movement Debug Variables:")]
        [SerializeField]
        [Tooltip("Show the goal of the controller.")]
        bool m_ShowMoveToDebugPos = false;
        [SerializeField]
        [Tooltip("Color of the move to goal for debug.")]
        Color m_MoveToDebugColor = Color.blue;

        [Header("Input Vars:")]
        [SerializeField]
        [Tooltip("Drag distance threshold.")]
        float m_DragThresh = 7.0f;

        Ray m_SelectorRay;
        Vector3 m_StartTouchPos;
        aMechDie m_SelectedDie;
        Vector2 m_StartScreenPos;

        private static aMechInputController m_Instance = null;
        /// <summary>Returns an instance of SimMgr </summary>
        public static aMechInputController Inst { get { return m_Instance; } }

        public Vector3 StartTouchPos
        {
            get
            {
                return m_StartTouchPos;
            }

            set
            {
                m_StartTouchPos = value;
            }
        }

        public float DragThresh
        {
            get
            {
                return m_DragThresh;
            }
        }

        public aMechDie SelectedDie
        {
            get
            {
                return m_SelectedDie;
            }

            set
            {
                m_SelectedDie = value;
            }
        }

        public Vector2 StartScreenPos
        {
            get
            {
                return m_StartScreenPos;
            }

            set
            {
                m_StartScreenPos = value;
            }
        }

        new void Awake()
        {
            base.Awake();
            CreateStates();
            m_Instance = GetComponent<aMechInputController>();
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
        /// Returns a screen space position.
        /// </summary>
        /// <returns></returns>
        public Vector2 TouchPosScreenSpace()
        {
            Vector2 inputPos = new Vector2();
            bool touchBool = false;
            bool mouseClickBool = false;
            int i = 0;
            int index = 0;
            while (i < Input.touchCount)
            {
                if ((Input.GetTouch(i).phase != TouchPhase.Ended || Input.GetTouch(i).phase != TouchPhase.Canceled))
                {
                    touchBool = true;
                    index = i;
                }
                i++;
            }

            mouseClickBool = Input.GetMouseButton(0);

            if (touchBool)
                inputPos = Input.GetTouch(index).position;
            if (mouseClickBool)
                inputPos = Input.mousePosition;
            return inputPos;
        }

        /// <summary>
        /// Get the position of a touch or the mouse.
        /// </summary>
        /// <returns></returns>
        public Vector3 TouchPosWorldSpace()
        {
            Vector3 vectOut = new Vector3();
            Vector2 inputPos = new Vector2();
            bool touchBool = false;
            bool mouseClickBool = false;
            int i = 0;
            int index = 0;
            while (i < Input.touchCount)
            {
                if ((Input.GetTouch(i).phase != TouchPhase.Ended || Input.GetTouch(i).phase != TouchPhase.Canceled))
                {
                    touchBool = true;
                    index = i;
                }
                i++;
            }

            mouseClickBool = Input.GetMouseButton(0);

            if (touchBool)
                inputPos = Input.GetTouch(index).position;
            if (mouseClickBool)
                inputPos = Input.mousePosition;

            for (int j = 0; j < Camera.allCameras.Length; j++)
                vectOut = Camera.allCameras[j].ScreenToWorldPoint(inputPos);

            return vectOut;
        }

        /// <summary>
        /// Touch Screen says if you touching the screen.
        /// </summary>
        /// <returns></returns>
        public bool TouchScreen()
        {
            bool touchBool = false;
            bool mouseClickBool = false;
            int i = 0;
            int index = 0;
            while (i < Input.touchCount)
            {
                if ((Input.GetTouch(i).phase != TouchPhase.Ended || Input.GetTouch(i).phase != TouchPhase.Canceled))
                {
                    touchBool = true;
                    index = i;
                }
                i++;
            }

            // mouseClickBool = Input.GetMouseButtonDown(0);
            mouseClickBool = Input.GetMouseButton(0);

            Vector2 inputPos = new Vector2(0, 0);
            if (touchBool)
                inputPos = Input.GetTouch(index).position;
            if (mouseClickBool)
                inputPos = Input.mousePosition;

            if (!touchBool && !mouseClickBool)
                return false;

            for (int j = 0; j < Camera.allCameras.Length; j++)
                m_SelectorRay = Camera.allCameras[j].ScreenPointToRay(inputPos);

            return true;
        }

        /// <summary>
        /// Is the selector ray intersecting with the closest die.
        /// </summary>
        /// <returns></returns>
        public aMechDie GetTouchDie()
        {
            int index = -1;
            float compDist = float.MaxValue;
            for (int j = 0; j < Camera.allCameras.Length; j++)
            {
                //m_SelectorRay = Camera.allCameras[j].ScreenPointToRay(inputPos);
                for (int diceIndex = 0; diceIndex < SimMgr.Inst.DiceList.Count; diceIndex++)
                {
                    Bounds bounds = SimMgr.Inst.DiceList[diceIndex].gameObject.GetComponent<Collider>().bounds;
                    if (bounds.IntersectRay(m_SelectorRay))
                    {
                        float dist = Vector3.Distance(SimMgr.Inst.DiceList[diceIndex].transform.position, m_SelectorRay.GetPoint(0.0f));
                        if (dist < compDist)
                        {
                            index = diceIndex;
                            compDist = dist;
                        }

                    }
                }
            }

            if (index != -1)
                return SimMgr.Inst.DiceList[index];

            return null;
        }



        /// <summary>
        /// Autogenerated state are created here inside this function.
        /// </summary>
        void CreateStates()
        {

            m_CurrentState = AddState(new inputControllerStart(this.gameObject), "inputControllerStart");

            //<ArtiMechStates>
            AddState(new inputControllerCalcDragMove(this.gameObject), "inputControllerCalcDragMove");
            AddState(new inputControllerTouchDie(this.gameObject), "inputControllerTouchDie");
            AddState(new inputControllerTouchScreen(this.gameObject), "inputControllerTouchScreen");
            AddState(new inputControllerGameUpdate(this.gameObject), "inputControllerGameUpdate");

        }
    }
}