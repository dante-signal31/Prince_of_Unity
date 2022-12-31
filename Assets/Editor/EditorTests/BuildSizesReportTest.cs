using System.Collections.Generic;
using NUnit.Framework;

namespace Editor.EditorTests
{
    public class BuildSizesReportTest
    {
        private const string _logPath = @"Assets\Tests\TestResources\Editor.log";
        
        /// <summary>
        /// Assert build report is properly found and parsed in log file.
        /// </summary>
        [NUnit.Framework.Test]
        public void BuildSizesReportReadingTest()
        {
            Dictionary<string, BuildReportEntry> reportData = BuildSizesReport.GetLastBuildReportData(_logPath);
            Assert.True(reportData["Textures"].Size == "15.9");
            Assert.True(reportData["Textures"].SizeUnit == "MB");
            Assert.True(reportData["Other Assets"].Percentage == "64.3%");
            Assert.True(reportData["Complete build size"].Size == "105.0");
        }
        
        /// <summary>
        /// Assert a given build report line is properly parsed.
        /// </summary>
        [NUnit.Framework.Test]
        public void ParseLineTest()
        {
            string line1 = "Textures               14.9 mb	 28.8% ";
            BuildReportEntry entryLine1 = BuildSizesReport.ParseLine(line1);
            Assert.True(entryLine1.Size == "14.9");
            Assert.True(entryLine1.SizeUnit == "MB");
            Assert.True(entryLine1.Percentage == "28.8%");
            
            string line2 = "Animations             483.5 kb	 0.9% ";
            BuildReportEntry entryLine2 = BuildSizesReport.ParseLine(line2);
            Assert.True(entryLine2.Size == "483.5");
            Assert.True(entryLine2.SizeUnit == "KB");
            Assert.True(entryLine2.Percentage == "0.9%");
            
            string line3 = "Complete build size    104.8 mb";
            BuildReportEntry entryLine3 = BuildSizesReport.ParseLine(line3);
            Assert.True(entryLine3.Size == "104.8");
            Assert.True(entryLine3.SizeUnit == "MB");
            Assert.True(entryLine3.Percentage == "");
        }

        // // A UnityTest behaves like a coroutine in PlayMode
        // // and allows you to yield null to skip a frame in EditMode
        // [UnityEngine.TestTools.UnityTest]
        // public System.Collections.IEnumerator BuildSizesReportTestWithEnumeratorPasses()
        // {
        //     // Use the Assert class to test conditions.
        //     // yield to skip a frame
        //     yield return null;
        // }
    }
}