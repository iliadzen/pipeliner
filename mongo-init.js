db = db.getSiblingDB('db');

db.createCollection('Users');
db.Users.insertMany([
    {
        "Name": "User1",
        "Password": "Pass1"
    },
    {
        "Name": "User2",
        "Password": "Pass2"
    }
])
