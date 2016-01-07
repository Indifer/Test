namespace csharp Server

struct User { 
    1: i32 ID 
    2: string Name 
  }


service UserService { 
    User GetUserByID(1:i32 userID)
    list<User> GetAllUser()  
}