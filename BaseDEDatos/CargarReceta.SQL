CREATE PROCEDURE CargarReceta
	@nombre VARCHAR(20),
	@receta VARCHAR(1000),
	@ingredientes VARCHAR(500),
	@porciones INT,
	@fk_dificultad INT,
	@fk_usuario INT
AS
BEGIN
	INSERT INTO Recetas(nombre, receta, ingredientes, porciones, fk_dificultad, fk_usuario) 
	VALUES('@nombre', '@receta', '@ingredientes', @porciones, @fk_dificultad, @fk_usuario);
END;
