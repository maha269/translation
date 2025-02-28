﻿using NUnit.Framework;
using Shouldly;

using Translation.Client.Web.Models.LabelTranslation;
using static Translation.Common.Tests.TestHelpers.FakeModelTestHelper;
using static Translation.Common.Tests.TestHelpers.FakeConstantTestHelper;

namespace Translation.Client.Web.Unit.Tests.Models.ViewModels.LabelTranslation
{
    [TestFixture]
    public class TranslationCSVDataTests
    {
        public TranslationCSVData SystemUnderTest { get; set; }

        [SetUp]
        public void run_before_every_test()
        {
            SystemUnderTest = GetTranslationCSVData();
        }

        [Test]
        public void LanguageRevisionReadListModel_Parameter()
        {
            SystemUnderTest.Language2CharCode.ShouldBe(StringOne);
            SystemUnderTest.Translation.ShouldBe(StringTwo);
        }
    }
}