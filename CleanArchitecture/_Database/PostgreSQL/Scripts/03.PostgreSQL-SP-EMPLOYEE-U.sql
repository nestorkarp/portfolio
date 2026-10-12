-- PROCEDURE: public.sp_employee_u(integer, text, text, date, boolean)

-- DROP PROCEDURE IF EXISTS public.sp_employee_u(integer, text, text, date, boolean);

CREATE OR REPLACE PROCEDURE public.sp_employee_u(
	IN p_id integer,
	IN p_firstname text,
	IN p_lastname text,
	IN p_birthdate date,
	IN p_active boolean,
	OUT p_errorsource text,
	OUT p_errormessage text)
LANGUAGE 'plpgsql'
AS $BODY$
DECLARE
    v_rows_updated INTEGER;
BEGIN
    p_errorsource := NULL;
    p_errormessage := NULL;

    -- Ejecución de la actualización
    UPDATE EMPLOYEE
    SET ele_firstname = p_firstname,
        ele_lastname = p_lastname,
        ele_birthdate = p_birthdate,
        ele_active = p_active
    WHERE (ele_id = p_id);

    -- Obtener la cantidad de filas afectadas
    GET DIAGNOSTICS v_rows_updated = ROW_COUNT;

    IF v_rows_updated = 0 THEN
        p_errorsource := 'VALIDATION';
        p_errormessage := 'Registro Inexistente.';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        p_errorsource := 'TECHNICAL';
        p_errormessage := SQLERRM;
END;
$BODY$;
ALTER PROCEDURE public.sp_employee_u(integer, text, text, date, boolean)
    OWNER TO postgres;

