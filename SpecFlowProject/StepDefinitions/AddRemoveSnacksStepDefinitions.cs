using System;
using TechTalk.SpecFlow;
using ArenaGestor.Domain;
using System.Collections.Generic;
using ArenaGestor.Business;
using ArenaGestor.BusinessInterface;
using ArenaGestor.DataAccessInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using TechTalk.SpecFlow;
using System.Diagnostics;

namespace SpecFlowProject.StepDefinitions
{
    [Binding]
    public class AddRemoveSnacksStepDefinitions
    {
      
        private static Mock<ISnackManagement> managementMock = new Mock<ISnackManagement>(MockBehavior.Strict);
        private static SnackService snackService = new SnackService(managementMock.Object);
        private static Snack snack;
        private static int removeSnackId;
        private static int invilidRemoveSnackId;
        private static Snack invalidSnack;
        private static List<Snack> snackList = new List<Snack>() { snack };

   
        [Given(@"A valid snack with price (.*) and description (.*)")]
        public void GivenAValidSnackWithPriceAndDescriptionPapas(int price, string description)
        {
         
            snack = new Snack()
            {
                Id = 1,
                Description = description,
                Price = price
            };
        }


        [When(@"I add the snack")]
        public void WhenIAddTheSnack()
        {
            managementMock.Setup(x => x.Save());
            managementMock.Setup(x => x.InsertSnack(It.IsAny<Snack>()));
            snackService.AddSnack(snack);
            managementMock.VerifyAll();
        }

        [Then(@"the snack should be added")]
        public void ThenTheSnackShouldBeAdded()
        {
            managementMock.Setup(x => x.GetSnacks()).Returns(snackList);
            Assert.AreEqual(snackService.GetSnacks().Count, 1);
            managementMock.VerifyAll();
        }

        [When(@"I add the invalid snack")]
        public void WhenIAddTheInvalidSnack()
        {
           
        }


        [Given(@"A invalid snack with price (.*) and description (.*)")]
        public void GivenAInvalidSnackWithPriceAndDescriptionPapas(int price, string description)
        {
            invalidSnack = new Snack()
            {
                Id = 1,
                Description = description,
                Price = price
            };
        }
        
        [Then(@"a exception should be thrown saying (.*)")]
        public void ThenAExceptionShouldBeThrown(string error)
        {
            try
            {
                snackService.AddSnack(invalidSnack);
            }
            catch (Exception e)
            {
               Assert.AreEqual(e.Message, error);
            }
           
        }

        [Given(@"A valid snack id (.*)")]
        public void GivenAValidSnackId(int p0)
        {
            removeSnackId = p0;
        }

        [When(@"I remove the valid snack")]
        public void WhenIRemoveTheValidSnack()
        {
            managementMock.Setup(x => x.Save());
            managementMock.Setup(x => x.DeleteSnack(It.IsAny<Snack>()));
            snackService.RemoveSnack(removeSnackId);
            managementMock.VerifyAll();
        }

        [Then(@"the snack should be removed")]
        public void ThenTheSnackShouldBeRemoved()
        {
            
        }

        [Given(@"A invalid snack id (.*)")]
        public void GivenAInvalidSnackId(int p0)
        {
            invilidRemoveSnackId = p0;
        }

        [When(@"I remove the invalid snack")]
        public void WhenIRemoveTheInvalidSnack()
        {
           
          
           
        }

        [Then(@"a exception should be thrown that says (.*)")]
        public void ThenAExceptionShouldBeThrownThatSaysSnackWasInvalid(string error)
        {
            try
            {
                snackService.RemoveSnack(invilidRemoveSnackId);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, error);
            }
        }



    }
}