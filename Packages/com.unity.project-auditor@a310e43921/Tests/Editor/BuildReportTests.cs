using System.IO;
using System.Linq;
using NUnit.Framework;
using Unity.ProjectAuditor.Editor;
using Unity.ProjectAuditor.Editor.Auditors;
using UnityEditor;
using UnityEngine;

namespace Unity.ProjectAuditor.EditorTests
{
    class BuildReportTests
    {
        private TempAsset m_TempAsset;

        [OneTimeSetUp]
        public void SetUp()
        {
            var material = new Material(Shader.Find("UI/Default"));
            m_TempAsset = TempAsset.Save(material, "Resources/Shiny.mat");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            TempAsset.Cleanup();
        }

        [Test]
        public void BuildReport_IsSupported()
        {
            var projectAuditor = new Unity.ProjectAuditor.Editor.ProjectAuditor();
            var module = projectAuditor.GetModule<BuildReportModule>();
            var isSupported = module.IsSupported();
#if BUILD_REPORT_API_SUPPORT
            Assert.True(isSupported);
#else
            Assert.False(isSupported);
#endif
        }

        [Test]
#if !BUILD_REPORT_API_SUPPORT
        [Ignore("Not Supported in this version of Unity")]
#endif
        public void BuildReport_Files_AreReported()
        {
            var issues = Utility.AnalyzeBuild(IssueCategory.BuildFile, i => i.relativePath.Equals(m_TempAsset.relativePath));
            var matchingIssue = issues.FirstOrDefault();

            Assert.NotNull(matchingIssue);
            Assert.AreEqual(Path.GetFileNameWithoutExtension(m_TempAsset.relativePath), matchingIssue.description);
            Assert.That(matchingIssue.GetNumCustomProperties(), Is.EqualTo((int)BuildReportFileProperty.Num));
            Assert.AreEqual("resources.assets", matchingIssue.GetCustomProperty(BuildReportFileProperty.BuildFile));
            Assert.AreEqual(typeof(Material).ToString(), matchingIssue.GetCustomProperty(BuildReportFileProperty.Type));
            Assert.That(matchingIssue.GetCustomPropertyAsInt(BuildReportFileProperty.Size), Is.Positive);
        }

        [Test]
        public void BuildReportSize_Font_IsReported()
        {
            Assert.AreEqual("Font", BuildReportModule.GetDescriptor("font.ttf", typeof(Font)).description);
        }

        [Test]
        public void BuildReportSize_Script_IsReported()
        {
            Assert.AreEqual("Script", BuildReportModule.GetDescriptor("script.cs", typeof(MonoScript)).description);
        }

        [Test]
        public void BuildReportSize_Sprite_IsReported()
        {
            Assert.AreEqual("Texture", BuildReportModule.GetDescriptor("sprite.png", typeof(Sprite)).description);
        }

        [Test]
#if !BUILD_REPORT_API_SUPPORT
        [Ignore("Not Supported in this version of Unity")]
#endif
        public void BuildReport_Steps_AreReported()
        {
            var issues = Utility.AnalyzeBuild(IssueCategory.BuildStep);
            var step = issues.FirstOrDefault(i => i.description.Equals("Build player"));
            Assert.NotNull(step);
            Assert.That(step.depth, Is.EqualTo(0));

            step = issues.FirstOrDefault(i => i.description.Equals("Compile scripts"));
            Assert.NotNull(step, "\"Compile scripts\" string not found");
#if UNITY_2021_1_OR_NEWER
            Assert.That(step.depth, Is.EqualTo(3));
#else
            Assert.That(step.depth, Is.EqualTo(1));
#endif

            step = issues.FirstOrDefault(i => i.description.Equals("Postprocess built player"));
            Assert.NotNull(step, "\"Postprocess built player\" string not found");
            Assert.That(step.depth, Is.EqualTo(1));
        }
    }
}
