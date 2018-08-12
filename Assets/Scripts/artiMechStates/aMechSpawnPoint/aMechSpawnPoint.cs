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
    public class aMechSpawnPoint : stateMachineBase
    {
        [Header("Spawn Point:")]
        [SerializeField]
        [Tooltip("Gameobject to be spawned.")]
        GameObject m_SpawnObject;

        bool m_Spawn = false;

        public bool Spawn
        {
            get
            {
                return m_Spawn;
            }

            set
            {
                m_Spawn = value;
            }
        }

        public GameObject SpawnObject
        {
            get
            {
                return m_SpawnObject;
            }

            set
            {
                m_SpawnObject = value;
            }
        }

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

            m_CurrentState = AddState(new spawnPointStart(this.gameObject), "spawnPointStart");

            //<ArtiMechStates>
            AddState(new spawnPointSpawn(this.gameObject),"spawnPointSpawn");
            AddState(new spawnPointUpdate(this.gameObject),"spawnPointUpdate");

        }
    }
}