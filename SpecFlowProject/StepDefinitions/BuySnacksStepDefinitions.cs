using System;
using TechTalk.SpecFlow;
using ArenaGestor.Domain;
using System.Collections.Generic;
using ArenaGestor.Business;
using ArenaGestor.BusinessInterface;
using ArenaGestor.DataAccessInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Xml.Linq;

namespace SpecFlowProject.StepDefinitions
{
    [Binding]
    public class BuySnacksStepDefinitions
    {

        private static Mock<ITicketManagement>? managementMock = new Mock<ITicketManagement>(MockBehavior.Strict);
        private static Mock<IConcertsService>? concertServiceMock = new Mock<IConcertsService>(MockBehavior.Strict);
        private static Mock<ISecurityService>? securityServiceMock = new Mock<ISecurityService>(MockBehavior.Strict);
        private static Mock<ITicketStatusManagement>? ticketStatusManagementMock = new Mock<ITicketStatusManagement>(MockBehavior.Strict);
        private static TicketService? ticketService = new TicketService(concertServiceMock.Object, managementMock.Object, ticketStatusManagementMock.Object, securityServiceMock.Object);




        private int snackQuantity;
        private int snackQuantity1;
        private int ticketQuantity;
        private TicketBuy? ticketPurchased;
        private Concert concertOk = new Concert()
        {
            ConcertId = 1,
            TourName = "Olé Tour",
            Date = DateTime.Now.AddDays(10),
            Price = 100,
            TicketCount = 500,
            Location = new Location()
            {
                Country = new Country()
                {
                    CountryId = 1,
                    Name = "Uruguay"
                },
                LocationId = 1,
                Number = 1234,
                Place = "Estadio Centenario",
                Street = "Av. Ricaldoni"
            }
        };
        private User userOk = new User()
        {
            UserId = 1,
                Name = "Test",
                Surname = "User",
                Email = "test@user.com",
                Password = "testuser123",
                Roles = new List<UserRole>() {
                    new UserRole()
                    {
                        RoleId = RoleCode.Administrador
                    }
                }
        };

        private TicketStatus buyedStatus = new TicketStatus()
        {
            TicketStatusId = TicketCode.Comprado,
                Status = TicketCode.Comprado.ToString()
            };


        [Given(@"I buy (.*) snacks")]
        public void GivenIBuySnacks(int p0)
        {
            snackQuantity = p0;
        }

        [Given(@"(.*) tickets")]
        public void GivenTickets(int p0)
        {
            ticketQuantity = p0;
        }

        [When(@"I purchase the tickets with the snack")]
        public void WhenIPurchaseTheTicketsWithTheSnack()
        {
            securityServiceMock.Setup(x => x.GetUserOfToken(It.IsAny<string>())).Returns(userOk);

            ticketStatusManagementMock.Setup(x => x.GetStatus(It.IsAny<TicketCode>())).Returns(buyedStatus);
            concertServiceMock.Setup(x => x.GetConcertById(It.IsAny<int>())).Returns(concertOk);
            managementMock.Setup(x => x.InsertTicket(It.IsAny<Ticket>()));
            managementMock.Setup(x => x.Save());

            ticketPurchased = new TicketBuy()
            {
                Amount = ticketQuantity,
                Snacks = new List<SnackBuy>() 
                {
                    new SnackBuy()
                    {
                        Snack = new Snack()
                        {
                            Price = 1,
                            Description = "test"
                        },
                        Amount = snackQuantity
                    }
                }
            };
            ticketService?.BuyTicket("", ticketPurchased);
            concertServiceMock.VerifyAll();
            managementMock.VerifyAll();         
        }

        [Then(@"the ticket should have an ammount of (.*)")]
        public void ThenTheTicketShouldHaveAnAmmountOf(int p0)
        {
            Assert.AreEqual(p0, ticketPurchased!.Amount);
        }

        [Then(@"there should be (.*) snack")]
        public void ThenThereShouldBeSnack(int p0)
        {
            Assert.AreEqual(p0, ticketPurchased!.Snacks.Count);
        }

        [Given(@"I buy (.*) snacks of one type")]
        public void GivenIBuySnacksOfOneType(int p0)
        {
            snackQuantity = p0;
        }

        [Given(@"(.*) snacks of another type")]
        public void GivenSnacksOfAnotherType(int p0)
        {
            snackQuantity1 = p0;
        }

        [When(@"I purchase the tickets with the snacks")]
        public void WhenIPurchaseTheTicketsWithTheSnacks()
        {
            securityServiceMock.Setup(x => x.GetUserOfToken(It.IsAny<string>())).Returns(userOk);

            ticketStatusManagementMock.Setup(x => x.GetStatus(It.IsAny<TicketCode>())).Returns(buyedStatus);
            concertServiceMock.Setup(x => x.GetConcertById(It.IsAny<int>())).Returns(concertOk);
            managementMock.Setup(x => x.InsertTicket(It.IsAny<Ticket>()));
            managementMock.Setup(x => x.Save());

            ticketPurchased = new TicketBuy()
            {
                Amount = ticketQuantity,
                Snacks = new List<SnackBuy>()
                {
                    new SnackBuy()
                    {
                        Snack = new Snack()
                        {
                            Price = 1,
                            Description = "test"
                        },
                        Amount = snackQuantity
                    },
                    new SnackBuy()
                    {
                        Snack = new Snack()
                        {
                            Price = 100,
                            Description = "test1"
                        },
                        Amount = snackQuantity1
                    }
                }
            };
            ticketService?.BuyTicket("", ticketPurchased);
            concertServiceMock.VerifyAll();
            managementMock.VerifyAll();
        }

        [When(@"I purchase the tickets without the snacks")]
        public void WhenIPurchaseTheTicketsWithoutTheSnacks()
        {
            securityServiceMock.Setup(x => x.GetUserOfToken(It.IsAny<string>())).Returns(userOk);

            ticketStatusManagementMock.Setup(x => x.GetStatus(It.IsAny<TicketCode>())).Returns(buyedStatus);
            concertServiceMock.Setup(x => x.GetConcertById(It.IsAny<int>())).Returns(concertOk);
            managementMock.Setup(x => x.InsertTicket(It.IsAny<Ticket>()));
            managementMock.Setup(x => x.Save());

            ticketPurchased = new TicketBuy()
            {
                Amount = ticketQuantity,
                Snacks = new List<SnackBuy>()
                {
                    
                }
            };
            ticketService?.BuyTicket("", ticketPurchased);
            concertServiceMock.VerifyAll();
            managementMock.VerifyAll();
        }


    }
}
