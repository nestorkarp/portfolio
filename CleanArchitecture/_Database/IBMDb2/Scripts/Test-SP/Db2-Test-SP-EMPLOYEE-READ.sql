CREATE OR REPLACE VARIABLE v_out_src_r VARCHAR(10);
CREATE OR REPLACE VARIABLE v_out_msg_r VARCHAR(255);

-- [CASO EXITOSO 1]: Leer de forma general trayendo todos los empleados activos (ID=0, Cedula=NULL, Active=1)
CALL RH.SP_EMPLOYEE_R(0, NULL, 1, v_out_src_r, v_out_msg_r);
SELECT 
    v_out_src_r  AS ORIGEN_ERROR, 
    v_out_msg_r  AS MENSAJE_ERROR 
FROM SYSIBM.SYSDUMMY1;

-- [CASO EXITOSO 2]: Buscar por un ID específico y cédula con string de longitud cero ''
CALL RH.SP_EMPLOYEE_R(1, '', NULL, v_out_src_r, v_out_msg_r);
SELECT 
    v_out_src_r  AS ORIGEN_ERROR, 
    v_out_msg_r  AS MENSAJE_ERROR 
FROM SYSIBM.SYSDUMMY1;

-- [CASO EXITOSO 3]: Buscar usando una cédula exacta
CALL RH.SP_EMPLOYEE_R(0, '12345678901', NULL, v_out_src_r, v_out_msg_r);
SELECT 
    v_out_src_r  AS ORIGEN_ERROR, 
    v_out_msg_r  AS MENSAJE_ERROR 
FROM SYSIBM.SYSDUMMY1;