--<ScriptOptions statementTerminator="/"/>

CREATE OR REPLACE PROCEDURE RH.SP_EMPLOYEE_U (
    IN p_id INTEGER,
    IN p_firstname VARCHAR(30 OCTETS),
    IN p_lastname VARCHAR(30 OCTETS),
    IN p_birthdate VARCHAR(10),
    IN p_active SMALLINT,
    OUT p_ErrorSource VARCHAR(10),
    OUT p_ErrorMessage VARCHAR(255)
)
LANGUAGE SQL
DYNAMIC RESULT SETS 0
BEGIN
    DECLARE SQLCODE INTEGER DEFAULT 0;
    DECLARE SQLSTATE CHAR(5) DEFAULT '00000';

    DECLARE v_rows_updated INTEGER DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_ErrorSource = 'TECHNICAL';
        SET p_ErrorMessage = 'Error al actualizar el registro. SQLSTATE: ' || SQLSTATE;
    END;

        SET p_ErrorSource = '';
        SET p_ErrorMessage = '';

        UPDATE RH.EMPLOYEE
        SET ELE_FIRSTNAME = p_firstname,
            ELE_LASTNAME  = p_lastname,
            ELE_BIRTHDATE = DATE(p_birthdate),
            ELE_ACTIVE    = p_active
        WHERE ELE_ID = p_id;

        GET DIAGNOSTICS v_rows_updated = ROW_COUNT;

    IF (v_rows_updated = 0) THEN
        SET p_ErrorSource = 'VALIDATION';
        SET p_ErrorMessage = 'Registro Inexistente.';
    END IF;

END/