﻿using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Shouldly;

using Translation.Client.Web.Controllers;
using Translation.Client.Web.Models.Base;
using Translation.Client.Web.Models.Organization;
using Translation.Common.Tests.SetupHelpers;
using static Translation.Common.Tests.TestHelpers.ActionMethodNameConstantTestHelper;
using static Translation.Common.Tests.TestHelpers.FakeConstantTestHelper;
using static Translation.Common.Tests.TestHelpers.AssertViewModelTestHelper;
using static Translation.Common.Tests.TestHelpers.FakeModelTestHelper;

namespace Translation.Client.Web.Unit.Tests.Controllers
{
    [TestFixture]
    public class OrganizationControllerTests : ControllerBaseTests
    {
        public OrganizationController SystemUnderTest { get; set; }

        [SetUp]
        public void run_before_every_test()
        {
            Refresh();
            SystemUnderTest = Container.Resolve<OrganizationController>();
            SetControllerContext(SystemUnderTest);
        }

        [TestCase(DetailAction, new[] { typeof(Guid) }, typeof(HttpGetAttribute)),
         TestCase(EditAction, new[] { typeof(Guid) }, typeof(HttpGetAttribute)),
         TestCase(EditAction, new[] { typeof(OrganizationEditModel) }, typeof(HttpPostAttribute)),
         TestCase(RevisionsAction, new[] { typeof(Guid) }, typeof(HttpGetAttribute)),
         TestCase(RevisionsDataAction, new[] { typeof(Guid) }, typeof(HttpGetAttribute)),
         TestCase(RestoreAction, new[] { typeof(Guid), typeof(int) }, typeof(HttpPostAttribute)),
         TestCase(PendingTranslationsAction, new Type[] { }, typeof(HttpGetAttribute)),
         TestCase(PendingTranslationsDataAction, new[] { typeof(Guid), typeof(int), typeof(int) }, typeof(HttpGetAttribute)),
         TestCase(UserLoginLogListAction, new[] { typeof(Guid) }, typeof(HttpGetAttribute)),
         TestCase(UserLoginLogListDataAction, new[] { typeof(Guid), typeof(int), typeof(int) }, typeof(HttpGetAttribute)),
         TestCase(UserListDataAction, new[] { typeof(Guid), typeof(int), typeof(int) }, typeof(HttpGetAttribute)),
         TestCase(IntegrationListDataAction, new[] { typeof(Guid), typeof(int), typeof(int) }, typeof(HttpGetAttribute)),
         TestCase(ProjectListDataAction, new[] { typeof(Guid), typeof(int), typeof(int) }, typeof(HttpGetAttribute)),
         TestCase(TokenRequestLogListAction, new[] { typeof(Guid) }, typeof(HttpGetAttribute)),
         TestCase(TokenRequestLogListDataAction, new[] { typeof(Guid), typeof(int), typeof(int) }, typeof(HttpGetAttribute)),
         TestCase(JournalListAction, new[] { typeof(Guid) }, typeof(HttpGetAttribute)),
         TestCase(JournalListDataAction, new[] { typeof(Guid), typeof(int), typeof(int) }, typeof(HttpGetAttribute))]
        public void Methods_Has_Http_Verb_Attributes(string actionMethod, Type[] parameters, Type httpVerbAttribute)
        {
            var type = SystemUnderTest.GetType();
            var methodInfo = type.GetMethod(actionMethod, parameters);
            var attributes = methodInfo.GetCustomAttributes(httpVerbAttribute, true);
            Assert.AreEqual(attributes.Length, 1);
        }

        [Test]
        public void Controller_Derived_From_BaseController()
        {
            var type = SystemUnderTest.GetType();
            type.BaseType.Name.StartsWith("BaseController").ShouldBeTrue();
        }

        [Test]
        public void Detail_GET()
        {
            // arrange
            MockOrganizationService.Setup_GetOrganization_Returns_OrganizationReadResponse_Success();

            // act
            var result = SystemUnderTest.Detail(UidOne);

            // assert
            AssertViewWithModel<OrganizationDetailModel>(result);
            MockOrganizationService.Verify_GetOrganization();
        }

