﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pipeliner.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
}
