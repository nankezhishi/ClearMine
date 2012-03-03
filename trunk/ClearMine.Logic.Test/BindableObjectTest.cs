namespace ClearMine.Logic.Test
{
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass()]
    public class BindableObjectTest
    {
        [TestMethod()]
        public void SetPropertyPerformanceTest()
        {
            var times = 1000000;
            var obj = new SampleObject() { Property = -1, SampleProperty = -1 };

            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < times; i++)
            {
                obj.SampleProperty = i;
            }
            var stringSetter = stopwatch.ElapsedTicks;

            stopwatch.Restart();
            for (int i = 0; i < times; i++)
            {
                obj.Property = i;
            }
            var lambdaSetter = stopwatch.ElapsedTicks;

            Assert.IsTrue(lambdaSetter > stringSetter);
            Assert.IsTrue(lambdaSetter < 2 * stringSetter);
        }
    }
}