        [Test]
        public void Detail_GET_FailedResponse()
        {
            // arrange
            MockOrganizationService.Setup_GetOrganization_Returns_OrganizationReadResponse_Failed();

            // act
            var result = SystemUnderTest.Detail(UidOne);

            // assert
            AssertViewAccessDenied(result);
            MockOrganizationService.Verify_GetOrganization();
        }

        [Test]
        public void Detail_GET_InvalidResponse()
        {
            // arrange
            MockOrganizationService.Setup_GetOrganization_Returns_OrganizationReadResponse_Invalid();

            // act
            var result = SystemUnderTest.Detail(UidOne);

            // assert
            AssertViewAccessDenied(result);
            MockOrganizationService.Verify_GetOrganization();
        }

        [Test]
        public void Detail_GET_InvalidParameter()
        {
            // arrange
            MockOrganizationService.Setup_GetOrganization_Returns_OrganizationReadResponse_Success();

            // act
            var result = SystemUnderTest.Detail(EmptyUid);

            // assert
            AssertViewWithModel<OrganizationDetailModel>(result);
            MockOrganizationService.Verify_GetOrganization();
        }

        [Test]
        public void Edit_GET()
        {
            // arrange
            MockOrganizationService.Setup_GetOrganization_Returns_OrganizationReadResponse_Success();

            // act
            var result = SystemUnderTest.Edit(UidOne);

            // assert
            AssertViewWithModel<OrganizationEditModel>(result);
            MockOrganizationService.Verify_GetOrganization();
        }

        [Test]
        public void Edit_GET_FailedResponse()
        {
            // arrange
            MockOrganizationService.Setup_GetOrganization_Returns_OrganizationReadResponse_Failed();

            // act
            var result = SystemUnderTest.Edit(UidOne);

            // assert
            AssertViewAccessDenied(result);
            MockOrganizationService.Verify_GetOrganization();
        }

        [Test]
        public void Edit_GET_InvalidResponse()
        {
            // arrange
            MockOrganizationService.Setup_GetOrganization_Returns_OrganizationReadResponse_Invalid();

            // act
            var result = SystemUnderTest.Edit(UidOne);

            // assert
            AssertViewAccessDenied(result);
            MockOrganizationService.Verify_GetOrganization();
        }

        [Test]
        public void Edit_GET_InvalidParameter()
        {
            // arrange
            MockOrganizationService.Setup_GetOrganization_Returns_OrganizationReadResponse_Invalid();

            // act
            var result = SystemUnderTest.Edit(EmptyUid);

            // assert
            AssertView<OrganizationEditModel>(result);
            MockOrganizationService.Verify_GetOrganization();
        }

        [Test]
        public async Task Edit_POST()
        {
            // arrange
            MockOrganizationService.Setup_EditOrganization_Returns_OrganizationEditResponse_Success();
            var model = GetOrganizationOneOrganizationEditModel();

            // act
            var result = await SystemUnderTest.Edit(model);

            // assert
            ((RedirectResult)result).Url.ShouldBe($"/Organization/Detail/{OrganizationOneUid}");
            MockOrganizationService.Verify_EditOrganization();
        }

        [Test]
        public async Task Edit_POST_FailedResponse()
        {
            // arrange
            MockOrganizationService.Setup_EditOrganization_Returns_OrganizationEditResponse_Failed();
            var model = GetOrganizationOneOrganizationEditModel();

            // act
            var result = await SystemUnderTest.Edit(model);

            // assert
            AssertErrorMessagesForInvalidOrFailedResponse<OrganizationEditModel>(result);
            MockOrganizationService.Verify_EditOrganization();
        }

        [Test]
        public async Task Edit_POST_InvalidResponse()
        {
            // arrange
            MockOrganizationService.Setup_EditOrganization_Returns_OrganizationEditResponse_Invalid();
            var model = GetOrganizationOneOrganizationEditModel();

            // act
            var result = await SystemUnderTest.Edit(model);

            // assert
            AssertErrorMessagesForInvalidOrFailedResponse<OrganizationEditModel>(result);
            MockOrganizationService.Verify_EditOrganization();
        }

        [Test]
        public async Task Edit_POST_InvalidModel()
        {
            // arrange
            var model = new OrganizationEditModel();

            // act
            var result = await SystemUnderTest.Edit(model);

            // assert
            AssertInputErrorMessagesOfView(result, model);
        }

