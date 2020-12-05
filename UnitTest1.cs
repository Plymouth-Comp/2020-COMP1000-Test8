using System;
using System.Collections.Generic;
using Xunit;
using XUnit.Project.Attributes;

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
        [InlineData("files/file1.txt", 0)]
        [InlineData("file/file1.txt", -2)]
        [InlineData("files/files1.txt", -1)]
        public void Test1(string values, int result)
        {
            var outcome = prog.OpenFile(values);
            Assert.True(outcome == result, $"When trying to open a file, you should have returned state: {result} but did return {outcome}.");
        }

        [Theory, TestPriority(4)]
        [InlineData("files/file1.txt", 0, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In lobortis, ligula at hendrerit facilisis, eros risus dapibus dui, et volutpat dui nibh vel nisi. Morbi gravida sapien ac odio tincidunt tristique. Praesent tristique libero tristique tincidunt varius. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. ")]
        [InlineData("files/file2.map", 2, "Duis vel sagittis elit. Pellentesque et viverra nibh. Proin sed lectus justo. Aliquam volutpat laoreet nisi a placerat. Sed nulla erat, volutpat in dictum ac, pulvinar non enim. Mauris finibus lacus fermentum facilisis bibendum. Mauris dui tortor, vehicula eu libero condimentum, sodales volutpat sem. Nulla facilisi.")]
        [InlineData("files/file2.map", 1, "")]
        [InlineData("files/file2.map", 8, "Sed at maximus ipsum, sed faucibus risus. Aliquam ligula dui, semper in rhoncus vel, ornare a libero. Pellentesque sit amet felis ut libero aliquet tempor. Praesent lacinia metus in luctus vehicula. Donec massa quam, mattis vitae urna vel, blandit rhoncus nulla. Nunc volutpat libero sit amet risus consequat, eget fermentum justo bibendum. Donec at dignissim mauris, nec suscipit leo. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Curabitur et interdum est, porttitor tincidunt turpis. Nullam vulputate mauris in fringilla volutpat. Cras dapibus molestie libero. In eget bibendum augue, rhoncus pharetra nulla. Maecenas lorem dolor, aliquam eget suscipit ac, dignissim at lectus. ")]
        public void Test2(string values, int result, string line)
        {
            var outcome = prog.OpenFileAndReadLines(values, result + 1);

            Assert.True(outcome[result].Equals(line), $"When trying to open and read a file: From the file, you should have returned {result + 1} lines where line nr.{result} is:\n \"{line}\" \nbut did return: \n\"{outcome[result]}\".");
        }

        [Theory, TestPriority(3)]
        [InlineData("files/file1.txt", 0, 1)]
        [InlineData("file/file1.txt", -2, 0)]
        [InlineData("files/file01.txt", -1, 0)]
        [InlineData("files/files1.map", -1, 1)]
        [InlineData("files/file2.map", 0, 2)]
        public void Test3(string values, int result, int retries)
        {
            prog = new Program();

            var outcome = prog.PersistentFileOpen(values);
            Assert.True(outcome == result, $"When trying to open a file  in persistent mode which requires an object variable: You should have returned state: {result} but did return {outcome}.");

            if (retries > 0)
            {
                if (result == 0)
                {
                    outcome = prog.PersistentFileOpen(values);
                    Assert.True(outcome == 1, "When trying to open a file  in persistent mode: Opening a file the 2nd time but did not get correct response!");

                }
                else
                {
                    outcome = prog.PersistentFileOpen(values);
                    Assert.True(outcome == result, $"When trying to open a file  in persistent mode: You should have returned file state: {result} but did return {outcome}.");
                }
            }
            else
            {
                prog = new Program();
                outcome = prog.PersistentFileOpen(values);
                Assert.True(outcome == result, $"When trying to open a file  in persistent mode: Opening the file a 2nd time after closing the object once. You should have returned file state:{result} but did return {outcome}.");

            }
        }

        [Theory, TestPriority(3)]
        [InlineData("files/file1.txt", 0)]
        [InlineData("file/file1.txt", -2)]
        [InlineData("files/file01.txt", -1)]
        [InlineData("files/files1.map", -1)]
        [InlineData("files/files.map", 0)]
        public void Test4(string values, int result)
        {
            prog = new Program();

            var outcome = prog.PersistentFileClose(values);
            Assert.True(outcome == -1, $"When trying to close a file  in persistent mode: Trying to close a file which was not opened before should return -1 instead you returned: {outcome}");



            outcome = prog.PersistentFileOpen(values);
            if (outcome == result)//file could be opened or not
            {
                if (result == 0)// file should now be open
                {
                    outcome = prog.PersistentFileClose("something weird");
                    Assert.True(outcome == -1, $"We have an open file but tried closing some other file which did not work, state: {outcome}");
                    outcome = prog.PersistentFileClose(values);
                    Assert.True(outcome == 0, $"We have an open file but closing it did not work, state: {outcome}");
                    outcome = prog.PersistentFileClose(values);
                    Assert.True(outcome == -1, "We tried closing an open file twice but the system does not return the correct state: -1");
                }
            }
            else //file open did not work correctly
            {
                Assert.True(false, "File Opening is not working correctly! To close a persistent file it needs to be open first.");
            }
        }

        [Theory, TestPriority(2)]
        [InlineData("files/file1.txt", 0, 1, "")]
        [InlineData("file/file1.txt", -2, 0, "")]
        [InlineData("files/file1.txt", 0, 1, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In lobortis, ligula at hendrerit facilisis, eros risus dapibus dui, et volutpat dui nibh vel nisi. Morbi gravida sapien ac odio tincidunt tristique. Praesent tristique libero tristique tincidunt varius. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.")]
        [InlineData("files/file2.map", 0, 2, "Duis vel sagittis elit. Pellentesque et viverra nibh. Proin sed lectus justo. Aliquam volutpat laoreet nisi a placerat. Sed nulla erat, volutpat in dictum ac, pulvinar non enim. Mauris finibus lacus fermentum facilisis bibendum. Mauris dui tortor, vehicula eu libero condimentum, sodales volutpat sem. Nulla facilisi.")]
        [InlineData("files/file2.map", 0, 1, "")]
        [InlineData("files/file2.map", 0, 8, "Sed at maximus ipsum, sed faucibus risus. Aliquam ligula dui, semper in rhoncus vel, ornare a libero. Pellentesque sit amet felis ut libero aliquet tempor. Praesent lacinia metus in luctus vehicula. Donec massa quam, mattis vitae urna vel, blandit rhoncus nulla. Nunc volutpat libero sit amet risus consequat, eget fermentum justo bibendum. Donec at dignissim mauris, nec suscipit leo. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Curabitur et interdum est, porttitor tincidunt turpis. Nullam vulputate mauris in fringilla volutpat. Cras dapibus molestie libero. In eget bibendum augue, rhoncus pharetra nulla. Maecenas lorem dolor, aliquam eget suscipit ac, dignissim at lectus.")]

        public void Test5(string values, int result, int lines, string line)
        {
            prog = new Program();

            var outcome = prog.PersistentFileRead(lines);
            Assert.True(outcome.Length == 0, $"When trying to read a file  in persistent mode: Trying to read from a file which is not open did return {outcome.Length} lines.");

            var openCheck = prog.PersistentFileOpen(values);
            Assert.True(openCheck == result, $"When trying to read a file  in persistent mode: Trying to open a file for persistent access did not work.");

            if (result != 0)
                return;
            openCheck = prog.PersistentFileClose(values);
            Assert.True(openCheck == result, $"Trying to close a file for persistent access did not work.");
            if (result != 0)
                return;


            outcome = prog.PersistentFileRead(lines);
            Assert.True(outcome.Length == 0, $"Trying to read from a file which is not open did return {outcome.Length} lines.");

            prog.PersistentFileOpen(values);
            outcome = prog.PersistentFileRead(lines);
            Assert.True(outcome[lines - 1].Equals(line), $"From the file, you should have returned {lines} lines where line nr.{lines - 1} is:\n {line} \nbut did return:\n {outcome[lines - 1]}.");

            Assert.True(outcome[lines - 1].Equals(line), $"Reading a second time should return the same result.\nFrom the file, you should have returned {lines} lines where line nr.{lines - 1} is:\n {line} \nbut did return:\n {outcome[lines - 1]}.");

            openCheck = prog.PersistentFileClose(values);
            Assert.True(openCheck == result, $"Trying to close a file for persistent access did not work.");
        }
    }
}
