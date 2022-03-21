namespace Pipeliner.Models;

public class UserGetResponse
{
    public string Id { get; set; }
    public string Name { get; set; }

    public UserGetResponse(string id, string name)
    {
        Id = id;
        Name = name;
    }
}
