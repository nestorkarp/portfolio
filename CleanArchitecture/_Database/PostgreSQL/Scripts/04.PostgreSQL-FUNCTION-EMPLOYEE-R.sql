-- FUNCTION: public.function_employee_r(integer, character varying, boolean)

-- DROP FUNCTION IF EXISTS public.function_employee_r(integer, character varying, boolean);

CREATE OR REPLACE FUNCTION public.function_employee_r(
	p_id integer,
	p_identitycard character varying,
	p_active boolean)
    RETURNS TABLE(employeeid integer, identitycard character varying, firstname character varying, lastname character varying, birthdate date, active boolean, auditdatetime text) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
BEGIN
    RETURN QUERY
    SELECT 
        ele_id AS employeeid,
        ele_identityCard AS identitycard,
        ele_firstname AS firstname,
        ele_lastname AS lastname,
        ele_birthdate AS birthdates,
        ele_active AS active,
        TO_CHAR(ele_audit_datetime, 'dd/mm/yyyy HH24:MI:SS') AS auditdatetime
    FROM employee
    WHERE 
        ( ((p_id IS NULL) OR (p_id = 0)) OR  (ele_id = p_id))
        AND ( ((p_identitycard IS NULL) OR (LENGTH(TRIM(p_identitycard)) = 0) ) OR (ele_identityCard = p_identitycard) )
        AND ( (p_active IS NULL) OR (ele_active = p_active) );
END;
$BODY$;


