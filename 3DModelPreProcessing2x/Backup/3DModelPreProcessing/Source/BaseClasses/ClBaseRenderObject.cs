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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClBaseRenderObject.cs
*   @brief      Base class to reander objects, we can register in it to Eavents
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       26-10-2007
*
*   @history
*   @item		26-10-2007 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace ModelPreProcessing
{
    public abstract class ClBaseRenderObject : ClBaseEaventHandler, IRenderObject
    {
        //---------------------------- Attribites -------------------------
        private static int sm_iGlobalID = 0;
        protected string m_sObjectName = "";
        protected int m_iId = 0;

        //----------------------------- Methods ---------------------------
        public ClBaseRenderObject(string p_sObjectName)
        {
            m_sObjectName = p_sObjectName;
            m_iId = sm_iGlobalID++;
        }

        public abstract void Render(Device p_dDevice, Control p_cRenderWindow);

        public string GetObjectName()
        {
            return m_sObjectName;
        }

        public int GetObjectID()
        {
            return m_iId;
        }
    }
}
