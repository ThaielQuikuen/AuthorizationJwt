CREATE PROCEDURE EliminarReceta
	@receta_id int
AS 	
BEGIN 
	DELETE FROM Recetas WHERE receta_id = @receta_id;
END;
