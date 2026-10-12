--<ScriptOptions statementTerminator="/"/>

CREATE OR REPLACE PROCEDURE RH.SP_EMPLOYEE_C (
    IN p_identitycard VARCHAR(11 OCTETS),
    IN p_firstname VARCHAR(30 OCTETS),
    IN p_lastname VARCHAR(30 OCTETS),
    IN p_birthdate VARCHAR(10),
    IN p_active SMALLINT,
    OUT p_Id INTEGER,
    OUT p_ErrorSource VARCHAR(10),
    OUT p_ErrorMessage VARCHAR(255)
)
LANGUAGE SQL
DYNAMIC RESULT SETS 0
BEGIN
    DECLARE SQLCODE INTEGER DEFAULT 0;
    DECLARE SQLSTATE CHAR(5) DEFAULT '00000';

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        IF (SUBSTR(SQLSTATE, 1, 5) = '23505') THEN
            SET p_id = 0;
            SET p_ErrorSource = 'VALIDATION';
            SET p_ErrorMessage = 'Registro Existente.';
        ELSE
            SET p_id = 0;
            SET p_ErrorSource = 'TECHNICAL';
            SET p_ErrorMessage = 'Error interno del sistema. SQLSTATE: ' || SQLSTATE;
        END IF;
    END;

    SET p_Id = 0;
    SET p_ErrorSource = '';
    SET p_ErrorMessage = '';

    INSERT INTO RH.EMPLOYEE (
        ELE_IDENTITYCARD,
        ELE_FIRSTNAME,
        ELE_LASTNAME,
        ELE_BIRTHDATE,
        ELE_ACTIVE
    ) VALUES (
        p_identitycard,
        p_firstname,
        p_lastname,
        DATE(p_birthdate),
        p_active
    );

    SET p_Id = IDENTITY_VAL_LOCAL();

END/

