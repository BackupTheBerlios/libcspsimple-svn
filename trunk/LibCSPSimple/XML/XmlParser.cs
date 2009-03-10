/* LibCSPSimple - Library of Constraint satisfaction problems (CSP)
 * Copyright (C) 2009  Sourcealbert254.net
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */
using LibCSPSimple.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace LibCSPSimple.XML
{
    /// <summary>
    /// Parseur XML
    /// Authors:
    ///     oliver oliver254@hotmail.fr 
    /// </summary>
    class XmlParser
    {
        #region Champs
        private T_ETATS petat;
        /// <summary>
        /// Représente la pile XML
        /// </summary>
        private Stack<IArg> ppile;
        /// <summary>
        /// Représente le parseur
        /// </summary>
        private XmlTextReader pxtreader;
        #endregion

        #region Méthodes
        /// <summary>
        /// Construit l'arbre XML
        /// </summary>
        public XmlParser()
        {
            ppile = new Stack<IArg>();
            petat = T_ETATS.ETAT_INIT;
        }
        /// <summary>
        /// Parse un document XML
        /// </summary>
        /// <param name="filename"></param>
        public void Parse(string filename)
        {
            if (File.Exists(filename) == false)
            {
                throw new Exception("This file does not exists");
            }
            pxtreader = new XmlTextReader(filename);
            try
            {
                while (pxtreader.Read())
                {
                    switch (pxtreader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                this.XmlElement();                               
                                break;
                            }
                        case XmlNodeType.EndElement:
                            {
                                this.XmlEndElement();                                
                                break;
                            }
                    }
                }
            }
            finally
            {
                pxtreader.Close();
            }
        }
        /// <summary>
        /// Parse l'élément de fin
        /// </summary>
        private void XmlEndElement()
        {
            switch (petat)
            {
                case T_ETATS.ETAT_MATH:
                    {
                        if (pxtreader.LocalName.Equals("math") == true)
                        {
                            petat = T_ETATS.ETAT_INIT;
                        }
                        break;
                    }
                case T_ETATS.ETAT_DECLARE:
                    {
                        if (pxtreader.LocalName.Equals("declare") == true)
                        {
                            petat = T_ETATS.ETAT_MATH;
                        }
                        break;
                    }
                case T_ETATS.ETAT_APPLY:
                    {
                        if (pxtreader.LocalName.Equals("apply") == true && pxtreader.Depth == 1)
                        {
                            petat = T_ETATS.ETAT_MATH;
                        }

                        break;
                    }
            }
        }
        /// <summary>
        /// Parse l'élément
        /// </summary>
        private void XmlElement()
        {
            switch (petat)
            {
                case T_ETATS.ETAT_INIT:
                    {
                        if (pxtreader.LocalName.Equals("math") == true)
                        {
                            petat = T_ETATS.ETAT_MATH;
                        }
                        else
                        {
                            throw new Exception("Error!");
                        }
                        break;
                    }
                case T_ETATS.ETAT_MATH:
                    {
                        if (pxtreader.LocalName.Equals("declare") == true)
                        {
                            //Variable var = new Variable();
                            petat = T_ETATS.ETAT_DECLARE;
                        }
                        else if (pxtreader.LocalName.Equals("apply") == true)
                        {
                            petat = T_ETATS.ETAT_APPLY;
                        }
                        break;
                    }
                case T_ETATS.ETAT_DECLARE:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Propriétés
        #endregion

        #region Types imbriqués
        private enum T_ETATS { ETAT_INIT, ETAT_MATH, ETAT_DECLARE, ETAT_APPLY }
        #endregion
    }
}
