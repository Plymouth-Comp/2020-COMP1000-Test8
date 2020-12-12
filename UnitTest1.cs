using System;
using System.Collections.Generic;
using Xunit;
using XUnit.Project.Attributes;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Exercise.Tests
{
    [TestCaseOrderer("XUnit.Project.Orderers.PriorityOrderer", "XUnit.Project")]
    public class UnitTest1
    {
        private Exercise.Program prog;
        public UnitTest1()
        {
            prog = new Program();
        }
        [Theory, TestPriority(5)]
        [InlineData(new int[] { 1, 2 }, 3)]
        [InlineData(new int[] { 1 }, 1)]
        [InlineData(new int[] { 1, -2 }, -1)]
        [InlineData(new int[] {1, 2, 3 }, 6)]
        [InlineData(new int[] { 1, -1, 2, 3, 67, 1000, 15, -12 }, 1075)]
        public void Test1(int [] values, int result)
        {
            var outcome = prog.RecursiveSumFromLast(values,values.Length-1);
            Assert.True(outcome == result, $"You should have returned: {result} but did return {outcome}.");
        }

        [Theory, TestPriority(4)]
        [InlineData(new int[] { 1, 2, 3 }, 6)]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 11 }, 56)]
        [InlineData(new int[] { -1, -2, -3 }, -6)]
        [InlineData(new int[] { -1, -2, -3, 6 }, 0)]
        public void Test2(int[] values, int result)
        {
            var outcome = prog.RecursiveSumFromFirst(values, 0);
            Assert.True(outcome == result, $"You should have returned: {result} but did return {outcome}.");
        }

        [Theory, TestPriority(4)]
        [InlineData(new float[] { 1.0f, 2.0f, 3.0f }, 6.0f)]
        [InlineData(new float[] { -1.0f, 2.0f, 3.0f }, 4.0f)]
        [InlineData(new float[] { -1.0f, 2.0f, 3.0f, 2.0f }, 6.0f)]
        [InlineData(new float[] { -1.1f, 2.2f, 3.1f, 10.0f, 15.0f, 23.1f, 22,12f }, 86.30f)]
        public void Test3(float [] values, float result)
        {
            var test = new List<float>(values); 
            var outcome = prog.RecursiveSum(test);

            Assert.True(Math.Round(outcome,2) == Math.Round(result,2), $"You should have returned: {result} but did return {outcome}.");
        }

    }
}
