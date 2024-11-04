public abstract class Queries{

    public abstract string insert();

    public string select(string tabla)
    {
        return string.Format($"SELECT * FROM {tabla}");
    }
    public string selectbyUser()
    {
        return string.Format($"SELECT usuario  FROM Usuarios WHERE Usuarios.usuario  = ''");
    }
}