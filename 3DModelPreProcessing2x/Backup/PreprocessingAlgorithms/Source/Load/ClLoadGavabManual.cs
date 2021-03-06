﻿/*  Copyright (C) 2011 Przemyslaw Szeptycki <pszeptycki@gmail.com>, Ecole Centrale de Lyon,

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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClLoadModelCurvaturesValues.cs
*   @brief      ClLoadModelCurvaturesValues
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       08-01-2009
*
*   @history
*   @item		08-01-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClLoadGavabManual : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClLoadGavabManual();
        }

        public static string ALGORITHM_NAME = @"Load\Load GAVAB manual";

        public ClLoadGavabManual() : base(ALGORITHM_NAME) { }


        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string name = p_Model.ModelFileName;
           
            string FileName = p_Model.ModelFileFolder + name + ".gav";

            string line = "";
            List<ClTools.MainPoint3D> points = new List<ClTools.MainPoint3D>();
            using (StreamReader FileStream = File.OpenText(FileName))
            {
                while ((line = FileStream.ReadLine()) != null)
                {
                   
                    string[] coordinates = line.Split('\t');
                    if (coordinates[0].Contains("@"))
                        continue;

                    if (coordinates.Length != 4)
                        throw new Exception("Incorrect format, less than 3 coordinates for a landmark (landmekName x y z)");


                    points.Add(new ClTools.MainPoint3D(Single.Parse(coordinates[1].Split(' ')[1], System.Globalization.CultureInfo.InvariantCulture),
                                        Single.Parse(coordinates[2].Split(' ')[1], System.Globalization.CultureInfo.InvariantCulture),
                                        Single.Parse(coordinates[3].Split(' ')[1], System.Globalization.CultureInfo.InvariantCulture),
                                        coordinates[0]));
                }
            }
            
            
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (!iter.IsValid())
                throw new Exception("Iterator in the model is not valid!");

            do
            {
                foreach (ClTools.MainPoint3D pts in points)
                    pts.CheckClosest(iter);

            } while (iter.MoveToNext());

            foreach (ClTools.MainPoint3D pts in points)
                p_Model.AddSpecificPoint(pts.Name, pts.ClosestPoint);
           
        }
    }
}
