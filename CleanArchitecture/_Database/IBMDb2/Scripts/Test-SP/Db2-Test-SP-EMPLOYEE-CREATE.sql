CREATE OR REPLACE VARIABLE v_out_id INT;
CREATE OR REPLACE VARIABLE v_out_src VARCHAR(10);
CREATE OR REPLACE VARIABLE v_out_msg VARCHAR(255);

-- [CASO EXITOSO]: Inserción normal de un empleado limpio
CALL RH.SP_EMPLOYEE_C('12345678901', 'Juan', 'Perez', '1990-05-15', 1, v_out_id, v_out_src, v_out_msg);
-- Imprimir salidas esperadas (ID autogenerado, Src vacío, Msg vacío)
SELECT 
    v_out_id   AS ID_GENERADO, 
    v_out_src  AS ORIGEN_ERROR, 
    v_out_msg  AS MENSAJE_ERROR 
FROM SYSIBM.SYSDUMMY1;

SELECT * FROM RH.EMPLOYEE;

-- [CASO ERRÓNEO]: Intento de duplicar la misma cédula '12345678901'
CALL RH.SP_EMPLOYEE_C('12345678901', 'Maria', 'Gomez', '1992-08-20', 1, v_out_id, v_out_src, v_out_msg);
-- Imprimir salidas esperadas (ID = 0, Src = 'VALIDATION', Msg = 'Registro Existente.')
SELECT 
    v_out_id   AS ID_GENERADO, 
    v_out_src  AS ORIGEN_ERROR, 
    v_out_msg  AS MENSAJE_ERROR 
FROM SYSIBM.SYSDUMMY1;

SELECT * FROM RH.EMPLOYEE;


 