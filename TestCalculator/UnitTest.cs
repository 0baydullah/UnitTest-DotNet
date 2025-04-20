using Castle.Core.Logging;
using NSubstitute;

namespace TestCalculator
{
    public class UnitTest
    {
        [Fact]
        public void Add_TowIntegerNumber_Sum()
        {
            //arrange
            var calculator = new Calculator();
            int a = 5;
            int b = 10;
            int expected = 15;

            //act
            var result = calculator.Add(a, b);

            //assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5, 10, 15)]
        [InlineData(0, 0, 0)]
        [InlineData(-5, -10, -15)]
        [InlineData(5, -10, -5)]
        [InlineData(-5, 10, 5)]
        [InlineData(int.MaxValue, 1, int.MinValue)]
        [InlineData(int.MinValue, -1, int.MaxValue)]
        [InlineData(int.MaxValue, int.MaxValue, -2)]
        [InlineData(int.MinValue, int.MinValue, 0)]
        [InlineData(0, int.MaxValue, int.MaxValue)]
        [InlineData(0, int.MinValue, int.MinValue)]
        [InlineData(1, 1, 2)]
        [InlineData(-1, -1, -2)]
        [InlineData(1, -1, 0)]
        [InlineData(-1, 1, 0)]
        [InlineData(100, 200, 300)]
        [InlineData(-100, -200, -300)]
        [InlineData(100, -200, -100)]
        public void Add_TwoIntegerNumber_Sum(int a, int b, int expected)
        {
            //arrange
            var calculator = new Calculator();
            //act
            var result = calculator.Add(a, b);
            //assert
            Assert.Equal(expected, result);
        }
    }

    public class NotificationServiceTests
    {
        [Fact]
        public void NotifyUser_ValidEmail_CallsSendWithCorrectMessage()
        {
            // Arrange
            var mockEmailSender = Substitute.For<IEmailSender>();
            var service = new NotificationService(mockEmailSender);

            // Act
            service.NotifyUser("user@example.com");

            // Assert
            mockEmailSender.Received().Send("user@example.com", "Hello from NotificationService!");
        }
    }

    public class BugServiceTests
    {
        [Fact]
        public void GetAllBugByMember_MatchingMembers_ReturnsFilteredBugs()
        {
            // Arrange
            var bugRepo = Substitute.For<IBugRepository>();
            var service = new BugService(bugRepo);

            var allBugs = new List<Bug>
            {
                new Bug { BugId = 1, Name = "Bug A", AssignMembersId = 101 },
                new Bug { BugId = 2, Name = "Bug B", AssignMembersId = 102 },
                new Bug { BugId = 3, Name = "Bug C", AssignMembersId = 103 }
            };

            var members = new List<Member>
            {
                new Member { MemberId = 101, Name = "John" },
                new Member { MemberId = 102, Name = "Alice" },
                new Member { MemberId = 103, Name = "Bob" }
            };

            bugRepo.GetAllBug().Returns(allBugs);

            // Act
            var result = service.GetAllBugByMember(members);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, b => b.BugId == 1);
            Assert.Contains(result, b => b.BugId == 2);
            Assert.Contains(result, b => b.BugId == 3);
        }

        [Fact]
        public void GetAllBugByMember_WhenExceptionThrown_Rethrows()
        {
            // Arrange
            var bugRepo = Substitute.For<IBugRepository>();
            var service = new BugService(bugRepo);

            bugRepo.GetAllBug().Returns(x => throw new Exception("DB error"));

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => service.GetAllBugByMember(new List<Member>()));
            Assert.Equal("DB error", exception.Message);
        }
    }
}