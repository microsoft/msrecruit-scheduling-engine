// <copyright file="GetRoleAssignmentTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.UnitTest.FalconData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.DocumentDB;
    using MS.GTA.Common.Web.Contracts;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.Talent.FalconEntities.IV.Entity;

    /// <summary>
    /// Tests for GetRoleAssignment method
    /// </summary>
    [TestClass]
    public class GetRoleAssignmentTest
    {
        private FalconQuery falconQuery;
        private Mock<IFalconQueryClient> falconQueryClientMock;
        private Mock<ILogger<FalconQuery>> loggerMock;
        private Mock<IHcmDocumentClient> mockDocumentClient;

        [TestInitialize]
        public void BeforEach()
        {
            this.loggerMock = new Mock<ILogger<FalconQuery>>();
            this.falconQueryClientMock = new Mock<IFalconQueryClient>();
            this.mockDocumentClient = new Mock<IHcmDocumentClient>();
            this.falconQuery = new FalconQuery(
                this.falconQueryClientMock.Object,
                this.loggerMock.Object);
        }

        /// <summary>
        /// Test for Get Role Assignment Valid request
        /// </summary>
        /// <returns><see cref="Task"/> for the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetRoleAssignmentTestValid()
        {
            string userOid = Guid.NewGuid().ToString();
            JobIVUser user = new JobIVUser()
            {
                Person = new Talent.TalentContracts.InterviewService.IVPerson { ObjectId = userOid },
                Roles = new List<IVApplicationRole> { IVApplicationRole.IVReadOnly },
            };

            this.falconQueryClientMock
                .Setup(c => c.GetFalconClient())
                .ReturnsAsync(this.mockDocumentClient.Object);
            this.falconQueryClientMock
                .Setup(c => c.GetFalconClient(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(this.mockDocumentClient.Object);
            this.mockDocumentClient
                .Setup(c => c.GetFirstOrDefault(It.IsAny<Expression<Func<JobIVUser, bool>>>(), null))
                .ReturnsAsync(user);

            var result = await this.falconQuery.GetRoleAssignment(userOid);

            Assert.IsNotNull(result, "Roles to be not null.");
            Assert.AreEqual(result.Count(), 1, "Role Count to be 1");

            this.mockDocumentClient
                .Verify(c => c.GetFirstOrDefault(It.IsAny<Expression<Func<JobIVUser, bool>>>(), null), Times.Once, "Expected to get roles once.");
        }
    }
}
