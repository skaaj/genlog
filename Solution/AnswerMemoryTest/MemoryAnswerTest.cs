using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Collections.Generic;
using Genlog;


namespace AnswerMemoryTest
{
    [TestClass]
    public class MemoryAnswerTest ///Permet d'effectuer un test unitaire sur des méthodes présentes dans MemoryTestView
    {
        //Il semble que le fait d'utiliser des bibliothèques externes implique une mise en place des tests unitaires différentes de celles vues en cours, un débogage n'a pas été trouvé
        [TestMethod]
        public void CountAnswerTest()
        {
            // arrange
            ImageNombre IN11 = new ImageNombre("image1","2");
            ImageNombre IN12 = new ImageNombre("image2", "2");
            ImageNombre IN13 = new ImageNombre("image1", "2");

            ImageNombre IN21 = new ImageNombre("image1", "2");
            ImageNombre IN22 = new ImageNombre("image2", "2");
            ImageNombre IN23 = new ImageNombre("image3", "2");


            List<ImageNombre> listeJuste = new List<ImageNombre> { IN11, IN12, IN13 };
            List<ImageNombre> listeAVerifier = new List<ImageNombre> { IN21, IN22, IN23 };

            MainWindow MW = new MainWindow();
            MemoryTestActivity _parent = new MemoryTestActivity(MW);
            _parent.listeMemorisation = listeJuste;
            _parent.ListeReponse = listeAVerifier;
            
            AnswerMemoryView view1 = new AnswerMemoryView(_parent); ;
 
            int[] equivalent = new int[3] {1,2,3};

            // act
            view1.VerificationReponse();

            // assert
            int expected = 2;
            int actual = 0;

            foreach (ImageNombre nIN in _parent.ListeReponse)
            {
                if(nIN._result == true)
                {
                    actual = actual +1;
                }
            }

            Assert.AreEqual(expected, actual, 0.001, "Mauvais compte de réponse");

        }
    }
}
