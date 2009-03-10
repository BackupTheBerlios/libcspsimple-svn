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
using System;
using System.Collections.Generic;
using System.Text;

namespace LibCSPSimple.Structures
{
    /// <summary>
    /// Représente la structure Graph
    /// Authors:
    ///     oliver oliver254@hotmail.fr    
    /// </summary>
    class Graph
    {
        #region Champs
        private List<Variable> pnoeuds;
        private List<Apply> papplications;
        #endregion

        #region Méthodes
        /// <summary>
        /// Construit la structure Graph
        /// </summary>
        public Graph()
        {

        }
        public void AddApply(Apply application)
        {
            if (papplications.Contains(application) == false)
            {
                papplications.Add(application);
            }
        }
        public void AddNode(Variable noeud)
        {
            if (pnoeuds.Contains(noeud) == false)
            {
                pnoeuds.Add(noeud);
            }
        }
        #endregion

        #region Propriétés
        #endregion
    }
}
