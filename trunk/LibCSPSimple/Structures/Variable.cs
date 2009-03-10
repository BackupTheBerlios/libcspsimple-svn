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
using System.Collections;
using System.Text;

namespace LibCSPSimple.Structures
{
    /// <summary>
    /// Représente une variable
    /// Authors:
    ///     oliver oliver254@hotmail.fr        
    /// </summary>
    class Variable
    {
        #region Champs
        /// <summary>
        /// Représente le nom
        /// </summary>
        private string pnom;
        /// <summary>
        /// Représente les valeurs
        /// </summary>
        private ArrayList pvaleurs;
        #endregion

        #region Méthodes
        /// <summary>
        /// Construit une variable 
        /// </summary>
        /// <param name="nom"></param>
        Variable(string nom)
        {
            pnom = nom;
            pvaleurs = new ArrayList();
        }
        /// <summary>
        /// Ajoute une valeur
        /// </summary>
        /// <param name="valeur"></param>
        public void Add(int valeur)
        {
            if (pvaleurs.Contains(valeur) == false)
            {
                pvaleurs.Add(valeur);
            }
        }
        public override bool Equals(object obj)
        {
            bool bresult = false;           

            if (obj.GetType() == typeof(Variable))
            {
                Variable mvar = (Variable)obj;
                bresult = mvar.Name.Equals(pnom);

            }
            return bresult;
        }
        /// <summary>
        /// Supprime une valeur
        /// </summary>
        /// <param name="valeur"></param>
        public void Remove(int valeur)
        {
            pvaleurs.Remove(valeur);
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Retourne le nom
        /// </summary>
        public string Name
        {
            get { return pnom; }
        }
        /// <summary>
        /// Retourne les valeurs
        /// </summary>
        public ArrayList Values
        {
            get { return pvaleurs; }
        }
        #endregion
    }
}
