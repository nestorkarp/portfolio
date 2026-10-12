-- Filtrar empleados activos
SELECT * FROM function_employee_r(0, NULL, true);

-- Filtrar por un ID específico
SELECT * FROM function_employee_r(1, '', NULL);

