using System;
using System.Collections.Generic;
using Xunit;

namespace Exercise.Tests
{
    public class UnitTest1
    {
        private Exercise.Program prog;
        public UnitTest1()
        {
            prog = new Program();
        }
        [Theory]
        [InlineData("files/file1.txt", 0)]
        [InlineData("file/file1.txt", -2)]
        [InlineData("files/files1.txt", -1)]
        public void Test1(string values, int result)
        {
            var outcome = prog.OpenFile(values);
            Assert.True(outcome == result, $"You should have returned {result} but did return {outcome}.");
        }

        [Theory]
        [InlineData("files/file1.txt", 0, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In lobortis, ligula at hendrerit facilisis, eros risus dapibus dui, et volutpat dui nibh vel nisi. Morbi gravida sapien ac odio tincidunt tristique. Praesent tristique libero tristique tincidunt varius. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. ")]
        public void Test2(string values, int result, string line)
        {
            var outcome = prog.OpenFileandReadLines(values, 1);

            Assert.True(outcome[result].Equals(line), $"You should have returned:\n {line} \nbut did return:\n {outcome[result]}.");
        } 
   }
}
