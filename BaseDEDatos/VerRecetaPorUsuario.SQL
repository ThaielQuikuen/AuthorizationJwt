CREATE PROCEDURE VerRecetaPorUsuario
	@usuario_id int
AS 	
BEGIN 
	 SELECT * FROM Recetas WHERE fk_usuario = @usuario_id;
END;