        [Test]
        public void Revisions_GET()
        {
            // arrange
            MockOrganizationService.Setup_GetOrganization_Returns_OrganizationReadResponse_Success();

            // act
            var result = SystemUnderTest.Revisions(OrganizationOneProjectOneUid);

            // assert
            AssertViewWithModel<OrganizationRevisionReadListModel>(result);
            MockOrganizationService.Verify_GetOrganization();
        }

        [Test]
        public void Revisions_GET_FailedResponse()
        {
            // arrange
            MockOrganizationService.Setup_GetOrganization_Returns_OrganizationReadResponse_Failed();

            // act
            var result = SystemUnderTest.Revisions(OrganizationOneProjectOneUid);

            // assert
            AssertView<NotFoundResult>(result);
            MockOrganizationService.Verify_GetOrganization();
        }

        [Test]
        public void Revisions_GET_InvalidResponse()
        {
            // arrange
            MockOrganizationService.Setup_GetOrganization_Returns_OrganizationReadResponse_Invalid();

            // act
            var result = SystemUnderTest.Revisions(OrganizationOneProjectOneUid);

            // assert
            AssertView<NotFoundResult>(result);
            MockOrganizationService.Verify_GetOrganization();
        }

        [Test]
        public void Revisions_GET_InvalidParameter()
        {
            // arrange

            // act
            var result = SystemUnderTest.Revisions(EmptyUid);

            // assert
            AssertViewRedirectToHome(result);
        }

        [Test]
        public void RevisionsData_GET()
        {
            // arrange
            MockOrganizationService.Setup_GetOrganizationRevisions_Returns_OrganizationRevisionReadListResponse_Success();

            // act
            var result = SystemUnderTest.RevisionsData(OrganizationOneProjectOneUid);

            // assert
            AssertViewAndHeaders(result, new[] { "revision", "revisioned_by", "revisioned_at", "organization_name", "is_active", "created_at", "" });
            MockOrganizationService.Verify_GetOrganizationRevisions();
        }

        [Test]
        public void RevisionsData_GET_FailedResponse()
        {
            // arrange
            MockOrganizationService.Setup_GetOrganizationRevisions_Returns_OrganizationRevisionReadListResponse_Failed();

            // act
            var result = SystemUnderTest.RevisionsData(OrganizationOneProjectOneUid);

            // assert
            AssertView<NotFoundResult>(result);
            MockOrganizationService.Verify_GetOrganizationRevisions();
        }

        [Test]
        public void RevisionsData_GET_InvalidResponse()
        {
            // arrange
            MockOrganizationService.Setup_GetOrganizationRevisions_Returns_OrganizationRevisionReadListResponse_Invalid();

            // act
            var result = SystemUnderTest.RevisionsData(OrganizationOneProjectOneUid);

            // assert
            AssertView<NotFoundResult>(result);
            MockOrganizationService.Verify_GetOrganizationRevisions();
        }

        [Test]
        public void RevisionsData_GET_InvalidParameter()
        {
            // arrange

            // act
            var result = SystemUnderTest.RevisionsData(EmptyUid);

            // assert
            AssertView<ForbidResult>(result);
        }

        [Test]
        public async Task Restore_Post()
        {
            // arrange
            MockOrganizationService.Setup_RestoreOrganization_Returns_OrganizationRestoreResponse_Success();

            // act
            var result = await SystemUnderTest.Restore(OrganizationOneProjectOneUid, One);

            // assert
            AssertView<JsonResult>(result);
            MockOrganizationService.Verify_RestoreOrganization();
        }

        [Test]
        public async Task Restore_Post_FailedResponse()
        {
            // arrange
            MockOrganizationService.Setup_RestoreOrganization_Returns_OrganizationRestoreResponse_Failed();

            // act
            var result = (JsonResult)await SystemUnderTest.Restore(OrganizationOneProjectOneUid, One);

            // assert
            ((CommonResult)result.Value).IsOk.ShouldBe(false);
            MockOrganizationService.Verify_RestoreOrganization();
        }

        [Test]
        public async Task Restore_Post_InvalidResponse()
        {
            // arrange
            MockOrganizationService.Setup_RestoreOrganization_Returns_OrganizationRestoreResponse_Invalid();

            // act
            var result = (JsonResult)await SystemUnderTest.Restore(OrganizationOneProjectOneUid, One);

            // assert
            ((CommonResult)result.Value).IsOk.ShouldBe(false);
            MockOrganizationService.Verify_RestoreOrganization();
        }

