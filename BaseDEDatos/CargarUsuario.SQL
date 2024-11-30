ALTER PROCEDURE CargarUsuario
	@usuario VARCHAR(15),
	@password VARCHAR(256)
AS 	
BEGIN 
	 INSERT INTO Usuarios(usuario, password) 
	 VALUES(@usuario,@password);
END;