-- Definición de variables para capturar los OUTPUTS
CREATE OR REPLACE VARIABLE v_out_src_u VARCHAR(10);
CREATE OR REPLACE VARIABLE v_out_msg_u VARCHAR(255);

-- [CASO EXITOSO]: Actualizar el registro ID = 1 (Se asume creado en el paso previo)
CALL RH.SP_EMPLOYEE_U(1, 'Juan Carlos', 'Perez Garcia', '1990-05-15', 1, v_out_src_u, v_out_msg_u);
SELECT 
    v_out_src_u  AS ORIGEN_ERROR, 
    v_out_msg_u  AS MENSAJE_ERROR 
FROM SYSIBM.SYSDUMMY1;

-- [CASO ERRÓNEO]: Intento de actualizar un ID inexistente (ej: ID = 99999)
CALL RH.SP_EMPLOYEE_U(99999, 'Test Fail', 'Test Fail', '2000-01-01', 0, v_out_src_u, v_out_msg_u);
SELECT 
    v_out_src_u  AS ORIGEN_ERROR, 
    v_out_msg_u  AS MENSAJE_ERROR 
FROM SYSIBM.SYSDUMMY1;