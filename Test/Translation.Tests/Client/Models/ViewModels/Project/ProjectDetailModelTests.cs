﻿using NUnit.Framework;
using Shouldly;

using Translation.Client.Web.Models.Project;
using Translation.Common.Helpers;
using static Translation.Tests.TestHelpers.FakeModelTestHelper;
using static Translation.Tests.TestHelpers.AssertViewModelTestHelper;

namespace Translation.Tests.Client.Models.ViewModels.Project
{
    [TestFixture]
    public class ProjectDetailModelTests
    {
        public ProjectDetailModel SystemUnderTest { get; set; }

        [SetUp]
        public void run_before_every_test()
        {
            SystemUnderTest = GetOrganizationOneProjectOneDetailModel();
        }

        [Test]
        public void ProjectCloneModel_Title()
        {
            Assert.AreEqual(SystemUnderTest.Title, "project_detail_title");
        }

        [Test]
        public void ProjectCloneModel_OrganizationUidInput()
        {
            AssertHiddenInputModel(SystemUnderTest.OrganizationUidInput, "OrganizationUid");
        }

        [Test]
        public void ProjectDetailModel_ProjectUidInput()
        {
            AssertHiddenInputModel(SystemUnderTest.ProjectUidInput, "ProjectUid");
        }

        [Test]
        public void ProjectDetailModel_IsActiveInput()
        {
            AssertCheckboxInputModel(SystemUnderTest.IsActiveInput, "IsActive", "is_active", false, true);
        }

        [Test]
        public void ProjectCloneModel_SetInputModelValues()
        {
            // arrange

            // act
            SystemUnderTest.SetInputModelValues();

            // assert
            SystemUnderTest.OrganizationUidInput.Value.ShouldBe(SystemUnderTest.OrganizationUid.ToUidString());
            SystemUnderTest.ProjectUidInput.Value.ShouldBe(SystemUnderTest.ProjectUid.ToUidString());
        }
    }
}
