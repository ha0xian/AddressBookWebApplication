using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Address_Book;

namespace UnitTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestIndexView()
        {
            var controller = new AddressBookController();
            var result = controller.AddAddress(models) as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void TestDelete()
        {
            var controller = new AddressBookController();
            controller.Delete(1);
            int result = controller.getLength();
            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void TestAdd()
        {
            AddressBookViewModel addressVm = new AddressBookViewModel()
            {
                first_name="hao", last_name="Xian", phone="1234567", email="haoxian@gmail.com"
            }
            var controller = new AddressBookController();
            controller.Add(addressVm);
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void TestDeleteRedirect()
        {
            var controller = new AddressBookController();
            var result = (RedirectToRouteResult) controller.Delete(1);
            Assert.AreEqual("Index", result.Values["actoin"])
        }
}
