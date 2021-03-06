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
*   @file       ClRemoveSpikesMedianFilter.cs
*   @brief      Algorithm to remove spikes
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       16-11-2007
*
*   @history
*   @item		16-11-2007 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClRemoveSpikesMedianFilter : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRemoveSpikesMedianFilter();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Remove spikes\Remove spikes (Median filter)";

        public ClRemoveSpikesMedianFilter() : base(ALGORITHM_NAME) { }

        float m_SpikeThreshold = 3f;
        bool m_bDecision = true;


        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("SpikeThreshold"))
            {
                m_SpikeThreshold = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("Decision"))
            {
                m_bDecision = Boolean.Parse(p_sValue);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("SpikeThreshold", m_SpikeThreshold.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Decision", m_bDecision.ToString()));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

            if(iter.IsValid())
            {
                do
                {
                    List<float> points = new List<float>();
                    List<Cl3DModel.Cl3DModelPointIterator> Neighbors = iter.GetListOfNeighbors();

                    foreach (Cl3DModel.Cl3DModelPointIterator neighbor in Neighbors)
                        points.Add(neighbor.Z);
                    points.Add(iter.Z);

                    points.Sort();

                    float result = 0;
                    if (points.Count % 2 == 0)// even
                    {
                        int first = (int)(points.Count / 2);
                        result = (points[first - 1] + points[first]) / 2;
                    }
                    else // odd
                    {
                        int element = (int)(points.Count / 2);
                        result = points[element];
                    }

                    if (m_bDecision)
                    {
                        float diff = Math.Abs(iter.Z - result);
                        if (diff >= m_SpikeThreshold)
                        {
                            iter = p_Model.RemovePointFromModel(iter);
                        }
                        else
                            if (!iter.MoveToNext())
                                break;
                    }
                    else
                    {
                        iter.Z = result;
                        if (!iter.MoveToNext())
                            break;
                    }
                } while (iter.IsValid());
            }
        }
             
    }
}
