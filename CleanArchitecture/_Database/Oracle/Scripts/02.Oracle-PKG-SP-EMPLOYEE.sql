--------------------------------------------------------
--  DDL for Procedure SPE_EMPLOYEE_C
--------------------------------------------------------
set define off;

  CREATE OR REPLACE NONEDITIONABLE PROCEDURE "HR"."SPE_EMPLOYEE_C" (
    p_IdentityCard IN  VARCHAR2,
    p_FirstName    IN  VARCHAR2,
    p_LastName     IN  VARCHAR2,
    p_Birthdate    IN  DATE,
    p_Active       IN  NUMBER,
    p_Id           OUT NUMBER,
    p_ErrorSource   OUT VARCHAR2,
    p_ErrorMessage  OUT VARCHAR2
) AS
BEGIN
    p_Id         := NULL;
    p_ErrorSource := NULL;
    p_ErrorMessage  := NULL;

    INSERT INTO HR.EMPLOYEE (
        ELE_IDENTITYCARD, ELE_FIRSTNAME, ELE_LASTNAME, ELE_BIRTHDATE, ELE_ACTIVE
    ) VALUES (
        p_IdentityCard, p_FirstName, p_LastName, p_Birthdate, p_Active
    )
    RETURNING ELE_ID INTO p_Id;

EXCEPTION
    WHEN DUP_VAL_ON_INDEX THEN
        p_Id         := NULL;
        p_ErrorSource := 'VALIDACION';
        p_ErrorMessage   := 'Registro Existente.';
    WHEN OTHERS THEN
        p_Id         := NULL;
        p_ErrorSource := 'TECHNICAL';
        p_ErrorMessage   := SQLERRM;
END SPE_Employee_C;

/
--------------------------------------------------------
--  DDL for Procedure SPE_EMPLOYEE_R
--------------------------------------------------------
set define off;

  CREATE OR REPLACE NONEDITIONABLE PROCEDURE "HR"."SPE_EMPLOYEE_R" (
    p_Id           IN  NUMBER,
    p_IdentityCard IN  VARCHAR2,
    p_Active       IN  NUMBER,
    p_Cursor       OUT SYS_REFCURSOR,
    p_ErrorSource   OUT VARCHAR2,
    p_ErrorMessage  OUT VARCHAR2
) AS
BEGIN
    p_ErrorSource := NULL;
    p_ErrorMessage   := NULL;

   OPEN p_Cursor FOR
    SELECT ELE_ID AS EmployeeId,
           ELE_IDENTITYCARD AS IdentityCard,
           ELE_FIRSTNAME AS FirstName, ELE_LASTNAME AS LastName,
           TO_CHAR(ELE_BIRTHDATE,'DD/MM/YYYY') AS Birthdate,
           ELE_ACTIVE AS Active,
           TO_CHAR(ELE_AUDIT_DATETIME, 'DD/MM/YYYY HH24:MI:SS')  AS Audit_DateTime
      FROM HR.EMPLOYEE
     WHERE ((p_Id = 0) OR (ELE_ID = p_Id))
       AND (  ( (p_IdentityCard IS NULL) OR (LENGTH(TRIM(p_IdentityCard))= 0))  OR (ELE_IDENTITYCARD = p_IdentityCard))
       AND ((p_Active IS NULL) OR (ELE_ACTIVE = p_Active));


EXCEPTION
    WHEN OTHERS THEN
        p_ErrorSource := 'TECHNICAL';
        p_ErrorMessage   := SQLERRM;
        IF p_Cursor%ISOPEN THEN
            CLOSE p_Cursor;
        END IF;
END SPE_Employee_R;

/
--------------------------------------------------------
--  DDL for Procedure SPE_EMPLOYEE_U
--------------------------------------------------------
set define off;

  CREATE OR REPLACE NONEDITIONABLE PROCEDURE "HR"."SPE_EMPLOYEE_U" (
    p_Id           IN  NUMBER,
    p_FirstName    IN  VARCHAR2,
    p_LastName     IN  VARCHAR2,
    p_Birthdate    IN  DATE,
    p_Active       IN  NUMBER,
    p_ErrorSource   OUT VARCHAR2,
    p_ErrorMessage  OUT VARCHAR2
) AS
BEGIN
    p_ErrorSource := NULL;
    p_ErrorMessage   := NULL;

    UPDATE HR.EMPLOYEE
       SET ELE_FIRSTNAME = p_FirstName,
           ELE_LASTNAME  = p_LastName,
           ELE_BIRTHDATE = p_Birthdate,
           ELE_ACTIVE    = p_Active
     WHERE (ELE_ID = p_Id);

    IF SQL%ROWCOUNT = 0 THEN
        p_ErrorSource := 'VALIDATION';
        p_ErrorMessage := 'Registro Inexistente.';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        p_ErrorSource := 'TECHNICAL';
        p_ErrorMessage := SQLERRM;
