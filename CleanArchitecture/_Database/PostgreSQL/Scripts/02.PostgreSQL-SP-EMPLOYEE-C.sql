-- PROCEDURE: public.sp_employee_c(text, text, text, date, boolean)

-- DROP PROCEDURE IF EXISTS public.sp_employee_c(text, text, text, date, boolean);

CREATE OR REPLACE PROCEDURE public.sp_employee_c(
	IN p_identitycard text,
	IN p_firstname text,
	IN p_lastname text,
	IN p_birthdate date,
	IN p_active boolean,
	OUT p_id integer,
	OUT p_errorsource text,
	OUT p_errormessage text)
LANGUAGE 'plpgsql'
AS $BODY$
BEGIN
    -- Inicializamos los parámetros de error como vacíos / éxito
    p_errorsource := NULL;
    p_errormessage := NULL;
    p_id := 0;

    -- Intento de inserción en la tabla
    INSERT INTO employee (ele_identitycard, ele_firstname, ele_lastname, ele_birthdate, ele_active)
    VALUES (p_identitycard, p_firstname, p_lastname, p_birthdate, p_active)
    RETURNING ele_id INTO p_id;

EXCEPTION
    -- Captura de error por duplicidad de índice (Unique Violation)
    WHEN unique_violation THEN
        p_id := 0;
        p_errorsource := 'VALIDATION';
        p_errormessage := 'Registro Existente.';
        
    -- Captura de cualquier otro error inesperado (Error Técnico)
    WHEN OTHERS THEN
        p_id := 0;
        p_errorsource := 'TECHNICAL';
        p_errormessage := SQLERRM;
END;
$BODY$;
ALTER PROCEDURE public.sp_employee_c(text, text, text, date, boolean)
    OWNER TO postgres;

