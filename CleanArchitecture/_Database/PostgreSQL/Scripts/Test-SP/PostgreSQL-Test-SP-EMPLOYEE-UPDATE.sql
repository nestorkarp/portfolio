DO $$
DECLARE
    v_err_source character varying(10);
    v_err_msg character varying(255);
BEGIN
    -- CASO 1: Exitoso (Asumiendo que el ID 1 existe del test anterior)
    RAISE NOTICE '--- Test UPDATE Exitoso ---';
    CALL SP_EMPLOYEE_U(1, 'Juan Carlos', 'Pérez Modificado', '1990-05-15', TRUE, v_err_source, v_err_msg);
    RAISE NOTICE 'ErrorSource: %, ErrorMsg: %', v_err_source, v_err_msg;

    -- CASO 2: Erróneo (ID inexistente)
    RAISE NOTICE '--- Test UPDATE Erróneo (Inexistente) ---';
    CALL SP_EMPLOYEE_U(99999, 'Nadie', 'Inexistente', '2000-01-01', FALSE, v_err_source, v_err_msg);
    RAISE NOTICE 'ErrorSource: %, ErrorMsg: %', v_err_source, v_err_msg;
END $$;

