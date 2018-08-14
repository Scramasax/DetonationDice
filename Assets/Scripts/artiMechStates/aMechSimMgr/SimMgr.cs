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
using System.Collections.Generic;
using UnityEngine.UI;

namespace Artimech
{
    public class SimMgr : stateMachineBase
    {
        [Header("SimMgr:")]

        [SerializeField]
        [Tooltip("Base points to be score.  Order of mag as match connect goes up.")]
        int m_BaseScore = 10;
        [SerializeField]
        [Tooltip("Min Random Time.")]
        float m_SpawnMinRndTime = 3.0f;
        [SerializeField]
        [Tooltip("Max Random Time.")]
        float m_SpawnMaxRndTime = 6.0f;
        [SerializeField]
        [Tooltip("Speed Up over time.")]
        float m_SpawSpeedUpOverTime = 0.01f;
        [SerializeField]
        [Tooltip("Speed Up over time.")]
        float m_SpawnTimeMinCap = 1.0f;
        [SerializeField]
        [Tooltip("Game level time in seconds.")]
        float m_GameLevelTimeMax = 150.0f;
        [SerializeField]
        [Tooltip("Y Height Lose. If a die gets over a certain height you loose.")]
        float m_YHeightLose = 5.0f;

        [SerializeField]
        [Tooltip("Toggle For push and roll.")]
        Toggle m_Toggle;
        [SerializeField]
        [Tooltip("The text for the score.")]
        Text m_ScoreText;
        [SerializeField]
        [Tooltip("Soundtrack.")]
        AudioSource m_SoundTrack;
        [SerializeField]
        [Tooltip("Game Over Text.")]
        Text m_GameOverText;
        [SerializeField]
        [Tooltip("You Win Text.")]
        Text m_YouWinText;
        [SerializeField]
        [Tooltip("Next Level to load if you win.")]
        string m_NextLevelWin;
        [SerializeField]
        [Tooltip("Next Level to load if you loose.")]
        string m_NextLevelLose;

        private static SimMgr m_Instance = null;
        /// <summary>Returns an instance of SimMgr </summary>
        public static SimMgr Inst { get { return m_Instance; } }

        private IList<aMechDie> m_DiceList;
        private IList<aMechSpawnPoint> m_SpawnPointList;
        private IList<aMechGridPoint> m_GridPointList;

        private IList<aMechDie> m_DiceMatchBufferList;

        private int m_TotalScore;
        private float m_MinusSpawnTime;
        private float m_GameLevelTime;
        bool m_GameWin;
        bool m_GameLose;

        #region Accessors
        public IList<aMechSpawnPoint> SpawnPointList
        {
            get
            {
                return m_SpawnPointList;
            }

            set
            {
                m_SpawnPointList = value;
            }
        }

        public IList<aMechDie> DiceList
        {
            get
            {
                return m_DiceList;
            }

            set
            {
                m_DiceList = value;
            }
        }

        public IList<aMechGridPoint> GridPointList
        {
            get
            {
                return m_GridPointList;
            }

            set
            {
                m_GridPointList = value;
            }
        }

        public Toggle Toggle
        {
            get
            {
                return m_Toggle;
            }
        }

        public int TotalScore
        {
            get
            {
                return m_TotalScore;
            }

            set
            {
                m_TotalScore = value;
            }
        }

        public bool GameWin
        {
            get
            {
                return m_GameWin;
            }

            set
            {
                m_GameWin = value;
            }
        }

        public bool GameLose
        {
            get
            {
                return m_GameLose;
            }

            set
            {
                m_GameLose = value;
            }
        }

        public Text GameOverText
        {
            get
            {
                return m_GameOverText;
            }

            set
            {
                m_GameOverText = value;
            }
        }

        public Text YouWinText
        {
            get
            {
                return m_YouWinText;
            }

            set
            {
                m_YouWinText = value;
            }
        }

        public float GameLevelTimeMax
        {
            get
            {
                return m_GameLevelTimeMax;
            }
        }

        public string NextLevelWin
        {
            get
            {
                return m_NextLevelWin;
            }

            set
            {
                m_NextLevelWin = value;
            }
        }

        public string NextLevelLose
        {
            get
            {
                return m_NextLevelLose;
            }

            set
            {
                m_NextLevelLose = value;
            }
        }

        #endregion

        public float GetRandomSpawnTimeLimit()
        {
            float fltTemp = 0;
            float outMin = m_SpawnMinRndTime - m_MinusSpawnTime;
            float outMax = m_SpawnMaxRndTime - m_MinusSpawnTime;
            fltTemp = Random.Range(outMax, outMax);

            //           utlDebugPrint.Inst.print("outMin " + outMin.ToString());
            //          utlDebugPrint.Inst.print("outMax " + outMax.ToString());
            //          utlDebugPrint.Inst.print("m_MinusSpawnTime " + m_MinusSpawnTime.ToString());

            fltTemp = Mathf.Clamp(fltTemp, m_SpawnTimeMinCap, m_SpawnMaxRndTime);
            return fltTemp;
        }

