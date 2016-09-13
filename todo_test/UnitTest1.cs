using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using todo;

namespace todo_test
{
    [TestClass]
    public class UnitTest1
    {
        #region Tests for items which are not completed
        [TestMethod]
        public void Item_create_not_completed_no_dates_no_priority()
        {
            var item = new Item("Do a task with no dates and no priority");

            Assert.AreEqual("Do a task with no dates and no priority", item.Text);
            Assert.IsNull(item.Priority);
            Assert.AreEqual(DateTime.MinValue, item.DateAdded);
            Assert.AreEqual(DateTime.MinValue, item.DateCompleted);
            Assert.AreEqual(false, item.IsCompleted);
        }

        [TestMethod]
        public void Item_create_not_completed_no_dates_with_priority()
        {
            var item = new Item("(B) This is a task with no dates and a priority");

            Assert.AreEqual("This is a task with no dates and a priority", item.Text);
            Assert.AreEqual("B", item.Priority);
            Assert.AreEqual(DateTime.MinValue, item.DateAdded);
            Assert.AreEqual(DateTime.MinValue, item.DateCompleted);
            Assert.AreEqual(false, item.IsCompleted);
        }

        [TestMethod]
        public void Item_create_not_completed_created_date_no_priority()
        {
            var item = new Item("2015-12-25 This is a task with a creation date and no priority");

            Assert.AreEqual("This is a task with a creation date and no priority", item.Text);
            Assert.IsNull(item.Priority);
            Assert.AreEqual(new DateTime(2015, 12, 25), item.DateAdded);
            Assert.AreEqual(DateTime.MinValue, item.DateCompleted);
            Assert.AreEqual(false, item.IsCompleted);
        }

        [TestMethod]
        public void Item_create_not_completed_created_date_and_priority()
        {
            var item = new Item("(A) 2015-12-25 This is a task with a creation date and a priority");

            Assert.AreEqual("This is a task with a creation date and a priority", item.Text);
            Assert.AreEqual("A", item.Priority);
            Assert.AreEqual(new DateTime(2015, 12, 25), item.DateAdded);
            Assert.AreEqual(DateTime.MinValue, item.DateCompleted);
            Assert.AreEqual(false, item.IsCompleted);
        }


        [TestMethod]
        public void Not_Completed_Item_ToString()
        {
            var item = new Item("(A) 2015-12-25 This is a task with a creation date and a priority");

            Assert.AreEqual("This is a task with a creation date and a priority", item.Text);
            Assert.AreEqual("A", item.Priority);
            Assert.AreEqual(new DateTime(2015, 12, 25), item.DateAdded);
            Assert.AreEqual(DateTime.MinValue, item.DateCompleted);
            Assert.AreEqual(false, item.IsCompleted);

            Assert.AreEqual("(A) 2015-12-25 This is a task with a creation date and a priority", item.ToString());
        }

        #endregion
        #region Tests for items which are completed
        [TestMethod]
        public void Item_create_completed_no_dates_no_priority()
        {
            var item = new Item("x Do a task with no dates and no priority");

            Assert.AreEqual("Do a task with no dates and no priority", item.Text);
            Assert.IsNull(item.Priority);
            Assert.AreEqual(DateTime.MinValue, item.DateAdded);
            Assert.AreEqual(DateTime.MinValue, item.DateCompleted);
            Assert.AreEqual(true, item.IsCompleted);
        }


        [TestMethod]
        public void Item_create_completed_no_dates_with_priority()
        {
            var item = new Item("x (B) This is a completed task with no dates and a priority");

            Assert.AreEqual("This is a completed task with no dates and a priority", item.Text);
            Assert.AreEqual("B", item.Priority);
            Assert.AreEqual(DateTime.MinValue, item.DateAdded);
            Assert.AreEqual(DateTime.MinValue, item.DateCompleted);
            Assert.AreEqual(true, item.IsCompleted);
        }


        [TestMethod]
        public void Item_create_completed_created_date_no_priority()
        {
            //Using todo.sh as a reference - when a task is set to Done the priority appears to be stripped off
            var item = new Item("x 2016-09-12 2015-12-25 This is a completed task with a creation date and no priority");

            Assert.AreEqual("This is a completed task with a creation date and no priority", item.Text);
            Assert.IsNull(item.Priority);
            Assert.AreEqual(new DateTime(2015, 12, 25), item.DateAdded);
            Assert.AreEqual(new DateTime(2016, 09, 12), item.DateCompleted);
            Assert.AreEqual(true, item.IsCompleted);
        }


        [TestMethod]
        public void Item_create_completed_created_date_and_priority()
        {
            var item = new Item("x (A) 2016-08-23 2015-12-25 This is a completed task with a creation date and a priority");

            Assert.AreEqual("This is a completed task with a creation date and a priority", item.Text);
            Assert.AreEqual("A", item.Priority);
            Assert.AreEqual(new DateTime(2015, 12, 25), item.DateAdded);
            Assert.AreEqual(new DateTime(2016, 08, 23), item.DateCompleted);
            Assert.AreEqual(true, item.IsCompleted);
        }

        [TestMethod]
        public void Completed_Item_ToString()
        {
            var item = new Item("x (A) 2016-08-23 2015-12-25 This is a completed task with a creation date and a priority");

            Assert.AreEqual("This is a completed task with a creation date and a priority", item.Text);
            Assert.AreEqual("A", item.Priority);
            Assert.AreEqual(new DateTime(2015, 12, 25), item.DateAdded);
            Assert.AreEqual(new DateTime(2016, 08, 23), item.DateCompleted);
            Assert.AreEqual(true, item.IsCompleted);

            Assert.AreEqual("x (A) 2016-08-23 2015-12-25 This is a completed task with a creation date and a priority", item.ToString());
        }
        #endregion
    }
}
