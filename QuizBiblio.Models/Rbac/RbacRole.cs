namespace QuizBiblio.Models.Rbac;

public sealed class RbacRole(int id, string name)
{
    private const string AdminRole = "ADMIN";
    private const string MemberRole = "MEMBER";

    public const int AdminId = 0;
    public const int MemberId = 1;

    public static readonly RbacRole Member = new(MemberId, MemberRole);
    public static readonly RbacRole Admin = new(AdminId, AdminRole);

    public RbacRole() : this(MemberId, MemberRole) {}

    private int Id { get; init; } = id;
    public string Name { get; init; } = name;

}