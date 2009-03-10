using LibCSPSimple.XML;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
namespace LibCSPSimple.Tests
{
    
    
    /// <summary>
    ///Classe de test pour XmlParserTest, destinée à contenir tous
    ///les tests unitaires XmlParserTest
    ///</summary>
    [TestClass()]
    public class XmlParserTest
    {
        #region Champs
        private TestContext testContextInstance;
        #endregion

        #region Attributs de tests supplémentaires
        // 
        //Vous pouvez utiliser les attributs supplémentaires suivants lors de l'écriture de vos tests :
        //
        //Utilisez ClassInitialize pour exécuter du code avant d'exécuter le premier test dans la classe
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            File.Copy(@"..\..\..\LibCSPSimple.Tests\samples\sample1.xml", Path.Combine(testContext.TestDeploymentDir, "sample1.xml"));
        }
        //
        //Utilisez ClassCleanup pour exécuter du code après que tous les tests ont été exécutés dans une classe
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Utilisez TestInitialize pour exécuter du code avant d'exécuter chaque test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Utilisez TestCleanup pour exécuter du code après que chaque test a été exécuté
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        #region Méthodes
        #endregion

        #region Propriétés
        /// <summary>
        ///Obtient ou définit le contexte de test qui fournit
        ///des informations sur la série de tests active ainsi que ses fonctionnalités.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #endregion




        /// <summary>
        ///Test pour XmlEndElement
        ///</summary>
        [TestMethod()]
        [DeploymentItem("LibCSPSimple.dll")]
        public void XmlEndElementTest()
        {
            XmlParser_Accessor target = new XmlParser_Accessor(); // TODO : initialisez à une valeur appropriée
            target.XmlEndElement();
            Assert.Inconclusive("Une méthode qui ne retourne pas une valeur ne peut pas être vérifiée.");
        }

        /// <summary>
        ///Test pour XmlElement
        ///</summary>
        [TestMethod()]
        [DeploymentItem("LibCSPSimple.dll")]       
        public void XmlElementTest()
        {
            XmlParser_Accessor target = new XmlParser_Accessor(); // TODO : initialisez à une valeur appropriée
            target.XmlElement();
            Assert.Inconclusive("Une méthode qui ne retourne pas une valeur ne peut pas être vérifiée.");
        }

        /// <summary>
        ///Test pour Parse
        ///</summary>
        [TestMethod()]
        public void ParseTest()
        {          
            XmlParser mparseur = new XmlParser();
            mparseur.Parse(@"sample1.xml");
            Assert.Inconclusive("Une méthode qui ne retourne pas une valeur ne peut pas être vérifiée.");
        }

        /// <summary>
        ///Test pour Constructeur XmlParser
        ///</summary>
        [TestMethod()]
        public void XmlParserConstructorTest()
        {
            XmlParser target = new XmlParser();
            Assert.Inconclusive("TODO : implémentez le code pour vérifier la cible");
        }
    }
}
