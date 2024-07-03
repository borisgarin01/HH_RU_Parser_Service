CREATE OR REPLACE FUNCTION GetAverageSalary(
    scheduleidInput VARCHAR(31), 
    experienceIdInput VARCHAR(31)
)
RETURNS TABLE (
    average decimal
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT (avg(salaries.salaryfrom)+avg(salaries.salaryto))/2
    FROM items 
    LEFT JOIN addresses ON items.addressid = addresses.id
    LEFT JOIN salaries ON items.id = salaries.id
    WHERE items.scheduleid = scheduleidInput AND items.experienceid = experienceIdInput;
END;
$$;

SELECT * FROM GetAverageSalary('remote', 'noExperience');