        [Test]
        public async Task Restore_Post_InvalidParameter()
        {
            // arrange

            // act
            var result = (JsonResult)await SystemUnderTest.Restore(EmptyUid, One);

            // assert
            ((CommonResult)result.Value).IsOk.ShouldBe(false);
        }

        [Test]
        public async Task Restore_Post_InvalidParameterRevisionZero()
        {
            // arrange

            // act
            var result = (JsonResult)await SystemUnderTest.Restore(UidOne, Zero);

            // assert
            ((CommonResult)result.Value).IsOk.ShouldBe(false);
        }

        [Test]
        public void PendingTranslations_GET()
        {
            // arrange
            
            // act
            var result = SystemUnderTest.PendingTranslations();

            // assert
            AssertViewWithModel<OrganizationPendingTranslationReadListModel>(result);
        }

        [Test]
        public void PendingTranslationsData_GET()
        {
            // arrange
            MockOrganizationService.Setup_GetPendingTranslations_Returns_OrganizationPendingTranslationReadListResponse_Success();

            // act
            var result = SystemUnderTest.PendingTranslationsData(OrganizationOneProjectOneUid, One, Two);

            // assert
            AssertView<JsonResult>(result);
            MockOrganizationService.Verify_GetPendingTranslations();
        }

        [Test]
        public void PendingTranslationsData_GET_FailedResponse()
        {
            // arrange
            MockOrganizationService.Setup_GetPendingTranslations_Returns_OrganizationPendingTranslationReadListResponse_Failed();

            // act
            var result = SystemUnderTest.PendingTranslationsData(OrganizationOneProjectOneUid, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
            MockOrganizationService.Verify_GetPendingTranslations();
        }

        [Test]
        public void PendingTranslationsData_GET_InvalidResponse()
        {
            // arrange
            MockOrganizationService.Setup_GetPendingTranslations_Returns_OrganizationPendingTranslationReadListResponse_Invalid();

            // act
            var result = SystemUnderTest.PendingTranslationsData(OrganizationOneProjectOneUid, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
            MockOrganizationService.Verify_GetPendingTranslations();
        }

        [Test]
        public void PendingTranslationsData_GET_InvalidParameter()
        {
            // arrange

            // act
            var result = SystemUnderTest.PendingTranslationsData(EmptyUid, One, Two);

            // assert
            AssertView<ForbidResult>(result);
        }

        [TestCase(10, 10)]
        [TestCase(10, 1000)]
        [TestCase(-10, 10)]
        [TestCase(10, -10)]
        [TestCase(1000, 10)]
        public async Task PendingTranslationsData_GET_SetPaging(int skip, int take)
        {
            // arrange
            MockOrganizationService.Setup_GetPendingTranslations_Returns_OrganizationPendingTranslationReadListResponse_Success();

            // act
            var result = (JsonResult)await SystemUnderTest.PendingTranslationsData(OrganizationOneProjectOneUid, skip, take);

            // assert
            AssertView<DataResult>(result);
            AssertPagingInfo(result);
            MockOrganizationService.Verify_GetPendingTranslations();
        }

        [Test]
        public void UserLoginLogList_GET()
        {
            // arrange

            // act
            var result = SystemUnderTest.UserLoginLogList(UidOne);

            // assert
            AssertViewWithModel<OrganizationUserLoginLogListModel>(result);
        }

        [Test]
        public void UserLoginLogList_GET_InvalidParameter()
        {
            // arrange

            // act
            var result = SystemUnderTest.UserLoginLogList(EmptyUid);

            // assert
            AssertViewWithModel<OrganizationUserLoginLogListModel>(result);
        }

        [Test]
        public void UserLoginLogListData_GET()
        {
            // arrange
            MockOrganizationService.Setup_GetUserLoginLogsOfOrganization_Returns_OrganizationLoginLogReadListResponse_Success();

            // act
            var result = SystemUnderTest.UserLoginLogListData(UidOne, One, Two);

            // assert
            AssertView<JsonResult>(result);
            MockOrganizationService.Verify_GetUserLoginLogsOfOrganization();
        }

        [TestCase(10, 10)]
        [TestCase(10, 1000)]
        [TestCase(-10, 10)]
        [TestCase(10, -10)]
        [TestCase(1000, 10)]
        public async Task UserLoginLogListData_GET_SetPaging(int skip, int take)
        {
            // arrange
            MockOrganizationService.Setup_GetUserLoginLogsOfOrganization_Returns_OrganizationLoginLogReadListResponse_Success();

            // act
            var result = (JsonResult)await SystemUnderTest.UserLoginLogListData(UidOne, skip, take);

            // assert
            AssertView<DataResult>(result);
            AssertPagingInfo(result);
        }

        [Test]
        public void UserLoginLogListData_GET_FailedResponse()
        {
            // arrange
            MockOrganizationService.Setup_GetUserLoginLogsOfOrganization_Returns_OrganizationLoginLogReadListResponse_Failed();

            // act
            var result = SystemUnderTest.UserLoginLogListData(UidOne, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
            MockOrganizationService.Verify_GetUserLoginLogsOfOrganization();
        }

        [Test]
        public void UserLoginLogListData_GET_InvalidResponse()
        {
            // arrange
            MockOrganizationService.Setup_GetUserLoginLogsOfOrganization_Returns_OrganizationLoginLogReadListResponse_Invalid();

            // act
            var result = SystemUnderTest.UserLoginLogListData(UidOne, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
            MockOrganizationService.Verify_GetUserLoginLogsOfOrganization();
        }

        [Test]
        public void UserLoginLogListData_GET_InvalidParameter()
        {
            // arrange

            // act
            var result = SystemUnderTest.UserLoginLogListData(EmptyUid, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
        }

        [Test]
        public void UserListData_GET()
        {
            // arrange
            MockOrganizationService.Setup_GetUsers_Returns_UserReadListResponse_Success();

            // act
            var result = SystemUnderTest.UserListData(UidOne, One, Two);

            // assert
            AssertView<JsonResult>(result);
            MockOrganizationService.Verify_GetUsers();
        }

        [TestCase(10, 10)]
        [TestCase(10, 1000)]
        [TestCase(-10, 10)]
        [TestCase(10, -10)]
        [TestCase(1000, 10)]
        public async Task UserListData_GET_SetPaging(int skip, int take)
        {
            // arrange
            MockOrganizationService.Setup_GetUsers_Returns_UserReadListResponse_Success();

            // act
            var result = (JsonResult)await SystemUnderTest.UserListData(UidOne, skip, take);

            // assert
            AssertView<DataResult>(result);
            AssertPagingInfo(result);
        }

        [Test]
        public void UserListData_GET_FailedResponse()
        {
            // arrange
            MockOrganizationService.Setup_GetUsers_Returns_UserReadListResponse_Failed();

            // act
            var result = SystemUnderTest.UserListData(UidOne, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
            MockOrganizationService.Verify_GetUsers();
        }

        [Test]
        public void UserListData_GET_InvalidResponse()
        {
            // arrange
            MockOrganizationService.Setup_GetUsers_Returns_UserReadListResponse_Invalid();

            // act
            var result = SystemUnderTest.UserListData(UidOne, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
            MockOrganizationService.Verify_GetUsers();
        }

        [Test]
        public void UserListData_GET_InvalidParameter()
        {
            // arrange

            // act
            var result = SystemUnderTest.UserListData(EmptyUid, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
        }

        [Test]
        public void IntegrationListData_GET()
        {
            // arrange
            MockIntegrationService.Setup_GetIntegrations_Returns_IntegrationReadListResponse_Success();

            // act
            var result = SystemUnderTest.IntegrationListData(UidOne, One, Two);

            // assert
            AssertView<JsonResult>(result);
            MockIntegrationService.Verify_GetIntegrations();
        }

        [TestCase(10, 10)]
        [TestCase(10, 1000)]
        [TestCase(-10, 10)]
        [TestCase(10, -10)]
        [TestCase(1000, 10)]
        public async Task IntegrationListData_GET_SetPaging(int skip, int take)
        {
            // arrange
            MockIntegrationService.Setup_GetIntegrations_Returns_IntegrationReadListResponse_Success();

            // act
            var result = (JsonResult)await SystemUnderTest.IntegrationListData(UidOne, skip, take);

            // assert
            AssertView<DataResult>(result);
            AssertPagingInfo(result);
        }

        [Test]
        public void IntegrationListData_GET_FailedResponse()
        {
            // arrange
            MockIntegrationService.Setup_GetIntegrations_Returns_IntegrationReadListResponse_Failed();

            // act
            var result = SystemUnderTest.IntegrationListData(UidOne, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
            MockIntegrationService.Verify_GetIntegrations();
        }

        [Test]
        public void IntegrationListData_GET_InvalidResponse()
        {
            // arrange
            MockIntegrationService.Setup_GetIntegrations_Returns_IntegrationReadListResponse_Invalid();

            // act
            var result = SystemUnderTest.IntegrationListData(UidOne, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
            MockIntegrationService.Verify_GetIntegrations();
        }

        [Test]
        public void IntegrationListData_GET_InvalidParameter()
        {
            // arrange

            // act
            var result = SystemUnderTest.IntegrationListData(EmptyUid, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
        }

        [Test]
        public void ProjectListData_GET()
        {
            // arrange
            MockProjectService.Setup_GetProjects_Returns_ProjectReadListResponse_Success();

            // act
            var result = SystemUnderTest.ProjectListData(UidOne, One, Two);

            // assert
            AssertView<JsonResult>(result);
            MockProjectService.Verify_GetProjects();
        }

        [TestCase(10, 10)]
        [TestCase(10, 1000)]
        [TestCase(-10, 10)]
        [TestCase(10, -10)]
        [TestCase(1000, 10)]
        public async Task ProjectListData_GET_SetPaging(int skip, int take)
        {
            // arrange
            MockProjectService.Setup_GetProjects_Returns_ProjectReadListResponse_Success();

            // act
            var result = (JsonResult)await SystemUnderTest.ProjectListData(UidOne, skip, take);

            // assert
            AssertView<DataResult>(result);
            AssertPagingInfo(result);
        }

        [Test]
        public void ProjectListData_GET_FailedResponse()
        {
            // arrange
            MockProjectService.Setup_GetProjects_Returns_ProjectReadListResponse_Failed();

            // act
            var result = SystemUnderTest.ProjectListData(UidOne, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
            MockProjectService.Verify_GetProjects();
        }

        [Test]
        public void ProjectListData_GET_InvalidResponse()
        {
            // arrange
            MockProjectService.Setup_GetProjects_Returns_ProjectReadListResponse_Invalid();

            // act
            var result = SystemUnderTest.ProjectListData(UidOne, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
            MockProjectService.Verify_GetProjects();
        }

        [Test]
        public void ProjectListData_GET_InvalidParameter()
        {
            // arrange

            // act
            var result = SystemUnderTest.ProjectListData(EmptyUid, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
        }

        [Test]
        public void TokenRequestLogList_GET()
        {
            // arrange

            // act
            var result = SystemUnderTest.TokenRequestLogList(UidOne);

            // assert
            AssertViewWithModel<OrganizationTokenRequestLogListModel>(result);
        }

        [Test]
        public void TokenRequestLogList_GET_InvalidParameter()
        {
            // arrange

            // act
            var result = SystemUnderTest.TokenRequestLogList(EmptyUid);

            // assert
            AssertViewWithModel<OrganizationTokenRequestLogListModel>(result);
        }

        [Test]
        public void TokenRequestLogListData_GET()
        {
            // arrange
            MockIntegrationService.Setup_GetTokenRequestLogsOfOrganization_Returns_OrganizationTokenRequestLogReadListResponse_Success();

            // act
            var result = SystemUnderTest.TokenRequestLogListData(UidOne, One, Two);

            // assert
            AssertView<JsonResult>(result);
            MockIntegrationService.Verify_GetTokenRequestLogsOfOrganization();

        }

        [TestCase(10, 10)]
        [TestCase(10, 1000)]
        [TestCase(-10, 10)]
        [TestCase(10, -10)]
        [TestCase(1000, 10)]
        public async Task TokenRequestLogListData_GET_SetPaging(int skip, int take)
        {
            // arrange
            MockIntegrationService.Setup_GetTokenRequestLogsOfOrganization_Returns_OrganizationTokenRequestLogReadListResponse_Success();

            // act
            var result = (JsonResult)await SystemUnderTest.TokenRequestLogListData(UidOne, skip, take);

            // assert
            AssertView<DataResult>(result);
            AssertPagingInfo(result);
        }

        [Test]
        public void TokenRequestLogListData_GET_FailedResponse()
        {
            // arrange
            MockIntegrationService.Setup_GetTokenRequestLogsOfOrganization_Returns_OrganizationTokenRequestLogReadListResponse_Failed();

            // act
            var result = SystemUnderTest.TokenRequestLogListData(UidOne, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
            MockIntegrationService.Verify_GetTokenRequestLogsOfOrganization();
        }

        [Test]
        public void TokenRequestLogListData_GET_InvalidResponse()
        {
            // arrange
            MockIntegrationService.Setup_GetTokenRequestLogsOfOrganization_Returns_OrganizationTokenRequestLogReadListResponse_Invalid();

            // act
            var result = SystemUnderTest.TokenRequestLogListData(UidOne, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
            MockIntegrationService.Verify_GetTokenRequestLogsOfOrganization();
        }

        [Test]
        public void TokenRequestLogListData_GET_InvalidParameter()
        {
            // arrange

            // act
            var result = SystemUnderTest.TokenRequestLogListData(EmptyUid, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
        }

        [Test]
        public void JournalList_GET()
        {
            // arrange

            // act
            var result = SystemUnderTest.JournalList(OrganizationOneProjectOneUid);

            // assert
            AssertViewWithModel<OrganizationJournalListModel>(result);
        }

        [Test]
        public void JournalList_GET_InvalidParameter()
        {
            // arrange

            // act
            var result = SystemUnderTest.JournalList(EmptyUid);

            // assert
            AssertViewWithModel<OrganizationJournalListModel>(result);
        }

        [Test]
        public void JournalListData_GET_Success()
        {
            // arrange
            MockJournalService.Setup_GetJournalsOfOrganization_Returns_OrganizationJournalReadListResponse_Success();

            // act
            var result = SystemUnderTest.JournalListData(OrganizationOneProjectOneUid, One, Two);

            // assert
            AssertViewAndHeaders(result, new[] { "user_name", "integration_name", "message", "created_at" });
            MockJournalService.Verify_GetJournalsOfOrganization();
        }

        [Test]
        public void JournalListData_GET_UserUidAndIntegrationUidEmpty_Success()
        {
            // arrange
            MockJournalService.Setup_GetJournalsOfOrganization_Returns_OrganizationJournalReadListResponse_UserUidAndIntegrationUidEmpty_Success();

            // act
            var result = SystemUnderTest.JournalListData(OrganizationOneProjectOneUid, One, Two);

            // assert
            AssertViewAndHeaders(result, new[] { "user_name", "integration_name", "message", "created_at" });
            MockJournalService.Verify_GetJournalsOfOrganization();
        }

        [Test]
        public void JournalListData_GET_FailedResponse()
        {
            // arrange
            MockJournalService.Setup_GetJournalsOfOrganization_Returns_OrganizationJournalReadListResponse_Failed();

            // act
            var result = SystemUnderTest.JournalListData(OrganizationOneProjectOneUid, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
            MockJournalService.Verify_GetJournalsOfOrganization();
        }

        [Test]
        public void JournalListData_GET_InvalidResponse()
        {
            // arrange
            MockJournalService.Setup_GetJournalsOfOrganization_Returns_OrganizationJournalReadListResponse_Invalid();

            // act
            var result = SystemUnderTest.JournalListData(OrganizationOneProjectOneUid, One, Two);

            // assert
            AssertView<NotFoundResult>(result);
            MockJournalService.Verify_GetJournalsOfOrganization();
        }

        [Test]
        public void JournalListData_GET_InvalidParameter()
        {
            // arrange

            // act
            var result = SystemUnderTest.JournalListData(EmptyUid, One, Two);

            // assert
            AssertView<ForbidResult>(result);
        }

        [TestCase(10, 10)]
        [TestCase(10, 1000)]
        [TestCase(-10, 10)]
        [TestCase(10, -10)]
        [TestCase(1000, 10)]
        public async Task JournalListData_GET_SetPaging(int skip, int take)
        {
            // arrange
            MockJournalService.Setup_GetJournalsOfOrganization_Returns_OrganizationJournalReadListResponse_Success();

            // act
            var result = (JsonResult)await SystemUnderTest.JournalListData(OrganizationOneProjectOneUid, skip, take);

            // assert
            AssertView<DataResult>(result);
            AssertPagingInfo(result);
        }
    }
}