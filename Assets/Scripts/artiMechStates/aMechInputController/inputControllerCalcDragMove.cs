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
    <alias>Calc Input Drag</alias>
    <comment></comment>
    <posX>343</posX>
    <posY>248</posY>
    <sizeX>140</sizeX>
    <sizeY>61</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class inputControllerCalcDragMove : stateGameBase
    {
        Vector2 m_EndPos;
        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public inputControllerCalcDragMove(GameObject gameobject) : base(gameobject)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new inputControllerCalcDragMove_To_inputControllerGameUpdate("inputControllerGameUpdate"));
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
            aMechInputController controller = m_GameObject.GetComponent<aMechInputController>();
            //controller.StartTouchPos
            m_EndPos = controller.TouchPosScreenSpace();
            Vector2 direction = controller.StartScreenPos - m_EndPos;
            direction = Vector3.Normalize(direction);

            float rotDeg = controller.SelectedDie.RotateAngle;
            //Move in the positive Z
            if (direction.x < 0 && direction.y < 0)
            {
                controller.SelectedDie.MoveVector = new Vector3(0, 0, -controller.SelectedDie.MoveDistVect.y);
                controller.SelectedDie.RotateTo = Quaternion.Euler(rotDeg, 0.0f, 0.0f) * controller.SelectedDie.gameObject.transform.rotation;
                //utlDebugPrint.Inst.print("A");
            }
            //Move in the positive x
            if (direction.x < 0 && direction.y > 0)
            {
                controller.SelectedDie.MoveVector = new Vector3(-controller.SelectedDie.MoveDistVect.x, 0, 0);
                controller.SelectedDie.RotateTo = Quaternion.Euler(0.0f, 0.0f, -rotDeg) * controller.SelectedDie.gameObject.transform.rotation;
                //utlDebugPrint.Inst.print("B");
            }
            //Move in the negative Z
            if (direction.x > 0 && direction.y > 0)
            {
                controller.SelectedDie.MoveVector = new Vector3(0, 0, controller.SelectedDie.MoveDistVect.y);
                controller.SelectedDie.RotateTo = Quaternion.Euler(-rotDeg, 0.0f, 0.0f) * controller.SelectedDie.gameObject.transform.rotation;
                //utlDebugPrint.Inst.print("C");
            }
            //Move in the negative x
            if (direction.x > 0 && direction.y < 0)
            {
                controller.SelectedDie.MoveVector = new Vector3(controller.SelectedDie.MoveDistVect.x, 0, 0);
                controller.SelectedDie.RotateTo = Quaternion.Euler(0.0f, 0.0f, rotDeg) * controller.SelectedDie.gameObject.transform.rotation;
                //utlDebugPrint.Inst.print("D");
            }

            controller.SelectedDie.MoveBool = true;

            /*           utlDebugPrint.Inst.print("controller.StartScreenPos = " + controller.StartScreenPos.ToString());
                       utlDebugPrint.Inst.print("m_EndPos = " + m_EndPos.ToString());
                       utlDebugPrint.Inst.print(direction.ToString());*/

            base.Enter();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            base.Exit();
        }
    }
}