ALTER PROCEDURE ExisteUsuario
	@usuario VARCHAR(15)
AS 	
BEGIN 
	 SELECT usuario FROM Usuarios WHERE usuario  = @usuario;
END;
