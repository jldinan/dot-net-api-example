namespace Api.Models;

public class User
{
    /*C# 11 required attributes*/
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}