        public void SpawnDieAtRandomSpawnPositions()
        {
            int rndIndex = Random.Range(0, m_SpawnPointList.Count - 1);
            m_SpawnPointList[rndIndex].Spawn = true;
        }

        public Vector3 GetClosestGridPoint(Vector3 pos)
        {
            Vector3 outVect = new Vector3();
            int index = -1;
            float distance = float.MaxValue;
            for (int i = 0; i < GridPointList.Count; i++)
            {
                float dist = Vector3.Distance(pos, GridPointList[i].transform.position);
                if (dist < distance)
                {
                    index = i;
                    distance = dist;
                }
            }

            if (index != -1)
                return GridPointList[index].transform.position;

            return outVect;
        }

        public bool IsDieRestAboveHeightLimit()
        {
            for (int i = 0; i < DiceList.Count; i++)
            {
                if (DiceList[i].transform.position.y > m_YHeightLose && DiceList[i].IsDieOnSurface())
                    return true;
            }
            return false;
        }

        new void Awake()
        {
            if (m_Instance != null)
            {
                Debug.LogWarning("There was already an instance of SimMgr.");
                return;
            }

            base.Awake();
            CreateStates();

            m_SpawnPointList = new List<aMechSpawnPoint>();
            DiceList = new List<aMechDie>();
            GridPointList = new List<aMechGridPoint>();
            m_DiceMatchBufferList = new List<aMechDie>();

            m_Instance = GetComponent<SimMgr>();
        }

        // Use this for initialization
        new void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        new void Update()
        {
            m_MinusSpawnTime += gameMgr.GetSeconds() * m_SpawSpeedUpOverTime;
            m_GameLevelTime += gameMgr.GetSeconds();

            //check for winning.
            if (m_GameLevelTime > GameLevelTimeMax)
                m_GameWin = true;

            //check for losing.
            m_GameLose = IsDieRestAboveHeightLimit();

            if (!m_GameWin && !m_GameLose)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (m_Toggle.isOn)
                        m_Toggle.isOn = false;
                    else
                        m_Toggle.isOn = true;
                }

                //UpdateDieMatch();
            }
            base.Update();


        }

        new void FixedUpdate()
        {
            base.FixedUpdate();
        }

        void UpdateDieMatch()
        {
            //int dieMatchCount = 0;

            for (int i = 0; i < DiceList.Count; i++)
            {
                if (DiceList[i].DeathBool)
                    continue;

                m_DiceMatchBufferList.Clear();
                for (int j = 0; j < DiceList[i].ContactDiceList.Count; j++)
                {
                    m_DiceMatchBufferList.Add(DiceList[i]);
                    for (int index = 0; index < DiceList[i].ContactDiceList[j].ContactDiceList.Count; index++)
                    {
                        aMechDie die = DiceList[i].ContactDiceList[j].ContactDiceList[index];
                        if (!IsDieAlreadyInBufferList(die))
                        {
                            m_DiceMatchBufferList.Add(die);
                        }
                    }
                }
                if (m_DiceMatchBufferList.Count >= 3)
                {
                    TotalScore += m_DiceMatchBufferList.Count * m_DiceMatchBufferList.Count * m_BaseScore;
                    m_ScoreText.text = "Score:" + TotalScore;
                    SetBufferedDiceToDeath();
                }
            }
        }

        void SetBufferedDiceToDeath()
        {
            for (int i = 0; i < m_DiceMatchBufferList.Count; i++)
            {
                m_DiceMatchBufferList[i].DeathBool = true;
            }
        }

        bool IsDieAlreadyInBufferList(aMechDie die)
        {
            for (int i = 0; i < m_DiceMatchBufferList.Count; i++)
            {
                if (m_DiceMatchBufferList[i] == die)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Autogenerated state are created here inside this function.
        /// </summary>
        void CreateStates()
        {

            m_CurrentState = AddState(new simMgrStart(this.gameObject), "simMgrStart");

            //<ArtiMechStates>
            AddState(new simMgrGameWinEnd(this.gameObject), "simMgrGameWinEnd");
            AddState(new simMgrGameWinStart(this.gameObject), "simMgrGameWinStart");
            AddState(new simMgrTriggerSpawn(this.gameObject), "simMgrTriggerSpawn");
            AddState(new simMgrStartGame(this.gameObject), "simMgrStartGame");
            AddState(new simMgrGameOverEnd(this.gameObject), "simMgrGameOverEnd");
            AddState(new simMgrGameOverStart(this.gameObject), "simMgrGameOverStart");
            AddState(new simMgrUpdate(this.gameObject), "simMgrUpdate");

        }
    }
}