using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Net;


namespace ContactBook_API_Tests
{
    public class contactBookApiTest
    {
        private const string apiUrl = "https://contactbook.kopan77.repl.co/api";
        private RestClient client;



        [OneTimeSetUp]
        public void Setup()
        {
            this.client = new RestClient(apiUrl);
            this.client.Timeout = 3000;


        }

        [Test]
        public void testSteveJobsExist()
        {

            //Arrange
            var request = new RestRequest(@"/contacts", Method.GET);

            //Act
            var response = client.Execute(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var contactsList = new JsonDeserializer().Deserialize<List<contacts>>(response);

           // string expectedFirstName = "Steve";
           // string expectedLastName = "Jobs";
            bool isTrue = false;
            if (contactsList[0].firstName == "Steve" && contactsList[0].lastName == "Jobs") { 
                isTrue = true;
            }

            Assert.IsTrue(isTrue);

        }

        [Test]
        public void testAlbertIsAlbert()
        {

            //Arrange
            var request = new RestRequest(@"/contacts", Method.GET);

            //Act
            var response = client.Execute(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var contactList = new JsonDeserializer().Deserialize<List<contacts>>(response);

            bool isTrue = false;
            foreach (var contact in contactList)
            {

               
                string firstName = contact.firstName.ToLower();
                string lastName = contact.lastName.ToLower();
                string comment= contact.comments.ToLower();

                if (comment.Contains("albert")) {
                    if (firstName == "albert" && lastName == "einstein") {
                        isTrue = true;
                        break;
                    }                   
                }


            }
            Assert.IsTrue(isTrue);

        }

        [Test]
        public void testMissingContact()
        {

            //Arrange
            var request = new RestRequest(@"/contacts", Method.GET);

            //Act
            var response = client.Execute(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var contactsList = new JsonDeserializer().Deserialize<List<contacts>>(response);
            string randomName = "randomName" + DateTime.Now.Ticks;
            bool isTrue = false;
            foreach (var contact in contactsList)
            {


                string firstName = contact.firstName.ToLower();
                string lastName = contact.lastName.ToLower();
                

                
                
                    if (firstName == randomName || lastName == randomName)
                    {
                        isTrue = true;
                    }
                


            }
            Assert.IsFalse(isTrue);

        }

        [Test]
        public void createWrongContact()
        {
            //Arrange
            var request = new RestRequest(@"/contacts", Method.POST);

            //Act

            request.AddHeader("Content-Type", "application/json");

            var newData = new
            {


                 firstName = "dsada",
                 lastName = "sadsa",
                 email ="this is invalid email",
                 phone ="sdfasdfasdf",
                 comments = ""
                 };
            request.AddJsonBody(newData);

            var response = client.Execute(request);
            var incomingMsg = response.StatusCode.ToString();
            //Assert
            Assert.AreEqual ("BadRequest", incomingMsg);

        }
        [Test]
        public void createRealcontact()
        {
            //Arrange
            var request = new RestRequest(@"/contacts", Method.POST);

            //Act

            request.AddHeader("Content-Type", "application/json");

            var newData = new
            {


                firstName = "Ani",
                lastName = "Ivanova",
                email = "ani@email.com",
                phone = "088844126516",
                comments = "no comment"
            };
            request.AddJsonBody(newData);

            var response = client.Execute(request);
            var incomingMsg = response.StatusCode.ToString();
            //Assert
            Assert.AreEqual("Created", incomingMsg);
            // I could foreach contacts and search for this particular contact, like in some of the previous tests, but I think it will be a time wasting... It took me almost 2h. 
        }



    }
}