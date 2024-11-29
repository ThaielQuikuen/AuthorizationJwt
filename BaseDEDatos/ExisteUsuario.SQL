CREATE PROCEDURE ExisteUsuario
	@usuario2 VARCHAR(15)
AS 	
BEGIN 
	 SELECT usuario FROM Usuarios WHERE Usuarios.usuario  = '@usuario2';
END;
