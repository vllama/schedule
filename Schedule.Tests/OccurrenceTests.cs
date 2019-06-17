using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VLL.Schedule.Tests {


    [TestClass]
    public class TestOnceOccurrence : ScheduleTestBase {
       
        [TestCleanup]
        public void tearDown()
        {
        }

        [TestMethod]
        public void OnceOccurrenceTest()
        {
            var occ = new OnceOccurrence(_date);
            Assert.IsNotNull(occ.GetNext(_date));
            Assert.IsNull(occ.GetNext(_dateout));

            Assert.AreEqual(_dateeq, occ.GetNext(_dateeq));
            Assert.AreEqual(_dateeq.Date, occ.GetNext(_dateeq, true));

        }
    }
}
