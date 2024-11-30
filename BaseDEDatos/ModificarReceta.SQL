ALTER PROCEDURE ModificarReceta
   @id INT,                 
    @nombre VARCHAR(20),
    @receta VARCHAR(1000),
    @ingredientes VARCHAR(500),
    @porciones INT,
    @fk_dificultad INT
AS
BEGIN
    UPDATE Recetas
    SET 
        nombre = @nombre,
        receta = @receta,
        ingredientes = @ingredientes,
        porciones = @porciones,
        fk_dificultad = @fk_dificultad
    WHERE receta_id = @id;  
END;