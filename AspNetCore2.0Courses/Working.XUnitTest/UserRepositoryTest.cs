using Moq;
using System;
using System.Data;
using Working.Models.Repository;
using Xunit;
using Dapper;
using Working.Models.DataModel;
using System.Collections.Generic;
using Moq.Dapper;

namespace Working.XUnitTest
{
    /// <summary>
    /// ”√ªß≤÷¥¢≤‚ ‘
    /// </summary>
    [Trait("≤÷¥¢≤„", "UserRepository")]
    public class UserRepositoryTest
    {
        #region µ«¬º≤‚ ‘
        /// <summary>
        /// ≤‚ ‘µ«¬º’˝≥£÷µ
        /// </summary>
        [Fact]
        public void Login_Default_Return()
        {
            var dbMock = new Mock<IDbConnection>();
            var userRepository = new UserRepository(dbMock.Object, "");

            var list = new List<UserRole>() {
                new UserRole{ ID=1, Name="πÀÿŒ∞", DepartmentID=1, Password="gsw", RoleID=1, RoleName="Leader", UserName="gsw" }
            };
            dbMock.SetupDapper(db => db.Query<UserRole>(It.IsAny<string>(), null, null, true, null, null)).Returns(list);

            var userRole = userRepository.Login("gsw", "gsw");

            Assert.NotNull(userRole);


        }
        /// <summary>
        /// ≤‚ ‘µ«¬º”√ªß√˚ªÚ√‹¬Î¥ÌŒÛ
        /// </summary>
        [Fact]
        public void Login_Null_ThrowException()
        {
            var dbMock = new Mock<IDbConnection>();
            var userRepository = new UserRepository(dbMock.Object, "");

            var list = new List<UserRole>();
            dbMock.SetupDapper(db => db.Query<UserRole>(It.IsAny<string>(), null, null, true, null, null)).Returns(list);

            var exc = Assert.Throws<Exception>(() => { userRepository.Login("gsw", "gsw"); });

            Assert.Contains("”√ªß√˚ªÚ√‹¬Î¥ÌŒÛ£°", exc.Message);
        }

        #endregion
    }
}