END SPE_Employee_U;

/
--------------------------------------------------------
--  DDL for Package PKG_EMPLOYEE
--------------------------------------------------------

  CREATE OR REPLACE NONEDITIONABLE PACKAGE "HR"."PKG_EMPLOYEE" AS

    -- Procedimiento para Insertar Empleado
    PROCEDURE SP_Employee_C (
        p_IdentityCard IN  VARCHAR2,
        p_FirstName    IN  VARCHAR2,
        p_LastName     IN  VARCHAR2,
        p_Birthdate    IN  DATE,
        p_Active       IN  NUMBER,
        p_Id            OUT NUMBER,
        p_ErrorSource   OUT VARCHAR2,
        p_ErrorMessage  OUT VARCHAR2
    );

    -- Procedimiento para Actualizar Empleado
    PROCEDURE SP_Employee_U (
        p_Id           IN  NUMBER,
        p_FirstName    IN  VARCHAR2,
        p_LastName     IN  VARCHAR2,
        p_Birthdate    IN  DATE,
        p_Active       IN  NUMBER,
        p_ErrorSource   OUT VARCHAR2,
        p_ErrorMessage  OUT VARCHAR2
    );

    -- Procedimiento para Leer Empleados (Retorna Cursor)
    PROCEDURE SP_Employee_R (
        p_Id           IN  NUMBER,
        p_IdentityCard IN  VARCHAR2,
        p_Active       IN  NUMBER,
        p_Cursor       OUT SYS_REFCURSOR,
        p_ErrorSource   OUT VARCHAR2,
        p_ErrorMessage     OUT VARCHAR2
    );

END PKG_EMPLOYEE;

/
--------------------------------------------------------
--  DDL for Package Body PKG_EMPLOYEE
--------------------------------------------------------

  CREATE OR REPLACE NONEDITIONABLE PACKAGE BODY "HR"."PKG_EMPLOYEE" AS

  PROCEDURE SP_Employee_C (
        p_IdentityCard IN  VARCHAR2,
        p_FirstName    IN  VARCHAR2,
        p_LastName     IN  VARCHAR2,
        p_Birthdate    IN  DATE,
        p_Active       IN  NUMBER,
        p_Id           OUT NUMBER,
        p_ErrorSource  OUT VARCHAR2,
        p_ErrorMessage OUT VARCHAR2
    ) AS
  BEGIN
        -- Invocación directa al stored procedure externo
        SPE_Employee_C(
            p_IdentityCard, p_FirstName, p_LastName, p_Birthdate, p_Active, 
            p_Id, p_ErrorSource, p_ErrorMessage 
        );
  END SP_Employee_C;

  PROCEDURE SP_Employee_U (
        p_Id           IN  NUMBER,
        p_FirstName    IN  VARCHAR2,
        p_LastName     IN  VARCHAR2,
        p_Birthdate    IN  DATE,
        p_Active       IN  NUMBER,
        p_ErrorSource   OUT VARCHAR2,
        p_ErrorMessage  OUT VARCHAR2
    ) AS
  BEGIN
     -- Invocación directa al stored procedure externo
        SPE_Employee_U(
            p_Id, p_FirstName, p_LastName, p_Birthdate, p_Active, 
            p_ErrorSource, p_ErrorMessage 
        );
  END SP_Employee_U;

  PROCEDURE SP_Employee_R (
        p_Id           IN  NUMBER,
        p_IdentityCard IN  VARCHAR2,
        p_Active       IN  NUMBER,
        p_Cursor       OUT SYS_REFCURSOR,
        p_ErrorSource  OUT VARCHAR2,
        p_ErrorMessage OUT VARCHAR2
    ) AS
  BEGIN
     -- Invocación directa al stored procedure externo
        SPE_Employee_R(
            p_Id, p_IdentityCard, p_Active, 
            p_Cursor, p_ErrorSource, p_ErrorMessage 
        );
  END SP_Employee_R;

END PKG_EMPLOYEE;

/
