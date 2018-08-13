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

#region XML_DATA

#if ARTIMECH_META_DATA
<!-- Atrimech metadata for positioning and other info using the visual editor.  -->
<!-- The format is XML. -->
<!-- __________________________________________________________________________ -->
<!-- Note: Never make ARTIMECH_META_DATA true since this is just metadata       -->
<!-- Note: for the visual editor to work.                                       -->

<stateMetaData>
  <State>
    <alias>Move On Surface</alias>
    <comment></comment>
    <posX>205</posX>
    <posY>406</posY>
    <sizeX>141</sizeX>
    <sizeY>51</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class dieMoveOnSurface : stateGameBase
    {
        Vector3 m_MoveToPos;
        Vector3 m_BeginPos;
        float m_BeginDist;
        bool m_GoalReached;
        Quaternion m_BeginRotation;

        public bool GoalReached
        {
            get
            {
                return m_GoalReached;
            }

            set
            {
                m_GoalReached = value;
            }
        }

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public dieMoveOnSurface(GameObject gameobject) : base(gameobject)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new dieMoveOnSurface_To_dieFall("dieFall"));
        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {

            base.Update();
        }

        /// <summary>
        /// Fixed Update for physics and such from the game object.
        /// </summary>
        public override void FixedUpdate()
        {
            Debug.DrawLine(m_GameObject.transform.position, m_MoveToPos,Color.yellow);

            aMechDie theScript = m_GameObject.GetComponent<aMechDie>();
            //theScript.GetComponent<Rigidbody>().useGravity = false;
            //theScript.GetComponent<Rigidbody>().AddForce(theScript.MoveVector);
            float currentDist = utlMath.FlatDistance(m_GameObject.transform.position, m_BeginPos);
            float distToGoal = utlMath.FlatDistance(m_GameObject.transform.position, m_MoveToPos);

            if (distToGoal < theScript.SnapDist)
            {
                m_GameObject.transform.position = m_MoveToPos;
                m_GameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                theScript.gameObject.transform.rotation = theScript.RotateTo;
                GoalReached = true;
                return;
            }

            float distCoef = 0.0f;
            if (currentDist > 0.0f)
                distCoef = currentDist / m_BeginDist;

            float velocity = theScript.MoveCurveByDistance.Evaluate(distCoef);

            Vector3 dirVelocity = Vector3.Normalize(m_MoveToPos - m_GameObject.transform.position) * velocity;

            m_GameObject.GetComponent<Rigidbody>().velocity = dirVelocity;
            theScript.gameObject.transform.rotation = Quaternion.LerpUnclamped(m_BeginRotation, theScript.RotateTo, distCoef + 0.05f);

            base.FixedUpdate();
        }

        /// <summary>
        /// For updateing the unity gui.
        /// </summary>
        public override void UpdateEditorGUI()
        {
            base.UpdateEditorGUI();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            GoalReached = false;
            aMechDie theScript = m_GameObject.GetComponent<aMechDie>();
            theScript.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            theScript.GetComponent<Rigidbody>().useGravity = false;
            theScript.GetComponent<Collider>().isTrigger = true;
            m_BeginRotation = theScript.transform.rotation;
            Vector3 movePos = theScript.transform.position - theScript.MoveVector;
            //m_MoveToPos = SimMgr.Inst.GetClosestGridPoint(movePos);
            m_MoveToPos = movePos;
            m_BeginPos = theScript.transform.position;
            m_BeginDist = Vector3.Distance(m_BeginPos, m_MoveToPos);

 /*           utlDebugPrint.Inst.print(" grid list = " + SimMgr.Inst.GridPointList.Count.ToString());
            utlDebugPrint.Inst.print(" m_BeginPos = " + m_BeginPos.ToString());
            utlDebugPrint.Inst.print(" movePos = " + movePos.ToString());
            utlDebugPrint.Inst.print(" m_MoveToPos = " + m_MoveToPos.ToString());
            utlDebugPrint.Inst.print(" m_BeginDist = " + m_BeginDist.ToString());
            for(int i=0;i< SimMgr.Inst.GridPointList.Count;i++)
                utlDebugPrint.Inst.print("grid list positions = " + SimMgr.Inst.GridPointList[i].transform.position.ToString());
*/
            
            base.Enter();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            aMechDie theScript = m_GameObject.GetComponent<aMechDie>();
            theScript.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            theScript.GetComponent<Rigidbody>().useGravity = true;
            theScript.GetComponent<Collider>().isTrigger = false;
            theScript.MoveBool = false;
            base.Exit();
        }
    }
}