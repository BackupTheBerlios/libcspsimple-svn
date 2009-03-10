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
    /// Représente la structure Apply
    /// Authors:
    ///     oliver oliver254@hotmail.fr 
    /// </summary>
    class Apply : IArg
    {
        #region Champs
        /// <summary>
        /// Représente l'opérateur
        /// </summary>
        private TOp poperateur;
        /// <summary>
        /// Représente les arguments
        /// </summary>
        private IArg pargument1, pargument2;
        #endregion

        #region Méthodes
        public Apply()
        {
            poperateur = TOp.Null;
            pargument1 = pargument2 = null;
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Retourne ou définit le premier argument
        /// </summary>
        public IArg Argument1
        {
            get { return pargument1; }
            set { pargument1 = value; }
        }
        /// <summary>
        /// Retourne ou définit le deuxieme argument
        /// </summary>
        public IArg Argument2
        {
            get { return pargument2; }
            set { pargument2 = value; }
        }
        /// <summary>
        /// Retourne ou définit l'opérateur
        /// </summary>
        public TOp Operator
        {
            get { return poperateur; }
            set { poperateur = value; }
        }
        #endregion

        #region Types imbriqués
        public enum TOp { Eq, Minus, Neq, Null }
        #endregion
    }
}
