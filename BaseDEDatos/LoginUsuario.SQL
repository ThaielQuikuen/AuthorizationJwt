CREATE PROCEDURE LoginUsuario
	@usuario VARCHAR(15)
AS 	
BEGIN 
	 SELECT usuario_id, password FROM Usuarios WHERE Usuarios.usuario  = '@usuario';
END;