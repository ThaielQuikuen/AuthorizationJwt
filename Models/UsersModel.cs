namespace ATDapi.Models;

public class UsersModel : Queries{

    public int? id {get; set;}
    public string username {get; set;}
    public string password {get; set;}
    public int role {get; set;}


    public override string insert(){
        return string.Format($"INSERT INTO Users(username,password,fk_role) VALUES('{username}','{password}',{1})");
    }

}