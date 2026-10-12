--<ScriptOptions statementTerminator="/"/>

CREATE OR REPLACE PROCEDURE RH.SP_EMPLOYEE_R (
    IN p_id INTEGER,
    IN p_identitycard VARCHAR(11 OCTETS),
    IN p_active SMALLINT,
    OUT p_ErrorSource VARCHAR(10),
    OUT p_ErrorMessage VARCHAR(255)
)
LANGUAGE SQL
DYNAMIC RESULT SETS 1
BEGIN
        DECLARE SQLSTATE CHAR(5) DEFAULT '00000';

        DECLARE c_employee CURSOR WITH RETURN TO CALLER FOR
        SELECT 
            ELE_ID AS EmployeeId,
            ELE_IDENTITYCARD AS IdentityCard,
            ELE_FIRSTNAME AS Firstname,
            ELE_LASTNAME AS Lastname,
            VARCHAR_FORMAT(ELE_BIRTHDATE,'DD-MM-YYYY') AS Birthdate,
            ELE_ACTIVE AS Active,
            VARCHAR_FORMAT(ELE_AUDIT_DATETIME, 'DD-MM-YYYY HH24:MI:SS') AS AuditDateTime
        FROM RH.EMPLOYEE
        WHERE 
            ( (p_id = 0) OR (ELE_ID = p_id) )
            AND ( ((p_identitycard IS NULL) OR (OCTET_LENGTH(p_identitycard) = 0)) OR (ELE_IDENTITYCARD = p_identitycard) )
            AND ((p_active IS NULL) OR (ELE_ACTIVE = p_active));

        DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_ErrorSource = 'TECHNICAL';
        SET p_ErrorMessage = 'Error al ejecutar la consulta. SQLSTATE: ' || SQLSTATE;
    END;

        SET p_ErrorSource = '';
        SET p_ErrorMessage = '';

        OPEN c_employee;

END/