DO $$
DECLARE
    v_id INTEGER;
    v_err_source character varying(10);
    v_err_msg character varying(255);
BEGIN
    -- CASO 1: Exitoso
    RAISE NOTICE '--- Test INSERT Exitoso ---';
    CALL SP_EMPLOYEE_C('12345678901', 'Juan', 'Pérez', '1990-05-15', true, v_id, v_err_source, v_err_msg);
    RAISE NOTICE 'ID Generado: %, ErrorSource: %, ErrorMsg: %', v_id, v_err_source, v_err_msg;

    -- CASO 2: Erróneo (Duplicado de Cédula de Identidad)
    RAISE NOTICE '--- Test INSERT Erróneo (Duplicado) ---';
    CALL SP_EMPLOYEE_C('12345678901', 'Pedro', 'Gómez', '1985-08-20', TRUE, v_id, v_err_source, v_err_msg);
    RAISE NOTICE 'ID Generado: %, ErrorSource: %, ErrorMsg: %', v_id, v_err_source, v_err_msg;
END $$;
