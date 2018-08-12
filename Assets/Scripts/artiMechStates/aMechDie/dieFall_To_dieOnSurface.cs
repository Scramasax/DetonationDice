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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// State Conditionals are created to contain the state transition tests. 
/// </summary>
namespace Artimech
{
    public class dieFall_To_dieOnSurface : stateConditionalBase
    {
        float m_FallTime = 0;
        public dieFall_To_dieOnSurface(string changeStateName) : base(changeStateName)
        {

        }

        public override void Enter(baseState state)
        {
            m_FallTime = 0;
        }

        public override void Exit(baseState state)
        {

        }

        /// <summary>
        /// Test conditionals are placed here.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>true or false depending if transition conditions are met.</returns>
        public override string UpdateConditionalTest(baseState state)
        {
            string strOut = null;

            aMechDie theScript = state.m_GameObject.GetComponent<aMechDie>();
            if (theScript.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < theScript.FallVelocityThreshold)
            {
                m_FallTime += gameMgr.GetSeconds();
            }

            if (theScript.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < theScript.FallVelocityThreshold && m_FallTime > theScript.FallTimeLimit)
            {
                strOut = m_ChangeStateName;
            }

            //utlDebugPrint.Inst.print(theScript.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude.ToString());

            return strOut;
        }
    }
}
