
using System.Diagnostics.CodeAnalysis;

public interface ICalculator
{
    int Add(int a, int b);
}

public class Calculator : ICalculator
{
    public int Add(int a, int b) => a + b;
}

public interface IEmailSender
{
    void Send(string to, string message);
}

public class NotificationService
{
    private readonly IEmailSender _emailSender;

    public NotificationService(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public void NotifyUser(string email)
    {
        _emailSender.Send(email, "Hello from NotificationService!");
    }
}

public interface IBugRepository
{
    List<Bug> GetAllBug();
}

public class Bug
{
    public int BugId { get; set; }
    public string Name { get; set; }
    public string Descripton { get; set; }
    public string Status { get; set; }
    public int UserStoryId { get; set; }
    public string Priority { get; set; }
    public string QaRemarks { get; set; }
    public int AssignMembersId { get; set; }
}

public class BugService
{
    private readonly IBugRepository _bugRepo;


    public BugService(IBugRepository bugRepo)
    {
        _bugRepo = bugRepo;
    }

    public List<Bug> GetAllBugByMember(List<Member> member)
    {
        try
        {
            var bugs = _bugRepo.GetAllBug().Join(member, b => b.AssignMembersId, m => m.MemberId, (b, m) =>
                new Bug
                {
                    BugId = b.BugId,
                    Name = b.Name,
                    Descripton = b.Descripton,
                    Status = b.Status,
                    UserStoryId = b.UserStoryId,
                    Priority = b.Priority,
                    QaRemarks = b.QaRemarks,
                    AssignMembersId = b.AssignMembersId,
                }).ToList();

            return bugs;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

public class Member
{
    public int MemberId { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }
}

public class Program
{
    [ExcludeFromCodeCoverage]
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}