/*  Copyright (C) 2011 Przemyslaw Szeptycki <pszeptycki@gmail.com>, Ecole Centrale de Lyon,

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClCamera.cs
*   @brief      We can add to rendering process this object. It represents camera in rendering
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       26-10-2007
*
*   @history
*   @item		26-10-2007 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace ModelPreProcessing
{
    public class ClCamera : ClBaseRenderObject
    {
        //------------------------- Attributes -----------------------
        private float m_fCamPositionX = 0;
        private float m_fCamPositionY = 0;
        private float m_fCamPositionZ = 400;

        private float m_fCamLookAtX = 0;
        private float m_fCamLookAtY = 0;
        private float m_fCamLookAtZ = 0;

        private float m_fCamUpVectorX = 0;
        private float m_fCamUpVectorY = 1;
        private float m_fCamUpVectorZ = 0;

        private float m_fRadius = 400;


        CustomVertex.PositionColored[] m_CenterPoint = new CustomVertex.PositionColored[1];
        //------------------------- Methods ---------------------------
        public ClCamera():base("Camera")
        {
        }

        public override void Render(Device p_dDevice, Control p_cRenderWindow)
        {            
            p_dDevice.Transform.Projection = Matrix.PerspectiveFovLH(   (float)Math.PI / 4,                                 // Camera Fov
                                                                        p_cRenderWindow.Width / p_cRenderWindow.Height,     // Camera Width and Height
                                                                        1f,                                                 // Camera first cut point
                                                                        5000f);                                             // Camera secound cut point

            p_dDevice.Transform.View = Matrix.LookAtLH( new Vector3(m_fCamPositionX, m_fCamPositionY, m_fCamPositionZ),     // Camera position
                                                        new Vector3(m_fCamLookAtX, m_fCamLookAtY, m_fCamLookAtZ),           // Camera look at
                                                        new Vector3(m_fCamUpVectorX, m_fCamUpVectorY, m_fCamUpVectorZ));    // Camera normal vector
            p_dDevice.RenderState.Lighting = false;

            m_CenterPoint[0] = new CustomVertex.PositionColored(m_fCamLookAtX, m_fCamLookAtY, m_fCamLookAtZ, Color.Red.ToArgb());
            p_dDevice.VertexFormat = CustomVertex.PositionColored.Format;
            p_dDevice.DrawUserPrimitives(PrimitiveType.PointList, 1, m_CenterPoint);
        }


        public void SetCameraPosition(float p_fPositionX, float p_fPositionY, float p_fPositionZ)
        {
          /*  m_fRadius = (float)Math.Sqrt(Math.Pow(GetCameraPositionX() - GetCameraLookAtX(), 2) +
                                                    Math.Pow(GetCameraPositionY() - GetCameraLookAtY(), 2) +
                                                    Math.Pow(GetCameraPositionZ() - GetCameraLookAtZ(), 2));
            */
            m_fCamPositionX = p_fPositionX;
            m_fCamPositionY = p_fPositionY;
            m_fCamPositionZ = p_fPositionZ;

            //ClRender.getInstance().ChangeLightPosition(GetCameraPositionX(), GetCameraPositionY(), GetCameraPositionZ());
            //ClRender.getInstance().ChangeLightDirection(GetCameraLookAtX(), GetCameraLookAtY(), GetCameraLookAtZ());
        }

        public void SetCameraLookAt(float p_fLookAtX, float p_fLookAtY, float p_fLookAtZ)
        {
            m_fRadius = (float)Math.Sqrt(Math.Pow(GetCameraPositionX() - GetCameraLookAtX(), 2) +
                                        Math.Pow(GetCameraPositionY() - GetCameraLookAtY(), 2) +
                                        Math.Pow(GetCameraPositionZ() - GetCameraLookAtZ(), 2));

            m_fCamLookAtX = p_fLookAtX;
            m_fCamLookAtY = p_fLookAtY;
            m_fCamLookAtZ = p_fLookAtZ;

            //ClRender.getInstance().ChangeLightPosition(GetCameraPositionX(), GetCameraPositionY(), GetCameraPositionZ());
            //ClRender.getInstance().ChangeLightDirection(GetCameraLookAtX(), GetCameraLookAtY(), GetCameraLookAtZ());
        }

        public void MoveCameraLookAt(float p_fLookAtX, float p_fLookAtY, float p_fLookAtZ)
        {
            float diffX = m_fCamLookAtX - (p_fLookAtX);
            float diffY = m_fCamLookAtY - (p_fLookAtY);
            float diffZ = m_fCamLookAtZ - (p_fLookAtZ);

            m_fCamPositionX -= diffX;
            m_fCamPositionY -= diffY;
            m_fCamPositionZ -= diffZ;

            m_fCamLookAtX = p_fLookAtX;
            m_fCamLookAtY = p_fLookAtY;
            m_fCamLookAtZ = p_fLookAtZ;
            

            //ClRender.getInstance().ChangeLightPosition(GetCameraPositionX(), GetCameraPositionY(), GetCameraPositionZ());
            //ClRender.getInstance().ChangeLightDirection(GetCameraLookAtX(), GetCameraLookAtY(), GetCameraLookAtZ());
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            // Left button  -- Move Camera look at --
            if (m_bLeftMouseButtonDown && !m_bRightMouseButtonDown)
            {
                MoveCameraLookAt(GetCameraLookAtX() + (m_iMouseButtonDownXpos - e.X) * (m_fCamUpVectorY),
                                  GetCameraLookAtY() + (m_iMouseButtonDownYpos - e.Y) * (m_fCamUpVectorY),
                                  GetCameraLookAtZ());

                m_iMouseButtonDownXpos = e.X;
                m_iMouseButtonDownYpos = e.Y;

               /* string information = "LookAt" +
                                    "\nCamera Position" +
                                    "\n x " + GetCameraPositionX().ToString() +
                                    "\n y " + GetCameraPositionY().ToString() +
                                    "\n z " + GetCameraPositionZ().ToString() +
                                    "\nCamera LookAt" +
                                    "\n x " + GetCameraLookAtX().ToString() +
                                    "\n y " + GetCameraLookAtY().ToString() +
                                    "\n z " + GetCameraLookAtZ().ToString();

                ClInformationSender.SendInformation(information , ClInformationSender.eInformationType.eDebugText);
                */
            }
            // Right button  -- Spinning camera --
            if (m_bRightMouseButtonDown && !m_bLeftMouseButtonDown)
            {
                float z = (float)Math.Sqrt(Math.Abs(Math.Pow(m_fRadius, 2) -
                                     Math.Pow(GetCameraPositionX() + (m_iMouseButtonDownXpos - e.X) * (m_fCamUpVectorY) - GetCameraLookAtX(), 2) -
                                     Math.Pow(GetCameraPositionY() + (m_iMouseButtonDownYpos - e.Y) * (m_fCamUpVectorY) - GetCameraLookAtY(), 2))
                                    ) + GetCameraLookAtZ();


                SetCameraPosition(GetCameraPositionX() + (m_iMouseButtonDownXpos - e.X) * (m_fCamUpVectorY),
                                  GetCameraPositionY() + (m_iMouseButtonDownYpos - e.Y) * (m_fCamUpVectorY),
                                  z);

                m_iMouseButtonDownXpos = e.X;
                m_iMouseButtonDownYpos = e.Y;

                /*string information = "Spin" +
                                    "\nCamera Position" +
                                    "\n x " + GetCameraPositionX().ToString() +
                                    "\n y " + GetCameraPositionY().ToString() +
                                    "\n z " + GetCameraPositionZ().ToString() +
                                    "\nCamera LookAt" +
                                    "\n x " + GetCameraLookAtX().ToString() +
                                    "\n y " + GetCameraLookAtY().ToString() +
                                    "\n z " + GetCameraLookAtZ().ToString();

                ClInformationSender.SendInformation(information, ClInformationSender.eInformationType.eDebugText);
                */
            }
        }

        public override void MouseWheel(object sender, MouseEventArgs e)
        {
            if (!m_bLeftMouseButtonDown && !m_bRightMouseButtonDown)
            {
                SetCameraPosition(GetCameraPositionX(),
                                    GetCameraPositionY(),
                                    GetCameraPositionZ() - e.Delta / 10);

                m_fRadius = (float)Math.Sqrt(Math.Pow(GetCameraPositionX() - GetCameraLookAtX(), 2) +
                                        Math.Pow(GetCameraPositionY() - GetCameraLookAtY(), 2) +
                                        Math.Pow(GetCameraPositionZ() - GetCameraLookAtZ(), 2));
                
                /*string information = "Roll" +
                                    "\nCamera Position" +
                                    "\n x " + GetCameraPositionX().ToString() +
                                    "\n y " + GetCameraPositionY().ToString() +
                                    "\n z " + GetCameraPositionZ().ToString() +
                                    "\nCamera LookAt" +
                                    "\n x " + GetCameraLookAtX().ToString() +
                                    "\n y " + GetCameraLookAtY().ToString() +
                                    "\n z " + GetCameraLookAtZ().ToString();
                ClInformationSender.SendInformation(information, ClInformationSender.eInformationType.eDebugText);
                */
                /*MoveCameraLookAt(    GetCameraLookAtX(),
                                    GetCameraLookAtY(),
                                    GetCameraLookAtZ() - e.Delta / 6);
                */
            }
        }

        public override void MouseButtonDown(object sender, MouseEventArgs e)
        {
            base.MouseButtonDown(sender, e);

            if (m_bRightMouseButtonDown && m_bLeftMouseButtonDown)
            {
                m_fCamUpVectorY *= (-1);
                ClInformationSender.SendInformation("Cam vector is: " + m_fCamUpVectorY.ToString(), ClInformationSender.eInformationType.eDebugText);
            }
        }

        public float GetCameraPositionX()
        {
            return m_fCamPositionX;
        }

        public float GetCameraPositionY()
        {
            return m_fCamPositionY;
        }

        public float GetCameraPositionZ()
        {
            return m_fCamPositionZ;
        }

        public float GetCameraLookAtX()
        {
            return m_fCamLookAtX;
        }

        public float GetCameraLookAtY()
        {
            return m_fCamLookAtY;
        }

        public float GetCameraLookAtZ()
        {
            return m_fCamLookAtZ;
        }

        public float getCameraRadius()
        {
            return m_fRadius;
        }

    }
}
