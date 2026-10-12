-- Table: public.employee

-- DROP TABLE IF EXISTS public.employee;

CREATE TABLE IF NOT EXISTS public.employee
(
    ele_id serial NOT NULL,
    ele_identitycard character varying(11) COLLATE pg_catalog."default" NOT NULL,
    ele_firstname character varying(30) COLLATE pg_catalog."default" NOT NULL,
    ele_lastname character varying(30) COLLATE pg_catalog."default" NOT NULL,
    ele_birthdate date NOT NULL,
    ele_active boolean NOT NULL DEFAULT true,
    ele_audit_datetime timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT employee_pk PRIMARY KEY (ele_id),
    CONSTRAINT "employee_idx_identintyCard" UNIQUE (ele_identitycard)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.employee
    OWNER to postgres;



-- FUNCTION: public.employee_update_audit()

-- DROP FUNCTION IF EXISTS public.employee_update_audit();

CREATE OR REPLACE FUNCTION public.employee_update_audit()
    RETURNS trigger
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE NOT LEAKPROOF
AS $BODY$
BEGIN
    NEW.ele_audit_datetime = CURRENT_TIMESTAMP; 
    RETURN NEW;
END;
$BODY$;

ALTER FUNCTION public.employee_update_audit()
    OWNER TO postgres;



-- Trigger: tr_employee_update_audit

-- DROP TRIGGER IF EXISTS tr_employee_update_audit ON public.employee;

CREATE OR REPLACE TRIGGER tr_employee_update_audit
    BEFORE UPDATE 
    ON public.employee
    FOR EACH ROW
    EXECUTE FUNCTION public.employee_update_audit();





